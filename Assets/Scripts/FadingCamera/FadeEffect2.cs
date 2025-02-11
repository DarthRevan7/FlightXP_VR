using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class FadeEffect2 : MonoBehaviour
{

    [SerializeField]
    private InputActionReference toggle_fade_input;
    private float last_fade_toggle = 0;
    private bool last_fade_state = false;
    // Riferimento all'oggetto Image da usare per il fade
    [SerializeField] private Image fadeImage;

    // Durata del fade (in secondi)
    [SerializeField] private float fadeDuration = 1.0f;

    // Flag per sapere se il fade è in corso
    public bool isFading = false;
    // public bool fadeIn = false, fadeOut=false;
    public float targetAlpha = 0.0f;
    

    void Start()
    {
        // Inizializza il fade a 1 (Opaco)
        fadeImage.canvasRenderer.SetAlpha(1.0f);
        //Attiva il gameobject dell'image x il fading e fa FadeOut ad ogni avvio delle scene, per facilitare la transizione.
        fadeImage.gameObject.SetActive(true);
        targetAlpha=0.0f;
    }

    void Update()
    {
        if(toggle_fade_input!=null && toggle_fade_input.action.IsPressed() && last_fade_toggle - Time.time > 1.0f)
        {
            if(last_fade_state) targetAlpha = 0.0f;
            else targetAlpha = 1.0f;

            last_fade_toggle = Time.time;
            last_fade_state = !last_fade_state;
        }
        float current_alpha = fadeImage.canvasRenderer.GetAlpha();
        float step_size = 1.0f / fadeDuration * Time.deltaTime;
        if(targetAlpha!=fadeImage.canvasRenderer.GetAlpha())
        {
            isFading = true;
            if(Mathf.Abs(targetAlpha-current_alpha) <= step_size) fadeImage.canvasRenderer.SetAlpha(targetAlpha);
            else
            {
                if(targetAlpha > current_alpha) current_alpha += step_size;
                else current_alpha -= step_size;

                fadeImage.canvasRenderer.SetAlpha(current_alpha);
            }
        }
        else
        {
            isFading = false;
        }
    }
}
