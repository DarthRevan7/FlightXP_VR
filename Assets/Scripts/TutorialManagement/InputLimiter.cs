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
    public float autopilot_delay = 1.0f;
    private float last_autopilot_change_time;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioManager = GameObject.FindAnyObjectByType<AudioManager>();
        planePhy = GetComponent<PlanePhyRB>();
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

        if(autopilot.action.IsPressed())
        {
            ToggleAutopilot();
        }

        if(autopilotEngaged)
        {
            float target_roll = 0;

            float target_speed = 50;

            //float target_vspeed = 0;
            float target_alt = 500;

            roll_in = /*planePhy.ang_vel.z*/ + (planePhy.rot.z*Mathf.Deg2Rad - target_roll);

            // throttle_in = planePhy.throttle_control;

            // throttle_in += 0.001f * (target_speed - planePhy.vel.magnitude);

            //pitch_in = -(target_vspeed - getVerticalSpeed()) - model.AngularVelocity.y;

            

            //pitch_in += -model.AngularAcceleration.y - model.AngularVelocity.y - (model.RPY.y - pitching_control_target);

            // pitch_in += -model.AngularAcceleration.y - model.AngularVelocity.y - (model.RPY.y - 0.1f);

            //Debug.Log($"Speed: {getSpeed()}");

            pitch_in = planePhy.pitch_control;
            pitch_in += (planePhy.ang_vel.x + (planePhy.rot.x*Mathf.Deg2Rad - 0.1f));
        }

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
