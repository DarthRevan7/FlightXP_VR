using NUnit.Framework;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UIElements;
using static PlanePhy2;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class PlanePhy2 : MonoBehaviour
{
    public AircraftModel model = new AircraftModel();

    public float timeDilation = 10.0f;

    public Vector3 startRPY  = new Vector3 (0, 0, 1);

    public bool autopilotEngaged = true;

    [SerializeField]
    private InputActionReference roll, pitch, yaw, throttle, autopilot;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        model.initAviaB534();

        model.Position = new  Vector3(transform.position.z, transform.position.x, -transform.position.y);

        //model.AngularVelocity = new Vector3(0,1,0);

        model.Velocity = new Vector3(50, 0,0);

        //model.RPY = startRPY;
    }

    void Update()
    {
        if(autopilot != null && autopilot.action.IsPressed())
        {
            ToggleAutopilot(); 
        }
    }

    public void ToggleAutopilot()
    {
        autopilotEngaged = !autopilotEngaged;
    }

    public float getAltitude()
    {
        return -model.Position.z;
    }

    public float getSpeed()
    {
        return model.Velocity.magnitude;
    }

    public Vector3 getAttitude()
    {
        return model.RPY;
    }

    public float getThrottle()
    {
        return model.Throttle;
    }

    public float getVerticalSpeed()
    {
        Vector3 w_speed = model.GenerateRPYToWorldMatrix() * model.Velocity;

        return w_speed.z; 
    }

    public Vector3 offset = Vector3.zero;
    public void increaseOffset(Vector3 pos)
    {
        offset += pos;
    }

    public Vector3 getWorldPosition()
    {
        return new Vector3(model.Position.y, -model.Position.z, model.Position.x);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float deltaT = Time.deltaTime/ timeDilation;

        //Input
        float roll_in = roll.action.ReadValue<float>();
        float pitch_in = pitch.action.ReadValue<float>();
        float yaw_in = yaw.action.ReadValue<float>();

        float throttle_in = model.Throttle;

        if (throttle.action.ReadValue<float>() != 0) 
        {
            throttle_in = throttle.action.ReadValue<float>();
        }



        if(autopilotEngaged)
        {
            float target_roll = 0;

            float target_speed = 50;

            //float target_vspeed = 0;
            float target_alt = 500;

            roll_in = - (model.RPY.x- target_roll);

            throttle_in = model.Throttle;

            throttle_in += 0.001f * (target_speed - getSpeed());

            //pitch_in = -(target_vspeed - getVerticalSpeed()) - model.AngularVelocity.y;

            float pitching_control_target = (target_alt-getAltitude()) / 10.0f;

            if (pitching_control_target > 0.5f) pitching_control_target = 0.5f;
            if (pitching_control_target < -0.5f) pitching_control_target = -0.5f;

            pitch_in = model.PitchInput;

            //pitch_in += -model.AngularAcceleration.y - model.AngularVelocity.y - (model.RPY.y - pitching_control_target);

            pitch_in += -model.AngularAcceleration.y - model.AngularVelocity.y - (model.RPY.y - 0.1f);

            //Debug.Log($"Speed: {getSpeed()}");
        }

        //if (roll.action.ReadValue<float>() > 0) roll_in = -1;
        //else if (roll.action.ReadValue<float>() < 0) roll_in = 1;
        //if (Input.GetKey(KeyCode.W)) pitch_in = -1;
        //else if (Input.GetKey(KeyCode.S)) pitch_in = 1;
        //if (Input.GetKey(KeyCode.Q)) yaw_in = -1;
        //else if (Input.GetKey(KeyCode.E)) yaw_in = 1;
        //if (Input.GetKey(KeyCode.LeftControl)) throttle_in = -1;
        //else if (Input.GetKey(KeyCode.LeftShift)) throttle_in = 1;

        model.control(pitch_in, roll_in, yaw_in, throttle_in, deltaT);

        model.FrameUpdate(deltaT);

        // x,y,z  ->  x, -y, -z
        // x,y,z  ->  -y, z, x

        // -> y, -z, x
        //

        transform.position = getWorldPosition() + offset;

       // transform.RotateAround()

        transform.rotation = Quaternion.Euler(-Rad2Deg(model.RPY.y), Rad2Deg(model.RPY.z), -Rad2Deg(model.RPY.x));

        //transform.Translate(AircraftModel.ToUnity(-model.CenterOfMass));
    }

    public enum ForceType
    {
        Base,
        Weight,
        Lift,
        Drag
    }

    public struct Force
    {
        public Vector3 ForceVector; // Force vector [N]
        public Vector3 Position; // Point of application [m]
        public ForceType Type;
    }

    public struct Surface
    {
        public Vector3 Center; // Aerodynamic center
        public Vector3 SurfaceArea; // Surface cross section area in 3 directions {perpendicular, Parallel, wide}
        public Vector3 Perpendicular; // Direction of surface movement if alpha = 0
        public Vector3 Parallel; // Direction of aerodynamic centerline
        public Vector3 Top; // Up direction of profiles

        public void CalculateTop()
        {
            Top = Vector3.Cross(Perpendicular, Parallel).normalized;
        }
    }

    public struct AircraftStatus
    {
        public bool Damaged;
        public bool CriticallyDamaged;
        public bool Destroyed;
        public bool Exploded;
    }

    public struct PlaneControls
    {
        public float PitchControl;
        public float RollControl;
        public float YawControl;
        public float Throttle;
        public bool TriggerDown;
    }

    public static float Deg2Rad(float x)
    {
        return x / 180.0f * Mathf.PI;
    }

    public static float Rad2Deg(float x)
    {
        return x * 180.0f / Mathf.PI;
    }

    public class AircraftModel
    {
        public Vector3 Position; // Position [m] NED
        public Vector3 Velocity; // Velocity [m/s] body
        public Vector3 RPY; // Attitude [rad]
        public Vector3 AngularVelocity; // Angular speed (RPY) [rad/s]

        public Vector3 Acceleration;
        public Vector3 AngularAcceleration;

        public float Mass; // [kg]
        public Vector3 CenterOfMass; // [m]
        public Vector3 MomentOfInertia;

        public List<Surface> Surfaces = new List<Surface>();
        public List<Surface> DragElements = new List<Surface>();
        public List<Force> Forces = new List<Force>();

        public float Throttle = 1.0f;
        public float ThrottleControlSpeed = 0.5f;
        public float PropellerMaxThrust;
        public Vector3 PropellerThrustPosition;

        public float PitchInput = 0.0f;
        public float PitchControlSpeed = 2.0f;
        public float MaxElevatorAngle;

        public float RollInput = 0.0f;
        public float RollControlSpeed = 2.0f;
        public float MaxFlapAngle;

        public float YawInput = 0.0f;
        public float YawControlSpeed = 2.0f;
        public float MaxVerticalTailAngle;

        public float ClAlpha;
        public float ClZero;
        public float MinAngleOfAttack;
        public float MaxAngleOfAttack;
        public float LiftCoefficient;

        public float RateOfFire;
        public float FireCooldown;
        public int FireSide;
        public int Hit;
        public int HitsToDestroy = 4;
        public float ParticleEmitterTimer;
        public float SmokeEmitterTimer;
        public float ExhaustEmitterTimer;
        public int Trigger;
        public float TriggerTime;
        public int FireId = -1;
        public float PropellerAngle;

        public AircraftStatus Status;

        public Surface RightWing;
        public Surface LeftWing;
        public Surface RightHorizontalStabilizer;
        public Surface LeftHorizontalStabilizer;
        public Surface VerticalStabilizer;
        public Surface RightFlap;
        public Surface LeftFlap;

        public float MaxSimulationStep = 0.002f;

        public void FullFrameUpdate(float deltaTime)
        {
            if (Status.Destroyed) return;

            FireCooldown -= deltaTime;

            while (deltaTime > MaxSimulationStep)
            {
                FrameUpdate(MaxSimulationStep);
                deltaTime -= MaxSimulationStep;
            }

            PropellerAngle += Throttle * deltaTime * 400 + Velocity.x * deltaTime;
            PropellerAngle = PropellerAngle % (2.0f * Mathf.PI);

            FrameUpdate(deltaTime);

            if (Hit > 0)
            {
                Status.Damaged = true;
                if (Hit >= HitsToDestroy) Status.CriticallyDamaged = true;
            }
        }

        public void FrameUpdate(float deltaTime)
        {
            Forces.Clear();

            //Surfaces.Add(LeftFlap);
            //Surfaces.Add(RightFlap);
            //Surfaces.Add(LeftWing);
            //Surfaces.Add(RightWing);
            //Surfaces.Add(RightHorizontalStabilizer);
            //Surfaces.Add(LeftHorizontalStabilizer);
            //Surfaces.Add(VerticalStabilizer);

            generateProceduralForces(ref Forces);

            //Surfaces.Clear();

            {
                Matrix4x4 rpy_w = GenerateRPYToWorldMatrix();

                foreach(var force  in Forces)
                {
                    Vector3 w_pos = (rpy_w * force.Position);
                    w_pos += Position;

                    Vector3 w_end = (rpy_w * force.ForceVector)/1000.0f;
                    w_end += w_pos;

                    Color c = Color.white;

                    if (force.Type == ForceType.Weight) c = Color.green;
                    else if (force.Type == ForceType.Lift) c = Color.blue;
                    else if (force.Type == ForceType.Drag) c = Color.red;

                    Debug.DrawLine(ToUnity(w_pos), ToUnity(w_end), c);
                    //Debug.DrawLine(ToUnity(world_w_pos), ToUnity(world_w_pos + world_w_f / 1000.0f), Color.red);
                }
            }
            

            

            Vector3 totalForce = Vector3.zero;
            Vector3 totalTorque = Vector3.zero;

            //Debug.Log($"throttle: {Forces[1].ForceVector}, {Forces[1].Position}");

            foreach (var force in Forces)
            {
                totalForce += force.ForceVector;
                Vector3 r = force.Position - CenterOfMass;
                totalTorque += Vector3.Cross(r, force.ForceVector);
            }

            //Debug.Log($"force: {totalForce}, torque: {totalTorque}, nforces: {Forces.Count}");
            //Debug.Log($"RPY: {RPY}");

            Vector3 acc =  new Vector3( 0.0f, 0.0f, 0.0f );
            Vector3 ang_acc = new Vector3(0.0f, 0.0f, 0.0f);

            acc.x = (totalForce.x / Mass - AngularVelocity.y * Velocity.z + AngularVelocity.z * Velocity.y);
            acc.y = (totalForce.y / Mass - AngularVelocity.z * Velocity.x + AngularVelocity.x * Velocity.z);
            acc.z = (totalForce.z / Mass - AngularVelocity.x * Velocity.y + AngularVelocity.y * Velocity.x);
            //std::cout << "acc: " << acc.x << " " << acc.y << " " << acc.z << "\n";
            Acceleration = acc;

            ang_acc.x = (-(MomentOfInertia.z - MomentOfInertia.y) * AngularVelocity.y * AngularVelocity.z + totalTorque.x) / MomentOfInertia.x;
            ang_acc.y = (-(MomentOfInertia.x - MomentOfInertia.z) * AngularVelocity.x * AngularVelocity.z + totalTorque.y) / MomentOfInertia.y;
            ang_acc.z = (-(MomentOfInertia.y - MomentOfInertia.x) * AngularVelocity.x * AngularVelocity.y + totalTorque.z) / MomentOfInertia.z;

            AngularAcceleration = ang_acc;

            Velocity += acc * deltaTime;
            AngularVelocity += ang_acc * deltaTime;


            float dphi = AngularVelocity.x + AngularVelocity.y * Mathf.Sin(RPY.x) * Mathf.Tan(RPY.y) + AngularVelocity.z * Mathf.Cos(RPY.x) * Mathf.Tan(RPY.y);//phi = roll theta = pitch
            float dteta = (AngularVelocity.y * Mathf.Cos(RPY.x) - AngularVelocity.z * Mathf.Sin(RPY.x));
            float dpsi = (AngularVelocity.y * Mathf.Sin(RPY.x) + AngularVelocity.z * Mathf.Cos(RPY.x)) / Mathf.Cos(RPY.y);

            Matrix4x4 rpy_to_world = GenerateRPYToWorldMatrix();


            Vector3 dworld = rpy_to_world * new Vector4(Velocity.x,  Velocity.y,  Velocity.z, 0.0f);

            Position += dworld * deltaTime;
            RPY += new Vector3( dphi, dteta, dpsi ) *deltaTime;
        }

        public Matrix4x4 GenerateRPYToWorldMatrix()
        {
            return Matrix4x4.Rotate(Quaternion.Euler(0, 0, Rad2Deg(RPY.z)))
                * Matrix4x4.Rotate(Quaternion.Euler(0, Rad2Deg(RPY.y), 0))
                * Matrix4x4.Rotate(Quaternion.Euler(Rad2Deg(RPY.x), 0, 0));
        }

        public Matrix4x4 GenerateWorldToRPYMatrix()
        {
            return Matrix4x4.Rotate(Quaternion.Euler(Rad2Deg(- RPY.x), 0, 0))
                * Matrix4x4.Rotate(Quaternion.Euler(0, Rad2Deg(-RPY.y), 0))
                * Matrix4x4.Rotate(Quaternion.Euler(0, 0, Rad2Deg(-RPY.z)));
        }

        public static Vector3 ToUnity(Vector3 v)
        {
            return new Vector3(v.y, -v.z, v.x);
        }
        public static Vector3 FromUnity(Vector3 v)
        {
            return new Vector3(v.z, v.x, -v.y);
        }

        public float ElevatorStabilityControl(float kt, float kv2)
        {
            float velocitySquared = Velocity.sqrMagnitude;
            return kv2 * velocitySquared + kt * Throttle;
        }

        //private void GenerateProceduralForces(ref List<Force> forces)
        //{
        //    // Example: Add lift force for a basic wing surface
        //    foreach (var surface in Surfaces)
        //    {
        //        float velocitySquared = Velocity.sqrMagnitude;
        //        Vector3 relativeVelocity = GenerateWorldToRPYMatrix().MultiplyVector(Velocity);

        //        // Calculate the angle of attack
        //        float angleOfAttack = Mathf.Atan2(
        //            Vector3.Dot(relativeVelocity, surface.Perpendicular),
        //            Vector3.Dot(relativeVelocity, surface.Parallel)
        //        );

        //        // Clamp the angle of attack to the lift limits
        //        angleOfAttack = Mathf.Clamp(angleOfAttack, MinAngleOfAttack * Mathf.Deg2Rad, MaxAngleOfAttack * Mathf.Deg2Rad);

        //        // Calculate lift force
        //        float liftForceMagnitude = 0.5f * LiftCoefficient * ClAlpha * angleOfAttack * velocitySquared * surface.SurfaceArea.x;
        //        Vector3 liftForce = liftForceMagnitude * surface.Top;

        //        forces.Add(new Force
        //        {
        //            ForceVector = liftForce,
        //            Position = surface.Center,
        //            Type = ForceType.Lift
        //        });

        //        // Example: Add drag force
        //        float dragForceMagnitude = 0.5f * LiftCoefficient * velocitySquared * surface.SurfaceArea.z;
        //        Vector3 dragForce = -dragForceMagnitude * surface.Parallel;

        //        forces.Add(new Force
        //        {
        //            ForceVector = dragForce,
        //            Position = surface.Center,
        //            Type = ForceType.Drag
        //        });
        //    }
        //}

        public void initAviaB534()
        {
            Mass = 1900;
            MomentOfInertia = new Vector3(
                0.5f * Mass * 1.4f * 1.4f,                           // Ix
                0.25f * Mass * 1.2f * 1.2f + 1.0f / 12.0f * Mass * 7.0f * 7.0f, // Iy
                0.25f * Mass * 1.2f * 1.2f + 1.0f / 12.0f * Mass * 7.0f * 7.0f  // Iz
            );

            CenterOfMass = new Vector3(1.8f, 0.0f, -2.0f);

            PropellerMaxThrust = 7000.0f;
            PropellerThrustPosition = CenterOfMass + new Vector3(3.0f, 0.0f, -0.10f );

            ClAlpha = 6; //wing properties
            ClZero = 0.0f;
            MinAngleOfAttack = -20.0f;
            MaxAngleOfAttack = 20.0f;
            LiftCoefficient = 0.6f; // air density * 1/2

            // Initialize control surface limits
            MaxElevatorAngle = 10.0f;
            MaxFlapAngle = 15.0f;
            MaxVerticalTailAngle = 10.0f;

            /*       DRAG ELEMENTS       */
            {
                Surface body_drag_surface = new Surface();
                body_drag_surface.Center = CenterOfMass;
                body_drag_surface.SurfaceArea = new Vector3( 1.4f, 7.0f, 4.0f); //body cross section surface in the 3 directions

                Surface wheels_drag_surface = new Surface();
                wheels_drag_surface.Center = CenterOfMass + new Vector3(1.0f, 0.0f, 1.0f );
                wheels_drag_surface.SurfaceArea = new Vector3(0.4f, 1.1f, 0.6f );//indicative surface of the wheels and supports in all 3 directions

                DragElements.Add(body_drag_surface);
                DragElements.Add(wheels_drag_surface);
            }

            /*       AERODYNAMIC SURFACES       */
            {
                /*WINGS*/
                Vector3 wing_offset = new Vector3( 0.34f, 0.0f, -0.3f );//offset between wing aerodynamic center and center of mass
                float wing_span = 8.0f;
                Vector3 wing_surface_area = new Vector3( 0.25f, 0.13f, 22.56f);

                //RightWing
                RightWing.Center = CenterOfMass + wing_offset + new Vector3( 0.0f, wing_span / 4f, 0.0f );
                RightWing.SurfaceArea = new Vector3( wing_surface_area.x, wing_surface_area.y, wing_surface_area.z / 2.0f );
                RightWing.Perpendicular = new Vector3(1,0,0 );
                RightWing.Parallel = new Vector3(0,1,0 );
                RightWing.CalculateTop();

                //LeftWing
                LeftWing.Center = CenterOfMass + wing_offset + new Vector3(0.0f, -wing_span / 4f, 0.0f );
                LeftWing.SurfaceArea = new Vector3(wing_surface_area.x, wing_surface_area.y, wing_surface_area.z / 2.0f );
                LeftWing.Perpendicular = new Vector3(1,0,0 );
                LeftWing.Parallel = new Vector3(0,1,0 );
                LeftWing.CalculateTop();


                /*BACK HORIZONTAL STABILIZER*/
                Vector3 tail_offset = new Vector3(-3.5f, 0.0f, -0.45f );
                Vector3 back_stabiliser_surface_area = new Vector3(0.05f, 0.03f, 4.1f);
                float back_stabiliser_span = 3.3f;

                //right horizontal stabiliser + pitch control
                RightHorizontalStabilizer.Center = CenterOfMass + tail_offset + new Vector3(0.0f, back_stabiliser_span / 4f, 0.0f );
                RightHorizontalStabilizer.SurfaceArea = new Vector3(back_stabiliser_surface_area.x, back_stabiliser_surface_area.y, back_stabiliser_surface_area.z / 2.0f );
                RightHorizontalStabilizer.Perpendicular = new Vector3(1,0,0 );
                RightHorizontalStabilizer.Parallel = new Vector3(0,1,0 );
                RightHorizontalStabilizer.CalculateTop();

                //left horizontal stabiliser + pitch control
                LeftHorizontalStabilizer.Center = CenterOfMass + tail_offset + new Vector3(0.0f, -back_stabiliser_span / 4f, 0.0f );
                LeftHorizontalStabilizer.SurfaceArea = new Vector3(back_stabiliser_surface_area.x, back_stabiliser_surface_area.y, back_stabiliser_surface_area.z / 2.0f );
                LeftHorizontalStabilizer.Perpendicular = new Vector3(1,0,0 );
                LeftHorizontalStabilizer.Parallel = new Vector3(0,1,0 );
                LeftHorizontalStabilizer.CalculateTop();


                /*VERTICAL STABILIZER + yaw control*/
                VerticalStabilizer.Center = CenterOfMass + new Vector3(-4, 0, -1 );
                VerticalStabilizer.SurfaceArea = new Vector3(0.05f, 0.03f, 1.5f );
                VerticalStabilizer.Perpendicular = new Vector3(1,0,0 );
                VerticalStabilizer.Parallel = new Vector3(0, 0, 1 );
                VerticalStabilizer.CalculateTop();


                /*FLAPS = roll control*/
                RightFlap.Center = RightWing.Center + new Vector3(0, +1.0f, -0.8f );
                RightFlap.SurfaceArea = new Vector3(0.00f, 0.03f, 1.5f );
                RightFlap.Perpendicular = new Vector3(1,0,0 );
                RightFlap.Parallel = new Vector3(0, 1, 0 );
                RightFlap.CalculateTop();


                LeftFlap.Center = LeftWing.Center + new Vector3(0, -1.0f, -0.8f );
                LeftFlap.SurfaceArea = new Vector3(0.00f, 0.03f, 1.5f );
                LeftFlap.Perpendicular = new Vector3(1,0,0 );
                LeftFlap.Parallel = new Vector3( 0, 1, 0 );
                LeftFlap.CalculateTop();
            }
        }

        public void generateProceduralForces(ref List<Force> forces)
        {
            Matrix4x4 world_to_rpy = GenerateWorldToRPYMatrix();
            
            Force weight = new Force();
            weight.Type = ForceType.Weight;
            weight.Position = CenterOfMass;
            weight.ForceVector = new Vector3( 0.0f, 0.0f, Mass * 9.81f );
            weight.ForceVector = world_to_rpy * new Vector4(weight.ForceVector.x, weight.ForceVector.y, weight.ForceVector.z, 0.0f);



            Vector3 world_w_pos = Position+weight.Position;
            Matrix4x4 rpy_world = GenerateRPYToWorldMatrix();
            Vector3 world_w_f = rpy_world * (weight.ForceVector);

            //Debug.DrawLine(new Vector3(100, 100, 0), new Vector3(-100, 100, 0));

            forces.Add(weight);
            
            Force propeller_pull  = new Force();
            propeller_pull.Type = ForceType.Base;
            propeller_pull.Position = PropellerThrustPosition;
            propeller_pull.ForceVector = new Vector3(PropellerMaxThrust* Throttle, 0.0f, 0.0f );


            forces.Add(propeller_pull);



            foreach (var element in DragElements)
            { // adds all drag coming from drag elements
                Vector3 element_vel = Velocity - Vector3.Cross(element.Center - CenterOfMass, AngularVelocity);
                float v_squared = element_vel.magnitude;
                if (v_squared == 0)
                {
                    continue;
                }
                v_squared *= v_squared;
                Vector3 drag_direction = drag_direction = -element_vel.normalized;
                Force element_drag = new Force();
                element_drag.Type = ForceType.Drag;
                element_drag.Position = element.Center;
                element_drag.ForceVector = drag_direction * Vector3.Scale(drag_direction, element.SurfaceArea).magnitude * 0.5f * v_squared * 0.6f;

                forces.Add(element_drag);
            }

            //foreach (var surface in Surfaces)
            //{
            //    Force lift = new Force();
            //    Force drag = new Force();
            //    surfaceForces(surface, ref lift, ref drag, 0);

            //    forces.Add(lift);
            //    forces.Add(drag);
            //}


            Force right_wing_lift = new Force();
            Force right_wing_drag = new Force();
            if (true)
            {
                surfaceForces(RightWing, ref right_wing_lift, ref right_wing_drag, 0);
                forces.Add(right_wing_lift);
                forces.Add(right_wing_drag);
            }

            Force left_wing_lift = new Force(), left_wing_drag  = new Force();
            surfaceForces(LeftWing, ref left_wing_lift, ref left_wing_drag, 0);
            forces.Add(left_wing_lift);
            forces.Add(left_wing_drag);


            Force right_stabiliser_lift = new Force(), right_stabiliser_drag = new Force();
            surfaceForces(RightHorizontalStabilizer, ref right_stabiliser_lift, ref right_stabiliser_drag, -Deg2Rad(MaxElevatorAngle) * PitchInput);
            forces.Add(right_stabiliser_lift);
            forces.Add(right_stabiliser_drag);

            Force left_stabiliser_lift = new Force(), left_stabiliser_drag = new Force();
            surfaceForces(LeftHorizontalStabilizer, ref left_stabiliser_lift, ref left_stabiliser_drag, -Deg2Rad(MaxElevatorAngle) * PitchInput);
            forces.Add(left_stabiliser_lift);
            forces.Add(left_stabiliser_drag);

            Force vertical_stabiliser_lift = new Force(), vertical_stabiliser_drag = new Force();
            surfaceForces(VerticalStabilizer, ref vertical_stabiliser_lift, ref vertical_stabiliser_drag, -Deg2Rad(MaxVerticalTailAngle) * YawInput);
            forces.Add(vertical_stabiliser_lift);
            forces.Add(vertical_stabiliser_drag);


            Force right_flap_lift = new Force(), right_flap_drag = new Force();
            if (true)
            {
                surfaceForces(RightFlap, ref right_flap_lift, ref right_flap_drag, -Deg2Rad(MaxFlapAngle) * RollInput);
                forces.Add(right_flap_lift);
                forces.Add(right_flap_drag);
            }

            Force left_flap_lift = new Force(), left_flap_drag = new Force();
            surfaceForces(LeftFlap, ref left_flap_lift, ref left_flap_drag, Deg2Rad(MaxFlapAngle) * RollInput);
            forces.Add(left_flap_lift);
            forces.Add(left_flap_drag);

        }



        public void surfaceForces(Surface surface, ref Force lift, ref Force drag, float tilt_angle)
        {
            lift.Type = ForceType.Lift;
            lift.Position = surface.Center;
            drag.Type = ForceType.Drag;
            drag.Position = lift.Position;

            Vector3 surface_vel = Velocity-Vector3.Cross(lift.Position - CenterOfMass, AngularVelocity);
            if (surface_vel.magnitude == 0)
            {
                drag.ForceVector = new Vector3( 0,0,0 );
                lift.ForceVector =new Vector3( 0,0,0 );
                return;
            }

            Vector3 surface_drag_direction = -(surface_vel.normalized);
            Vector3 perpxtop = Vector3.Cross(surface.Perpendicular, surface.Top);
            Vector3 surface_perp_vel = surface_vel - projectVector(surface_vel, perpxtop);
            float v_squared = surface_perp_vel.magnitude; v_squared *= v_squared;
            if (v_squared == 0)
            {
                lift.ForceVector = new Vector3( 0,0,0 );
            }
            else
            {
                //double CL = getCL(alpha, cl_alpha, cl_zero, min_angle_of_attack, max_angle_of_attack);
                float surface_alpha = angleBetweenVectors(surface.Perpendicular, surface_perp_vel);
                if (Vector3.Dot(surface_perp_vel, surface.Top) < 0)
                {
                    surface_alpha *= -1.0f;
                }
                //double surface_alpha = atan2(surface_vel.z, surface_vel.x);
                float surface_CL = getCL(surface_alpha + tilt_angle, ClAlpha, ClZero, MinAngleOfAttack, MaxAngleOfAttack);
                lift.ForceVector = (Vector3.Cross(surface.Parallel, surface_perp_vel)).normalized * surface_CL * v_squared * LiftCoefficient * surface.SurfaceArea.z;
                //Debug.Log($"CL: {surface_CL}  lc:  {LiftCoefficient}     saz: {surface.SurfaceArea.z}");

            }



            v_squared = (surface_vel).magnitude; v_squared *= v_squared;
            drag.ForceVector = surface_drag_direction * Vector3.Scale(surface_drag_direction, surface.SurfaceArea).magnitude * 0.5f * v_squared * 0.6f; //0.6 is 1/2 * air density, 0.5 is ~ the drag coefficient for a sphere. in general take drag coefficient into account when writing surface sizes

            //Debug.Log($"lift: {lift.ForceVector}  vsquare:  {v_squared}");
        }

        Vector3 projectVector(Vector3 first, Vector3 second)
        {
            float second_l = second.magnitude;

            return Vector3.Dot(second, first) / (second_l * second_l) * second;
        }

        float angleBetweenVectors(Vector3 vec1, Vector3 vec2) 
        {

	        float dotProduct = Vector3.Dot(vec1, vec2);

            float cosTheta = dotProduct / (vec1.magnitude * vec2.magnitude);

            float angleRad = Mathf.Acos(cosTheta);
            //Debug.Log($"v1: {vec1},  v2:  {vec2}, cos:  {cosTheta},  angleRad:  {angleRad}");
            return angleRad; // radians
        }

        float getCL(float alpha, float cl_alpha, float cl_zero, float min_angle_of_attack, float max_angle_of_attack)
        {
            //Debug.Log($"alpha: {alpha}, cl_alpha: {cl_alpha}, cl_zero: {cl_zero}, min_angle: {min_angle_of_attack}, max_angle: {max_angle_of_attack}");

            float CL;
            float k2 = (2 * cl_alpha * Deg2Rad(min_angle_of_attack) + cl_zero);
            float k3 = (2 * cl_alpha * Deg2Rad(max_angle_of_attack) + cl_zero);
            if (alpha < k2 / cl_alpha)
            {
                CL = 0;
            }
            else if (alpha < Deg2Rad(min_angle_of_attack))
            {
                CL = -cl_alpha * alpha + k2;
            }
            else if (alpha < Deg2Rad(max_angle_of_attack))
            {
                CL = cl_alpha * alpha + cl_zero;
            }
            else if (alpha < k3 / cl_alpha)
            {
                CL = -cl_alpha * alpha + k3;
            }
            else
            {
                CL = 0;
            }
            return CL;
        }

        public void control(float pitch_in, float roll_in, float yaw_in, float throttle_in, float delta_t)
        {
            if (pitch_in > 1) pitch_in = 1;
            else if (pitch_in < -1) pitch_in = -1;

            if (roll_in > 1) roll_in = 1;
            else if (roll_in < -1) roll_in = -1;

            if (yaw_in > 1) yaw_in = 1;
            else if (yaw_in < -1) yaw_in = -1;

            if (throttle_in > 1) throttle_in = 1;
            else if (throttle_in < 0) throttle_in = 0;

            if (PitchInput < pitch_in)
            {
                PitchInput += PitchControlSpeed * delta_t;
                if (PitchInput > pitch_in) PitchInput = pitch_in;
            }
            if (PitchInput > pitch_in)
            {
                PitchInput -= PitchControlSpeed * delta_t;
                if (PitchInput < pitch_in) PitchInput = pitch_in;
            }

            if (RollInput < roll_in)
            {
                RollInput += RollControlSpeed * delta_t;
                if (RollInput > roll_in) RollInput = roll_in;
            }
            if (RollInput > roll_in)
            {
                RollInput -= RollControlSpeed * delta_t;
                if (RollInput < roll_in) RollInput = roll_in;
            }

            if (YawInput < yaw_in)
            {
                YawInput += YawControlSpeed * delta_t;
                if (YawInput > yaw_in) YawInput = yaw_in;
            }
            if (YawInput > yaw_in)
            {
                YawInput -= YawControlSpeed * delta_t;
                if (YawInput < yaw_in) YawInput = yaw_in;
            }

            if (Throttle < throttle_in)
            {
                Throttle += ThrottleControlSpeed * delta_t;
                if (Throttle > throttle_in) Throttle = throttle_in;
            }
            if (Throttle > throttle_in)
            {
                Throttle -= ThrottleControlSpeed * delta_t;
                if (Throttle < throttle_in) Throttle = throttle_in;
            }
        }
    }


}
