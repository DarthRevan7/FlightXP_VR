using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialManager : MonoBehaviour
{
    //Gestione del Tutorial
    public TutorialStep[] tutorialSteps;
    public int currentStep = 0;
    public AudioSource audioSource;

    //Sistema di input
    public InputActionReference roll, pitch, yaw, throttle, repeatStep, autopilot;
    //Indica se ha compiuto una rotazione in entrambi i versi
    public bool clockwise = false, counterClockwise = false;
    //Indica se uno step è verificato
    public bool verifiedStep = false;

    //Da rimuovere dopo aver implementato le condizioni necessarie
    public bool conditionToImplement = true;

    



    void Awake()
    {
        tutorialSteps = Resources.LoadAll<TutorialStep>("Tutorial Steps\\");
        
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentStep = 0;
        audioSource.clip = tutorialSteps[currentStep].audioClip;
        //Assicurarsi che la traccia non parta in Awake.
        // audioSource.playOnAwake = false;

        //Inserire il play qui.
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RepeatCurrentStep()
    {
        audioSource.Play();
    }

    public void NextStep()
    {
        if(currentStep < tutorialSteps.Length && verifiedStep)
        {
            currentStep++;
            audioSource.clip = tutorialSteps[currentStep].audioClip;
            audioSource.Play();
            
            //Resetta la verifica dello step
            verifiedStep = false;
            clockwise = false;
            counterClockwise = false;
        }
    }

    public void VerificaStepRotazione(InputActionReference inputAction, float threshold)
    {
        if(inputAction.action.ReadValue<float>() > threshold && !clockwise)
        {
            clockwise = true;
        }
        if(inputAction.action.ReadValue<float>() < -threshold && !counterClockwise)
        {
            counterClockwise = true;
        }

        verifiedStep = clockwise && counterClockwise;
    }

    public void VerificaStepPressione(InputActionReference inputAction)
    {
        if(!verifiedStep)
        {
            verifiedStep = inputAction.action.ReadValue<bool>();
        }
    }
    
}
