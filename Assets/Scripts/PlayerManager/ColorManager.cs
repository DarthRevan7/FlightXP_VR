using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class ColorManager : MonoBehaviour
{
    //Istanza del Singleton
    public static ColorManager colorManager;
    //Deve essere settato all'inizio col Materiale dell'aereo nella scena.
    public Material planeMaterial;
    public bool experienceDone = false;

    public Material decalMaterial;
    // public GameObject decalProjectors;

    public GameObject aereoGraphics;
    public Transform fullModel;
    

    void Awake()
    {
        // DontDestroyOnLoad(this);
        if(colorManager != null && colorManager != this) 
        {
            Destroy(gameObject);
        }
        else
        {
            colorManager = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += UpdateMaterial;
        SceneManager.sceneLoaded += ReloadHangarScene;
    }

    void UpdateMaterial(Scene scene, LoadSceneMode loadSceneMode)
    {
        //Trova i componenti dell'aereo o l'oggetto aereo ed assegnagli il materiale giusto
        // GameObject aereo = GameObject.Find("aereo");
        // GameObject fusoliera = GameObject.Find("fusoliera");

        aereoGraphics = GameObject.Find("AereoGraphics");
        if(aereoGraphics != null)
        {
            Debug.Log("Trovato aereo graphix");
            fullModel = aereoGraphics.transform.Find("FullModel");
            
            Material []materials = new Material[2];
            materials[0] = planeMaterial;
            materials[1] = fullModel.GetComponent<MeshRenderer>().materials[1];
            fullModel.GetComponent<MeshRenderer>().materials = materials;
            for(int i = 0; i < fullModel.childCount; i++)
            {

                fullModel.GetChild(i).GetComponent<MeshRenderer>().materials = materials;
            }
        }

        //Se sono nella scena terra
        if(scene.name.Equals("Scena_Terra") && experienceDone)
        {
            //Setto il materiale x l'aereo.
            GameObject.FindAnyObjectByType<DraggingManager>().SetPlaneMaterial(planeMaterial);
        }

        // ChangeDecalMaterial(decalMaterial);

        
    }

    public void ChangeDecalMaterial(Material material)
    {
        decalMaterial = material;

        if(decalMaterial != null)
        {
            if(aereoGraphics != null)
            {
                fullModel = aereoGraphics.transform.Find("FullModel");
                fullModel.GetComponent<MeshRenderer>().materials[1] = decalMaterial;
                for(int i = 0; i < fullModel.childCount; i++)
                {
                    fullModel.GetChild(i).GetComponent<MeshRenderer>().materials[1] = decalMaterial;
                }
            }
            if(SceneManager.GetActiveScene().name.Equals("Scena_Terra"))
            {
                GameObject aereoNoPhysics = GameObject.Find("Aereo_NoPhysics");

                if(aereoNoPhysics != null && experienceDone)
                {
                    
                    MeshRenderer[] renderers = aereoNoPhysics.GetComponentsInChildren<MeshRenderer>();
                    Material []materials = new Material[2];//renderers[0].materials;

                    materials[0] = planeMaterial;
                    materials[1] = decalMaterial;
                    
                    for(int i = 0; i < renderers.Length; i++)
                    {
                        renderers[i].materials = materials;
                        
                        
                    }

                    foreach(DragPoint dp in aereoNoPhysics.GetComponentsInChildren<DragPoint>())
                    {
                        dp.CoverPanelDeactivate();
                    }
                }
            }
        }
    }

    void ReloadHangarScene(Scene scene, LoadSceneMode loadSceneMode)
    {

        if(scene.name.Equals("Scena_Terra"))
        {
            
            if(experienceDone)
            {
                GameObject aereoPezzi = GameObject.Find("Aereo_InPezzi");
                DraggingManager draggingManager = GameObject.Find("Aereo_NoPhysics").GetComponent<DraggingManager>();
                draggingManager.experienceFinished = true;
                GameObject.Destroy(aereoPezzi);
                //draggingManager.SetPlaneMaterial(planeMaterial);

            }
        }


    }
}
