using UnityEngine;
using UnityEngine.InputSystem;

public class InputLimiter : MonoBehaviour
{

    [SerializeField]
    private InputActionReference roll, pitch, yaw, throttle, autopilot, brake;
    

    private AudioManager audioManager;
    public float throttle_dead_zone = 0.1f;

    PlanePhyRB planePhy;

    public bool autopilotEngaged = true;

    public bool tutorialActive = false;
    public bool tutorialAllowRoll = false;
    public bool tutorialAllowPitch = false;
    public bool tutorialAllowThrottle = false;
    public bool tutorialAllowAutopilot = false;

    public bool tutorialAllowYaw = false;

    public float autopilot_delay = 1.0f;
    private float last_autopilot_change_time;

    private Vector3 last_vel;
    private Vector3 last_ang_vel;

    public float trim = -0.1f;
    
    public float target_roll = 0;
    public float target_speed = 70;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioManager = GameObject.FindAnyObjectByType<AudioManager>();
        planePhy = GetComponent<PlanePhyRB>();
        last_vel = planePhy.gameObject.GetComponent<Rigidbody>().linearVelocity;
        last_ang_vel = planePhy.gameObject.GetComponent<Rigidbody>().angularVelocity;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float roll_in = roll.action.ReadValue<float>();
        float pitch_in = pitch.action.ReadValue<float>();
        float yaw_in = yaw.action.ReadValue<float>();
        float throttle_in = planePhy.throttle_control;

        if (Mathf.Abs(throttle.action.ReadValue<float>()) > throttle_dead_zone)
        {
            throttle_in = throttle.action.ReadValue<float>();
            if (throttle_in > 0.0f) throttle_in = 1.0f;
            else throttle_in = 0.0f;
        }

        if(autopilot.action.IsPressed() && (!tutorialActive || tutorialAllowAutopilot))
        {
            ToggleAutopilot();
        }

        if(autopilotEngaged||tutorialActive)
        {

            //float target_vspeed = 0;
            // float target_alt = 500;

            roll_in = /*planePhy.ang_vel.z*/ + (planePhy.rot.z*Mathf.Deg2Rad - target_roll);

            throttle_in = planePhy.throttle_control;

            throttle_in += 0.001f * (target_speed - planePhy.vel.magnitude);

            //pitch_in = -(target_vspeed - getVerticalSpeed()) - model.AngularVelocity.y;

            

            //pitch_in += -model.AngularAcceleration.y - model.AngularVelocity.y - (model.RPY.y - pitching_control_target);

            // pitch_in += -model.AngularAcceleration.y - model.AngularVelocity.y - (model.RPY.y - 0.1f);

            //Debug.Log($"Speed: {getSpeed()}");
            float pitch_acc = (last_ang_vel.x - planePhy.ang_vel.x)/Time.fixedDeltaTime;
            pitch_in = planePhy.pitch_control;
            // pitch_in += (pitch_acc + planePhy.ang_vel.x + (-planePhy.rot.x*Mathf.Deg2Rad - 0.1f))*Time.fixedDeltaTime;
            pitch_in  = planePhy.rot.x*Mathf.Deg2Rad - trim;

            // Debug.Log($"pitch_acc: {pitch_acc},  planePhy.ang_vel.x: {planePhy.ang_vel.x}, planePhy.rot.x {planePhy.rot.x*Mathf.Deg2Rad}, pitch_in: {pitch_in}");
        }

        if(tutorialActive)
        {
            if(tutorialAllowPitch) pitch_in = pitch.action.ReadValue<float>();

            if(tutorialAllowRoll) roll_in = roll.action.ReadValue<float>();

            if(tutorialAllowThrottle)
            {
                throttle_in = planePhy.throttle_control;

                if (Mathf.Abs(throttle.action.ReadValue<float>()) > throttle_dead_zone)
                {
                    throttle_in = throttle.action.ReadValue<float>();
                    if (throttle_in > 0.0f) throttle_in = 1.0f;
                    else throttle_in = 0.0f;
                }
            }

            if(tutorialAllowYaw) yaw_in = yaw.action.ReadValue<float>();
        }



        last_vel = planePhy.vel;
        last_ang_vel = planePhy.ang_vel;
        planePhy.applyControls(throttle_in, roll_in, pitch_in, yaw_in, Time.fixedDeltaTime);
    }

    public void ToggleAutopilot()
    {
        if(Time.time - last_autopilot_change_time > 1.0f)
        {
            last_autopilot_change_time = Time.time;
            autopilotEngaged = !autopilotEngaged;

            audioManager.PlayAutopilot(autopilotEngaged);
        }
    }
}
