using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitioner2 : MonoBehaviour
{

    [SerializeField] private string sceneName;
    [SerializeField] private FadeEffect2 fadeEffect;

    public bool transition = false;

    private bool transitioning = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fadeEffect = GameObject.FindAnyObjectByType<FadeEffect2>();       
    }

    // Update is called once per frame
    void Update()
    {
        if(transition && !transitioning)
        {
            transition = false;
            StartTransition();
            return;
        }
        if(transitioning&&!fadeEffect.isFading)
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    public void StartTransition()
    {
        transitioning = true;
        fadeEffect.targetAlpha = 1.0f;
        fadeEffect.isFading = true;
    }

    

    // private IEnumerator SceneTransitioning()
    // {
    //     if(!fadeEffect.isFading)
    //     {
    //         fadeEffect.fadeIn=true;
    //         yield return new WaitForEndOfFrame();
    //     }

    //     if(fadeEffect.isFading)
    //     {
    //         yield return new WaitUntil( () => !fadeEffect.isFading );
    //         SceneManager.LoadScene(sceneName);
    //     }

    // }
}
