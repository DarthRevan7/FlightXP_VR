using UnityEngine;

public class DragXPUIManager : MonoBehaviour
{

    [SerializeField] private GameObject []coverPanels;
    //[SerializeField] private GameObject []UICards;
    //private int []unlockedCard;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        /*
        unlockedCard = new int[coverPanels.Length];

        for(int i=0;i<unlockedCard.Length;i++)
        {
            unlockedCard[i] = 0;
        }
        */

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UnlockUICard(int index)
    {
        if(index >= 0 && index < coverPanels.Length)
        {
            coverPanels[index].SetActive(false);
        }
    }
}
