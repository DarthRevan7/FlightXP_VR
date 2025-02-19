using UnityEngine;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine.SceneManagement;

public class DraggingManager : MonoBehaviour
{

    public static DraggingManager instance;

    
    [SerializeField] private MeshRenderer[] materialParts;
    [SerializeField] private Material highlightMaterial;
    [SerializeField] public bool experienceFinished = false;
    [SerializeField] private TMP_Text statoEsperienzaText, childCountText;

    [SerializeField] private GameObject aereoInPezzi, aereoInPezziPrefab;
    [SerializeField] private GameObject[] lastObjects;
    [SerializeField] private Vector3 aereoInPezziPosition;
    [SerializeField] private Quaternion aereoInPezziRotation;


    [SerializeField] private GameObject startFlight_UI;
    // [SerializeField] private GameObject colorManager;
    [SerializeField] private GameObject tutorialUI;
    private FadeEffect2 fadeEffect;
    private bool waitingFade = false;

    [SerializeField] private AudioSource audioSource;


    [SerializeField] public bool terminateXP;




    //Audio source spiegazione delle parti dell'aereo
    public AudioSource audioSpiegazione;
    //Serve per capire quali audio sono già riprodotti
    public int[] audioSpiegati;
    //Contiene gli scriptable object con gli audio dentro ed un numero che indica la uicard
    public ExplanationAudio[] explanationAudio;

    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;

        materialParts = GetComponentsInChildren<MeshRenderer>();
        highlightMaterial = materialParts[0].material;


        
        experienceFinished = ExperienceEnded();

        fadeEffect = Camera.main.gameObject.GetComponent<FadeEffect2>();


        //Modificare il nome del container delle mesh dell'aereo, nel caso in cui sia diverso
        //da questo.
        aereoInPezzi = GameObject.Find("Aereo_InPezzi");
        aereoInPezziPosition = aereoInPezzi.transform.position;
        aereoInPezziRotation = aereoInPezzi.transform.rotation;

        //Questo array servirà a contenere i riferimenti agli ultimi due gameobject rimasti da
        //sistemare, in modo da fornire un feedback corretto riguardo alla fine dell'esperienza
        //di dragging.
        lastObjects = new GameObject[2];

        audioSource = GameObject.Find("CompletamentoAereo").GetComponent<AudioSource>();   

        //Ricava le audioSources sistemate negli scriptableobjects
        explanationAudio = Resources.LoadAll<ExplanationAudio>("Audio_Spiegazione");
        //Ricava l'audio source x la spiegazione
        audioSpiegazione = GetComponent<AudioSource>();
        //Istanzia gli interi per il check della riproduzione
        audioSpiegati = new int[explanationAudio.Length];
        for(int i = 0; i < audioSpiegati.Length; i++)
        {
            audioSpiegati[i] = 0;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        //DECOMMENTARE!!!
        if(!experienceFinished && ExperienceEnded())
        {
            experienceFinished = true;
            //audioSource.Play();
            ColorManager.colorManager.experienceDone = true;
        }

        if(terminateXP)
        {
            TerminateXP();
            terminateXP = false;
        }

        //Text field di debug.
        //statoEsperienzaText.text = "Esperienza finita: " + experienceFinished.ToString();
        //Debug.Log("Child Count: " + aereoInPezzi.transform.childCount.ToString());
        //childCountText.text = "ChildCount: " + aereoInPezzi.transform.childCount.ToString();



        startFlight_UI.SetActive(experienceFinished);

        if(waitingFade && !fadeEffect.isFading)
        {
            tutorialUI.SetActive(true);
        }

    }

    public void PlayExplanationAudio(int index)
    {
        //Check index
        if(index < audioSpiegati.Length)
        {
            //Se c'è un audio in riproduzione
            if(audioSpiegazione.isPlaying)
            {
                //Ferma l'audio in riproduzione
                audioSpiegazione.Stop();
            }
            //Se l'audio non è già stato riprodotto
            if(audioSpiegati[index] == 0)
            {
                
                //Aggiorna la clip audio dell'audiosource
                audioSpiegazione.clip = explanationAudio[index].audioClip;
                //Mette un play delayed
                audioSpiegazione.PlayDelayed(0.5f);
                //Segna che l'audio è stato già riprodotto
                audioSpiegati[index] = 1;
            }
        }
    }

    

    //L'esperienza viene fatta ripartire ripristinando il materiale di highlight per tutte le parti
    //del velivolo e re-instanziando il modellino prefab delle parti di aereo da sistemare.
    public void RestartExperience()
    {
        for(int i = 0; i < materialParts.Length; i++)
        {
            materialParts[i].material = highlightMaterial;
        }
        Destroy(aereoInPezzi);
        aereoInPezzi = GameObject.Instantiate(aereoInPezziPrefab, aereoInPezziPosition, aereoInPezziRotation);
    }

    //L'esperienza è finita quando i children del transform che contiene i pezzetti di aereo da 
    //sistemare sono zero e gli ultimi due gameobject sono stati distrutti.
    //La parte degli ultimi 2 gameobjects è necessaria, perché un gameobject "grabbato" è 
    //considerato "child" del controller, pertanto viene "sottratto" ai gameobjects children del
    //gameobject container.
    private bool ExperienceEnded()
    {
        if(aereoInPezzi==null) return true;
        if(aereoInPezzi.transform.childCount == 2)
        {
            lastObjects[0] = aereoInPezzi.transform.GetChild(aereoInPezzi.transform.childCount -1).gameObject;
            lastObjects[1] = aereoInPezzi.transform.GetChild(aereoInPezzi.transform.childCount -2).gameObject;
        }
        if(aereoInPezzi.transform.childCount == 0 && lastObjects[0] == null && lastObjects[1] == null)
            return true;
        return false;
        
    }

    public void TerminateXP()
    {
        Debug.Log("term1");
        if(aereoInPezzi != null)
        {
                    Debug.Log("term2");

            SetPlaneMaterial(aereoInPezzi.transform.GetChild(0).GetComponent<MeshRenderer>().material);
            Destroy(aereoInPezzi);
        }
    }

    public bool GetExperienceFinished()
    {
        return experienceFinished;
    }

    public void SetPlaneMaterial(Material material)
    {
        for(int i = 0; i < materialParts.Length; i++)
        {
            materialParts[i].material = material;
        }
        ColorManager.colorManager.planeMaterial = material;
    }



    


}
