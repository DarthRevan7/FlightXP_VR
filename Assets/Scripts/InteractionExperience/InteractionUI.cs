using UnityEngine;
using TMPro;

public class InteractionUI : MonoBehaviour
{

    [SerializeField] private TMP_Text tMP_Text;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interaction()
    {
        if(tMP_Text != null)
        {
            tMP_Text.text = "Interacted with: " + gameObject.ToString();
        }
    }

}
