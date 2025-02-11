using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ColorManager : MonoBehaviour
{
    //Deve essere settato all'inizio col Materiale dell'aereo nella scena.
    public Material planeMaterial;
    public bool experienceDone = false;

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

        if(aereo != null && fusoliera != null && scene.name.Equals("Scena_Volo"))
        {
            aereo.GetComponent<MeshRenderer>().material = planeMaterial;
            fusoliera.GetComponent<MeshRenderer>().material = planeMaterial;
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

                GameObject.Destroy(aereoPezzi);
                draggingManager.SetPlaneMaterial(planeMaterial);

            }
        }


    }
}
