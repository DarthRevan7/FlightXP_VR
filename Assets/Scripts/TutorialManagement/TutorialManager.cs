using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    
    public TutorialStep[] tutorialSteps;



    void Awake()
    {
        tutorialSteps = Resources.LoadAll<TutorialStep>("Tutorial Steps\\");
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
