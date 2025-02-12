using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class ColorManager : MonoBehaviour
{
    //Deve essere settato all'inizio col Materiale dell'aereo nella scena.
    public Material planeMaterial;
    public bool experienceDone = false;

    public Material decalMaterial;
    public GameObject decalProjectors;

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += UpdateMaterial;
        SceneManager.sceneLoaded += ReloadHangarScene;
    }

    void UpdateMaterial(Scene scene, LoadSceneMode loadSceneMode)
    {
        //Trova i componenti dell'aereo o l'oggetto aereo ed assegnagli il materiale giusto
        GameObject aereo = GameObject.Find("aereo");
        GameObject fusoliera = GameObject.Find("fusoliera");

        //Recupera riferimento
        decalProjectors = GameObject.Find("Adesivi");
        

        if(aereo != null && fusoliera != null && scene.name.Equals("Scena_Volo"))
        {
            aereo.GetComponent<MeshRenderer>().material = planeMaterial;
            fusoliera.GetComponent<MeshRenderer>().material = planeMaterial;

            
        }

        ChangeDecalMaterial(decalMaterial);

        
    }

    public void ChangeDecalMaterial(Material material)
    {
        decalMaterial = material;

        if(decalMaterial != null)
        {
            if(SceneManager.GetActiveScene().name.Equals("Scena_Volo"))
            {
                GameObject aereo = GameObject.Find("aereo");

                if(aereo != null)
                {
                    Material []materials = aereo.GetComponent<MeshRenderer>().materials;
                    materials[1] = decalMaterial;
                    aereo.GetComponent<MeshRenderer>().materials = materials;
                }
            }
            else if(SceneManager.GetActiveScene().name.Equals("Scena_Terra"))
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
