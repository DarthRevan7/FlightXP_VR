using NUnit.Framework;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
using Cinemachine.Utility;
using static PlanePhyRB;
using System;
using static PlanePhy2;



#if UNITY_EDITOR
using UnityEditor;
#endif

public class PlanePhyRB : MonoBehaviour
{
    Rigidbody rb;

    public float timeDilation = 1.0f;

    [SerializeField]
    private InputActionReference roll, pitch, yaw, throttle, autopilot;

    //private List<LiftSurface> aerodinamic_surfaces;
    LiftSurface right_wing = new LiftSurface();
    LiftSurface left_wing = new LiftSurface();
    LiftSurface right_elevator = new LiftSurface();
    LiftSurface left_elevator = new LiftSurface();
    LiftSurface rudder = new LiftSurface();
    LiftSurface right_flap = new LiftSurface();
    LiftSurface left_flap = new LiftSurface();
    LiftSurface horizontal_stabilizer = new LiftSurface();
    //LiftSurface left_flap = new LiftSurface();
    private List<Surface> drag_elements = new List<Surface>();

    public float propeller_max_thrust = 7000.0f;
    public float throttle_dead_zone = 0.1f;
    public Vector3 propeller_thrust_position = new Vector3(0.0f, 0.10f, 3.0f);

    public float LiftCoefficient = 0.6f; // air density * 1/2

    // Initialize control surface limits
    public float MaxElevatorAngle = 10.0f;
    public float MaxFlapAngle = 15.0f;
    public float MaxVerticalTailAngle = 10.0f;

    public float PitchControlSpeed = 2.0f;
    public float RollControlSpeed = 2.0f;
    public float YawControlSpeed = 2.0f;
    public float ThrottleControlSpeed = 0.5f;

    float throttle_control = 1.0f;
    float pitch_control = 0.0f;
    float roll_control = 0.0f;
    float yaw_control = 0.0f;


    public Vector3 vel;
    public Vector3 ang_vel;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.mass = 1900;
        rb.inertiaTensor = new Vector3(
                0.25f * rb.mass * 1.2f * 1.2f + 1.0f / 12.0f * rb.mass * 7.0f * 7.0f,
                0.25f * rb.mass * 1.2f * 1.2f + 1.0f / 12.0f * rb.mass * 7.0f * 7.0f,
                0.5f * rb.mass * 1.4f * 1.4f
            );
        rb.linearVelocity = new Vector3(0,0,50);

        initializePlane();
    }

    void initializePlane()
    {
        /*DRAG ELEMENTS*/
        Surface body_drag = new Surface();
        body_drag.Center = Vector3.zero;//centered in center of mass
        body_drag.SurfaceArea = new Vector3(7.0f, 4.0f, 1.4f);

        Surface landing_gear_drag = new Surface();
        landing_gear_drag.Center = new Vector3(0.0f, -1.0f, 1.0f);
        landing_gear_drag.SurfaceArea = new Vector3(1.1f, 0.6f, 0.4f);

        drag_elements.Add( body_drag );
        drag_elements.Add(landing_gear_drag );


        /*LIFT SURFACES*/
        //WINGS
        Vector3 wing_offset = new Vector3( 0.0f, 0.3f, 0.34f );//offset between wing aerodynamic center and center of mass
        float wing_span = 8.0f;
        Vector3 wing_surface_area = new Vector3(0.13f, 22.56f, 0.25f);

        right_wing.surface.Center = wing_offset + new Vector3(wing_span/4.0f, 0.0f, 0.0f);
        right_wing.surface.SurfaceArea = new Vector3(wing_surface_area.x, wing_surface_area.y/2.0f, wing_surface_area.z);
        right_wing.Perpendicular = Vector3.forward;
        right_wing.Parallel = Vector3.right;
        right_wing.Top = Vector3.up;

        left_wing.surface.Center = wing_offset - new Vector3(wing_span / 4.0f, 0.0f, 0.0f);
        left_wing.surface.SurfaceArea = new Vector3(wing_surface_area.x, wing_surface_area.y / 2.0f, wing_surface_area.z);
        left_wing.Perpendicular = Vector3.forward;
        left_wing.Parallel = Vector3.right;
        left_wing.Top = Vector3.up;


        //ELEVATORS
        Vector3 tail_offset = new Vector3(0.0f, 0.45f, -3.5f);
        Vector3 elevator_surface_area = new Vector3(0.03f, 4.1f, 0.05f);
        float elevator_span = 3.3f;

        right_elevator.surface.Center = tail_offset + new Vector3(elevator_span / 4.0f, 0.0f, 0.0f);
        right_elevator.surface.SurfaceArea = new Vector3(elevator_surface_area.x, elevator_surface_area.y / 2.0f, elevator_surface_area.z);
        right_elevator.Perpendicular = Vector3.forward;
        right_elevator.Parallel = Vector3.right;
        right_elevator.Top = Vector3.up;

        left_elevator.surface.Center = tail_offset - new Vector3(elevator_span / 4.0f, 0.0f, 0.0f);
        left_elevator.surface.SurfaceArea = new Vector3(elevator_surface_area.x, elevator_surface_area.y / 2.0f, elevator_surface_area.z);
        left_elevator.Perpendicular = Vector3.forward;
        left_elevator.Parallel = Vector3.right;
        left_elevator.Top = Vector3.up;


        //RUDDER
        rudder.surface.Center = new Vector3(0, 1, -4);
        rudder.surface.SurfaceArea = new Vector3(1.5f, 0.03f, 0.05f);
        rudder.Perpendicular = Vector3.forward;
        rudder.Parallel = Vector3.up;
        rudder.Top = Vector3.left;


        //FLAPS
        right_flap.surface.Center = new Vector3(2.5f, 0.8f, 0.0f);
        right_flap.surface.SurfaceArea = new Vector3( 0.03f, 0.9f, 0.00f);
        right_flap.Perpendicular = Vector3.forward;
        right_flap.Parallel = Vector3.right;
        right_flap.Top = Vector3.up;

        left_flap.surface.Center = new Vector3(-2.5f, 0.8f, 0.0f);
        left_flap.surface.SurfaceArea = new Vector3(0.03f, 0.9f, 0.00f);
        left_flap.Perpendicular = Vector3.forward;
        left_flap.Parallel = Vector3.right;
        left_flap.Top = Vector3.up;


        horizontal_stabilizer.surface.Center = new Vector3(0.0f, 0.45f, -3.2f);
        horizontal_stabilizer.surface.SurfaceArea = new Vector3(0.03f, 2.0f, 0.03f);
        horizontal_stabilizer.Perpendicular = Vector3.forward;
        horizontal_stabilizer.Parallel = Vector3.right;
        horizontal_stabilizer.Top = Vector3.up;
    }

    private void FixedUpdate()
    {
        float roll_in = roll.action.ReadValue<float>();
        float pitch_in = pitch.action.ReadValue<float>();
        float yaw_in = yaw.action.ReadValue<float>();
        float throttle_in = throttle_control;

        if (Mathf.Abs(throttle.action.ReadValue<float>()) > throttle_dead_zone)
        {
            throttle_in = throttle.action.ReadValue<float>();
            if (throttle_in > 0.0f) throttle_in = 1.0f;
            else throttle_in = 0.0f;
        }

        if(autopilot.action.IsPressed())
        {
#if UNITY_EDITOR
            EditorApplication.isPaused = true;
#endif
        }

        applyControls(throttle_in, roll_in, pitch_in, yaw_in, Time.fixedDeltaTime);

        foreach(Surface drag_element in drag_elements)
        {
            applyDrag(drag_element);
        }

        //Debug.Log($"Input:   roll: {roll_in}    pitch: {pitch_in}   yaw: {yaw_in}   throttle: {throttle_in}");
        //Debug.Log($"Control:   roll: {roll_control}    pitch: {pitch_control}   yaw: {yaw_control}   throttle: {throttle_control}");

        applySurfaceForces2(right_wing, 0);
        applySurfaceForces2(left_wing, 0);
        applySurfaceForces2(right_elevator, -Mathf.Deg2Rad * MaxElevatorAngle * pitch_control);
        applySurfaceForces2(left_elevator, -Mathf.Deg2Rad * MaxElevatorAngle * pitch_control);
        applySurfaceForces2(rudder, Mathf.Deg2Rad * MaxVerticalTailAngle * yaw_control);
        applySurfaceForces2(right_flap, -Mathf.Deg2Rad * MaxFlapAngle * roll_control);
        applySurfaceForces2(left_flap, Mathf.Deg2Rad * MaxFlapAngle * roll_control);
        applySurfaceForces2(horizontal_stabilizer, 0);

        applyForce(rb.transform.TransformDirection(new Vector3(0.0f, 0.0f, throttle_control * propeller_max_thrust)), rb.transform.TransformPoint(propeller_thrust_position), Color.white);

        //Debug.Log($"FRAME -- pos: {rb.position}   vel; {rb.linearVelocity}   rotvel: {rb.angularVelocity}");

//#if UNITY_EDITOR
//        EditorApplication.isPaused = true;
//#endif
        vel = rb.linearVelocity;
        ang_vel = rb.angularVelocity;
    }

    void applyControls(float throttle_input, float roll_input, float pitch_input, float yaw_input, float deltaT)
    {
        if (pitch_input > 1) pitch_input = 1;
        else if(pitch_input < -1) pitch_input = -1;

        if (roll_input > 1) roll_input = 1;
        else if(roll_input < -1) roll_input = -1;

        if (yaw_input > 1) yaw_input = 1;
        else if (yaw_input < -1) yaw_input = -1;

        if (throttle_input > 1) throttle_input = 1;
        else if (throttle_input < -1) throttle_input = -1;


        if(pitch_control < pitch_input)
        {
            pitch_control += PitchControlSpeed * deltaT;
            if (pitch_control > pitch_input) pitch_input = pitch_control;
        }
        if (pitch_control > pitch_input)
        {
            pitch_control -= PitchControlSpeed * deltaT;
            if (pitch_control < pitch_input) pitch_input = pitch_control;
        }

        if (roll_control < roll_input)
        {
            roll_control += RollControlSpeed * deltaT;
            if (roll_control > roll_input) roll_input = roll_control;
        }
        if (roll_control > roll_input)
        {
            roll_control -= RollControlSpeed * deltaT;
            if (roll_control < roll_input) roll_input = roll_control;
        }

        if (yaw_control < yaw_input)
        {
            yaw_control += YawControlSpeed * deltaT;
            if (yaw_control > yaw_input) yaw_input = yaw_control;
        }
        if (yaw_control > yaw_input)
        {
            yaw_control -= YawControlSpeed * deltaT;
            if (yaw_control < yaw_input) yaw_input = yaw_control;
        }

        if (throttle_control < throttle_input)
        {
            throttle_control += ThrottleControlSpeed * deltaT;
            if (throttle_control > throttle_input) throttle_control = throttle_input;
        }
        if (throttle_control > throttle_input)
        {
            throttle_control -= ThrottleControlSpeed * deltaT;
            if (throttle_control < throttle_input) throttle_control = throttle_input;
        }
    }

    void applyForce(Vector3 force, Vector3 position, Color color)
    {
        rb.AddForceAtPosition(force, position);

        Debug.DrawLine(position, position + force / 1000.0f, color);
    }


    void applyDrag(Surface drag_element)
    {
        Vector3 force_position = rb.transform.TransformPoint(drag_element.Center);

        Vector3 surface_vel = rb.transform.InverseTransformDirection(rb.linearVelocity) - Vector3.Cross(drag_element.Center, rb.transform.InverseTransformDirection(rb.angularVelocity));

        Vector3 drag_direction = (-surface_vel).normalized;

        float v_drag_square = Vector3.Scale(-surface_vel, drag_element.SurfaceArea).magnitude; v_drag_square *= v_drag_square;
        Vector3 drag_force = rb.transform.TransformDirection(drag_direction * v_drag_square * 0.5f * LiftCoefficient);

        applyForce(drag_force, force_position, Color.red);
        Debug.DrawLine(force_position, force_position + drag_force / 1000.0f, Color.red);
    }

    void applySurfaceForces2(LiftSurface surface, float tilt_angle)
    {
        Vector3 force_position = rb.transform.TransformPoint(surface.surface.Center);

        Vector3 surface_vel = rb.transform.InverseTransformDirection(rb.linearVelocity) - Vector3.Cross(surface.surface.Center, rb.transform.InverseTransformDirection(rb.angularVelocity));

        Vector3 drag_direction = (-surface_vel).normalized;

        Vector3 perpxtop = Vector3.Cross(surface.Perpendicular, surface.Top);
        Vector3 surface_perp_vel = surface_vel - projectVector(surface_vel, perpxtop);
        float v_squared = surface_perp_vel.magnitude; v_squared *= v_squared;

        float v_drag_square = Vector3.Scale(-surface_vel, surface.surface.SurfaceArea).magnitude; v_drag_square *= v_drag_square;
        Vector3 drag_force = rb.transform.TransformDirection(drag_direction * v_drag_square * 0.5f * LiftCoefficient);

        applyForce(drag_force, force_position, Color.red);
        Debug.DrawLine(force_position, force_position + drag_force / 1000.0f, Color.red);

        float surface_alpha = angleBetweenVectors(surface.Perpendicular, surface_perp_vel);
        if (Vector3.Dot(surface_perp_vel, surface.Top) > 0) surface_alpha *= -1.0f;

        //Debug.Log($"surface alpha: {surface_alpha}  worldperp: {world_surface.Perpendicular}, surf_perp_vel: {surface_perp_vel}");


        float surface_CL = getCL(surface_alpha + tilt_angle, surface.cl_alpha, surface.cl_zero, surface.min_angle_of_attack, surface.max_angle_of_attack);
        Vector3 lift_dir = surface.Top;
        Vector3 lift_force = rb.transform.TransformDirection(lift_dir * surface_CL * v_squared * LiftCoefficient * Vector3.Scale(surface.Top, surface.surface.SurfaceArea).magnitude);

        applyForce(lift_force, force_position, Color.blue);
        Debug.DrawLine(force_position, force_position + lift_force / 1000.0f, Color.blue);
    }

    void applySurfaceForces(LiftSurface surface, float tilt_angle)
    {
        //Debug.Log($"applySurfaceForces begin.   surface.cent: {surface.surface.Center}");
        

        Vector3 force_position = rb.transform.TransformPoint(surface.surface.Center);



        Vector3 surface_vel = rb.linearVelocity - Vector3.Cross(rb.transform.TransformDirection(surface.surface.Center), rb.angularVelocity);

        if (surface_vel.magnitude == 0) return;
        //Debug.Log($"after 0 transform begin.   surface.cent: {surface.surface.Center}");
        LiftSurface world_surface = surface.transform(rb.transform);
        //Debug.Log($"after 1 transform begin.   surface.cent: {surface.surface.Center}");

        Vector3 drag_direction = (-surface_vel).normalized;
        Vector3 perpxtop = Vector3.Cross(world_surface.Perpendicular, world_surface.Top);
        Vector3 surface_perp_vel = surface_vel - projectVector(surface_vel, perpxtop);
        float v_squared = surface_perp_vel.magnitude; v_squared *= v_squared;

        //float v_complete_squared = surface_vel.magnitude; v_complete_squared *= v_complete_squared;
        float v_drag_square = Vector3.Scale(-surface_vel, world_surface.surface.SurfaceArea).magnitude; v_drag_square*=v_drag_square;
        Vector3 drag_force = drag_direction * v_drag_square * 0.5f * LiftCoefficient;

        rb.AddForceAtPosition(drag_force, force_position);
        Debug.Log($"world_surf: area: {world_surface.surface.SurfaceArea}   surface_vel: {surface_vel}");
        //Debug.Log($"surface drag: {drag_force} at {force_position}.  v_complete_squared = {v_complete_squared}    drag_direction = {drag_direction}    drag_magnitude_coefficient = {Vector3.Scale(drag_direction, world_surface.surface.SurfaceArea).magnitude}    surface_vel = {surface_vel}     rblinvel = {rb.linearVelocity}    surf_rotvel = {Vector3.Cross(rb.transform.TransformDirection(surface.surface.Center), rb.angularVelocity)}    surf cent: {surface.surface.Center}");
        Debug.DrawLine(force_position, force_position + drag_force / 1000.0f, Color.red);

        if (v_squared == 0) return;

        float surface_alpha = angleBetweenVectors(world_surface.Perpendicular, surface_perp_vel);
        if (Vector3.Dot(surface_perp_vel, world_surface.Top) > 0) surface_alpha *= -1.0f;

        //Debug.Log($"surface alpha: {surface_alpha}  worldperp: {world_surface.Perpendicular}, surf_perp_vel: {surface_perp_vel}");


        float surface_CL = getCL(surface_alpha + tilt_angle, surface.cl_alpha, surface.cl_zero, surface.min_angle_of_attack, surface.max_angle_of_attack);
        Vector3 lift_dir = world_surface.Top;
        Vector3 lift_force = lift_dir * surface_CL * v_squared * LiftCoefficient * Vector3.Scale(world_surface.Top, world_surface.surface.SurfaceArea).magnitude;

        rb.AddForceAtPosition(lift_force, force_position);
        //Debug.Log($"surface lift: {lift_force} at {force_position},   surface_CL: {surface_CL}, v_squared: {v_squared}, lift_dir: { (Vector3.Cross(world_surface.Parallel, surface_perp_vel)).normalized },  world_surface.Parallel: {world_surface.Parallel},  surface_perp_vel: {surface_perp_vel}");

        Debug.DrawLine(force_position, force_position+lift_force/1000.0f, Color.blue);
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

        float CL;
        float k2 = (2 * cl_alpha * Mathf.Deg2Rad * (min_angle_of_attack) + cl_zero);
        float k3 = (2 * cl_alpha * Mathf.Deg2Rad * (max_angle_of_attack) + cl_zero);

        //Debug.Log($"alpha: {alpha}, cl_alpha: {cl_alpha}, cl_zero: {cl_zero}, min_angle: {min_angle_of_attack}, max_angle: {max_angle_of_attack}, k2: {k2}, k3: {k3}");
        if (alpha < k2 / cl_alpha)
        {
            CL = 0;
        }
        else if (alpha < Mathf.Deg2Rad * (min_angle_of_attack))
        {
            CL = -cl_alpha * alpha + k2;
        }
        else if (alpha < Mathf.Deg2Rad * (max_angle_of_attack))
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

    public struct Surface
    {
        public Vector3 Center; // Aerodynamic center
        public Vector3 SurfaceArea; // Surface cross section area in 3 directions {perpendicular, Parallel, wide}

        public Surface transform(Transform t)
        {
            Surface res = new Surface();
            res.Center = t.TransformPoint(Center);
            res.SurfaceArea = t.TransformDirection(SurfaceArea).Abs();
            return res;
        }
    }

    public class LiftSurface
    {
        public Surface surface = new Surface();
        public Vector3 Perpendicular; // Direction of surface movement if alpha = 0
        public Vector3 Parallel; // Direction of aerodynamic centerline
        public Vector3 Top; // Up direction of profiles

        public float cl_alpha = 6; //wing properties
        public float cl_zero = 0.0f;
        public float min_angle_of_attack = -20.0f;
        public float max_angle_of_attack = 20.0f;

        public void CalculateTop()
        {
            Top = Vector3.Cross(Perpendicular, Parallel).normalized;
        }

        public LiftSurface transform(Transform t)
        {
            LiftSurface res = (LiftSurface)this.MemberwiseClone();
            res.surface = surface.transform(t);
            res.Perpendicular = t.TransformDirection(Perpendicular);
            res.Parallel = t.TransformDirection(Parallel);
            res.Top = t.TransformDirection(Top);
            return res;
        }
    }
}

