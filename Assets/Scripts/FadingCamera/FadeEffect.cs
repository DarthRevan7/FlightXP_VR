using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeEffect : MonoBehaviour
{
    // Riferimento all'oggetto Image da usare per il fade
    [SerializeField] private Image fadeImage;

    // Durata del fade (in secondi)
    [SerializeField] private float fadeDuration = 1.0f;

    // Flag per sapere se il fade è in corso
    public bool isFading = false;
    public bool fadeIn = false, fadeOut=false;
    

    void Start()
    {
        // Inizializza il fade a 1 (Opaco)
        fadeImage.canvasRenderer.SetAlpha(1.0f);
        //Attiva il gameobject dell'image x il fading e fa FadeOut ad ogni avvio delle scene, per facilitare la transizione.
        fadeImage.gameObject.SetActive(true);
        fadeOut=true;
    }

    void Update()
    {
        if(fadeIn)
        {
            FadeIn();
            fadeIn=false;
        }

        if(fadeOut)
        {
            FadeOut();
            fadeOut=!fadeOut;
        }
    }

    // Funzione per attivare il fade in
    public void FadeIn()
    {
        if (!isFading)
            StartCoroutine(FadeTo(1.0f));
    }

    // Funzione per attivare il fade out
    public void FadeOut()
    {
        if (!isFading)
            StartCoroutine(FadeTo(0.0f));
    }

    // Coroutine per eseguire l'effetto di fade
    private IEnumerator FadeTo(float targetAlpha)
    {
        
        isFading = true;
        float startAlpha = fadeImage.canvasRenderer.GetAlpha();
        float timeElapsed = 0f;

        while (timeElapsed < fadeDuration)
        {
            // Interpolazione lineare dell'alpha tra startAlpha e targetAlpha
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, timeElapsed / fadeDuration);
            fadeImage.canvasRenderer.SetAlpha(newAlpha);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Assicurati che l'alpha arrivi esattamente al target
        fadeImage.canvasRenderer.SetAlpha(targetAlpha);
        isFading = false;
    }
}
