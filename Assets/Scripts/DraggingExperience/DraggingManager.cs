using UnityEngine;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine.SceneManagement;

public class DraggingManager : MonoBehaviour
{

    public static DraggingManager instance;

    
    [SerializeField] private MeshRenderer[] materialParts;
    [SerializeField] private Material highlightMaterial;
    [SerializeField] private bool experienceFinished = false;
    [SerializeField] private TMP_Text statoEsperienzaText, childCountText;

    [SerializeField] private GameObject aereoInPezzi, aereoInPezziPrefab;
    [SerializeField] private GameObject[] lastObjects;
    [SerializeField] private Vector3 aereoInPezziPosition;
    [SerializeField] private Quaternion aereoInPezziRotation;


    [SerializeField] private GameObject startFlight_UI;
    [SerializeField] private GameObject colorManager;
    [SerializeField] private GameObject tutorialUI;
    private FadeEffect fadeEffect;
    private bool waitingFade = false;



    /*
    
    Potrei fare in modo che quando riparte l'esperienza, il parent dell'aereo viene re-istanziato 
    tramite Destroy e poi un successivo Instance.
    Oltre a questo devo rimettere a posto il reference dell'oggetto re-istanziato.
    Ovviamente devo creare un pulsante x far ripartire l'esperienza.

    Ricapitolando:
    1. I pezzi appariranno e cadranno in una cesta
    2. Il giocatore compone l'aereo
    3. L'esperienza termina
    4. Il giocatore preme sul pulsante per ricominciare l'esperienza
    
    
    */


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;

        materialParts = GetComponentsInChildren<MeshRenderer>();
        highlightMaterial = materialParts[0].material;


        //DECOMMENTARE!!
        experienceFinished = ExperienceEnded();

        fadeEffect = Camera.main.gameObject.GetComponent<FadeEffect>();


        //Modificare il nome del container delle mesh dell'aereo, nel caso in cui sia diverso
        //da questo.
        aereoInPezzi = GameObject.Find("Aereo_InPezzi");
        aereoInPezziPosition = aereoInPezzi.transform.position;
        aereoInPezziRotation = aereoInPezzi.transform.rotation;

        //Questo array servirà a contenere i riferimenti agli ultimi due gameobject rimasti da
        //sistemare, in modo da fornire un feedback corretto riguardo alla fine dell'esperienza
        //di dragging.
        lastObjects = new GameObject[2];
    }

    // Update is called once per frame
    void Update()
    {
        //DECOMMENTARE!!!
        experienceFinished = ExperienceEnded();

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
        if(aereoInPezzi.transform.childCount == 2)
        {
            lastObjects[0] = aereoInPezzi.transform.GetChild(aereoInPezzi.transform.childCount -1).gameObject;
            lastObjects[1] = aereoInPezzi.transform.GetChild(aereoInPezzi.transform.childCount -2).gameObject;
        }
        if(aereoInPezzi.transform.childCount == 0 && lastObjects[0] == null && lastObjects[1] == null)
            return true;
        return false;
        
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
        colorManager.GetComponent<ColorManager>().planeMaterial = material;
    }

    


}
