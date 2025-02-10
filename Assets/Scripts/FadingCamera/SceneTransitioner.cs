using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitioner : MonoBehaviour
{

    [SerializeField] private string sceneName;
    [SerializeField] private FadeEffect fadeEffect;

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fadeEffect = GameObject.FindAnyObjectByType<FadeEffect>();

        fadeEffect.fadeOut=true;

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartTransition()
    {
        StartCoroutine(SceneTransitioning());
    }

    private IEnumerator SceneTransitioning()
    {
        if(!fadeEffect.isFading)
        {
            fadeEffect.fadeIn=true;
            yield return new WaitForEndOfFrame();
        }

        if(fadeEffect.isFading)
        {
            yield return new WaitUntil( () => !fadeEffect.isFading );
            SceneManager.LoadScene(sceneName);
        }

    }
}
