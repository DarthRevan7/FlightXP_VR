using UnityEngine;
using TMPro;
using Unity.XR.CoreUtils;

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

        experienceFinished = ExperienceEnded();

        aereoInPezzi = GameObject.Find("Aereo in pezzi");
        aereoInPezziPosition = aereoInPezzi.transform.position;
        aereoInPezziRotation = aereoInPezzi.transform.rotation;

        lastObjects = new GameObject[2];
    }

    // Update is called once per frame
    void Update()
    {
        experienceFinished = ExperienceEnded();
        statoEsperienzaText.text = "Esperienza finita: " + experienceFinished.ToString();
        //Debug.Log("Child Count: " + aereoInPezzi.transform.childCount.ToString());
        childCountText.text = "ChildCount: " + aereoInPezzi.transform.childCount.ToString();
    }

    //L'esperienza viene fatta ripartire ripristinando il materiale di highlight per tutte le parti
    //del velivolo
    public void RestartExperience()
    {
        for(int i = 0; i < materialParts.Length; i++)
        {
            materialParts[i].material = highlightMaterial;
        }
        Destroy(aereoInPezzi);
        aereoInPezzi = GameObject.Instantiate(aereoInPezziPrefab, aereoInPezziPosition, aereoInPezziRotation);
    }

    //L'esperienza è finita quando i materiali sono tutti diversi da quello per l'effetto highlight
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
}
