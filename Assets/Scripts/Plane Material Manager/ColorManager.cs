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

        if(decalProjectors != null)
        {
            Debug.Log("decalproj not null");
            //Se il materiale è null
            if(decalMaterial == null)
            {
                //Disattiva l'oggetto proiettore decal
                decalProjectors.SetActive(false);
            }
            else
            {
                decalProjectors.SetActive(true);

                //Aggiorna il materiale del decal dell'aereo
                DecalProjector[] projectors = decalProjectors.GetComponentsInChildren<DecalProjector>();
                for(int i = 0; i < projectors.Length; i++)
                {
                    projectors[i].material = decalMaterial;
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
                draggingManager.SetPlaneMaterial(planeMaterial);

            }
        }


    }
}
