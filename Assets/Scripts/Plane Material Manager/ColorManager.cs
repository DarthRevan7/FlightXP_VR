using UnityEngine;

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
        //Trova i componenti dell'aereo o l'oggetto aereo ed assegnagli il materiale giusto

    }
}
