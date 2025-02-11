using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ColorManager : MonoBehaviour
{
    //Deve essere settato all'inizio col Materiale dell'aereo nella scena.
    public Material planeMaterial;

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += UpdateMaterial;
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
}
