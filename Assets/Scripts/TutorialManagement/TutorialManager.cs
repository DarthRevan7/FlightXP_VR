using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    //Gestione del Tutorial
    public TutorialStep[] tutorialSteps;
    public TutorialStep[] congratulazioni;
    public int congratsIndex = -1;
    public int currentStep = 0;
    public AudioSource audioSource;

    public InputLimiter inputLimiter;

    //Sistema di input
    public InputActionReference roll, pitch, yaw, throttle, repeatStep, autopilot;
    //Soglia di rotazione
    public float rotationThreshold = 0.3f;
    //Ritardo tra le registrazioni.
    public float delayTime = 2f;
    //Indica se ci si sta congratulando
    public bool congratulating = false;
    //Indica se ha compiuto una rotazione in entrambi i versi
    public bool clockwise = false, counterClockwise = false;
    //Indica se uno step è verificato
    public bool verifiedStep = false;

    //Da rimuovere dopo aver implementato le condizioni necessarie
    public bool conditionToImplement = true;
    //Canvas x immagini del tutorial
    public Image tutorialImage;

    



    void Awake()
    {
        tutorialSteps = Resources.LoadAll<TutorialStep>("TutorialSteps");
        congratulazioni = Resources.LoadAll<TutorialStep>("Congratulazioni");

        audioSource = gameObject.GetComponent<AudioSource>();
        
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentStep = 0;
        audioSource.clip = tutorialSteps[currentStep].audioClip;
        audioSource.PlayDelayed(delayTime);

        inputLimiter.tutorialActive = true;

        tutorialImage = GameObject.Find("TutorialImage").GetComponent<Image>();

        SetTutorialImage();
        //Assicurarsi che la traccia non parta in Awake.
        // audioSource.playOnAwake = false;

        //Inserire il play qui.
    }

    // Update is called once per frame
    void Update()
    {
        Tutorial();
    }

    public void Tutorial()
    {
        RepeatCurrentStep();
        if(verifiedStep)
        {
            NextStep();
        }
        if(audioSource.isPlaying) return;
        //Premi pulsante x ripetere il tutorial
        if(currentStep == 0)
        {
            VerificaStepPressione(repeatStep);
        }
        //Pitch (su e giù)
        if(currentStep == 1)
        {
            inputLimiter.tutorialAllowPitch = true;
            VerificaStepRotazione(pitch, rotationThreshold);
        }
        //Roll (Rotazione dx o sx)
        if(currentStep == 2)
        {
            inputLimiter.tutorialAllowRoll = true;
            VerificaStepRotazione(roll, rotationThreshold);
        }
        // Yaw (Rotazione rispetto all'alto dx sx)
        if(currentStep == 3)
        {
            inputLimiter.tutorialAllowYaw = true;
            VerificaStepRotazione(yaw,rotationThreshold);
        }
        //Potenza del motore
        if(currentStep == 4)
        {
            inputLimiter.tutorialAllowThrottle = true;
            VerificaStepRotazione(throttle,rotationThreshold);
        }
        //Autopilota
        if(currentStep == 5)
        {
            inputLimiter.autopilotEngaged = false;
            inputLimiter.tutorialActive = false;
            inputLimiter.tutorialAllowAutopilot = true;
            VerificaStepPressione(autopilot);
        }
    }

    public void RepeatCurrentStep()
    {
        //La sorgente audio non deve essere in riproduzione
        //Devo aver premuto il pulsante per ripetere le istruzioni
        //Non devo essere allo step 0 perché lì viene usato solo come verifica.
        //Non devo aver finito il tutorial, perché altrimenti mi ripete l'ultima congratulazione.
        if(!audioSource.isPlaying && repeatStep.action.IsPressed() && currentStep!=0 && currentStep < tutorialSteps.Length)
        {
            inputLimiter.tutorialAllowAutopilot = false;
            inputLimiter.tutorialAllowPitch = false;
            inputLimiter.tutorialAllowRoll = false;
            inputLimiter.tutorialAllowThrottle = false;
            inputLimiter.tutorialAllowYaw = false;
            inputLimiter.tutorialActive = true;
            audioSource.Play();
        }
    }

    public void NextStep()
    {
        inputLimiter.tutorialAllowAutopilot = false;
        inputLimiter.tutorialAllowPitch = false;
        inputLimiter.tutorialAllowRoll = false;
        inputLimiter.tutorialAllowThrottle = false;
        inputLimiter.tutorialAllowYaw = false;
        inputLimiter.tutorialActive = true;
        //Va avanti nel tutorial
        //Se l'audio source non sta venendo riprodotta
        //Se ci sono altri step rimasti
        //E se lo step corrente è stato verificato
        if(currentStep < tutorialSteps.Length && verifiedStep && !audioSource.isPlaying)
        {
            //Se non sta riproducendo nulla e devo congratularmi(congratsIndex == -1)
            //Non devo congratularmi alla fine del tutorial!
            if(congratsIndex == -1 && currentStep < tutorialSteps.Length-1 && currentStep < tutorialSteps.Length -2)
            {
                congratsIndex = Random.Range(0,3);
                audioSource.clip = congratulazioni[congratsIndex].audioClip;
                audioSource.Play();
            }
            //Se mi sono già congratulato oppure l'utente è alla fine del tutorial
            else if(congratsIndex != -1 || currentStep == tutorialSteps.Length -2)
            {
                //Resetta la verifica dello step 
                verifiedStep = false;
                clockwise = false;
                counterClockwise = false;

                //Avanza nel tutorial
                currentStep++;
                audioSource.clip = tutorialSteps[currentStep].audioClip;
                audioSource.PlayDelayed(delayTime);
                SetTutorialImage();

                //Resetta le congratulazioni
                congratsIndex = -1;
                
            }
        }
        
    }

    public void VerificaStepRotazione(InputActionReference inputAction, float threshold)
    {
        if(inputAction.action.ReadValue<float>() > threshold && !clockwise && !audioSource.isPlaying)
        {
            clockwise = true;
        }
        if(inputAction.action.ReadValue<float>() < -threshold && !counterClockwise && !audioSource.isPlaying)
        {
            counterClockwise = true;
        }

        verifiedStep = clockwise && counterClockwise;
    }

    public void VerificaStepPressione(InputActionReference inputAction)
    {
        if(!verifiedStep && !audioSource.isPlaying)
        {
            verifiedStep = inputAction.action.IsPressed();
        }
    }

    //Setta l'immagine del tutorial in base allo step corrente.
    public void SetTutorialImage()
    {
        //Nel caso non funzionasse, prova ad inserire una sprite placeholder
        tutorialImage.sprite = tutorialSteps[currentStep].tutorialSprite;
    }
    
}
