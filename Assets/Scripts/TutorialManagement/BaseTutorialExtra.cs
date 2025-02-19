using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BaseTutorialExtra : MonoBehaviour
{
    public Image image;
    public AudioClip audioClip, shootAudioClip;
    public AudioSource audioSource;
    public Sprite sprite, shootSprite;


    public string sceneMode;

    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioClip;
        // audioSource.Play();
        image = GameObject.Find("TutorialImage").GetComponent<Image>();
        
        if(sceneMode.Equals("Base"))
        {
            StartCoroutine(BaseCoroutine());
        }
        else if(sceneMode.Equals("Shoot"))
        {
            StartCoroutine(ShootCoroutine());
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator BaseCoroutine()
    {
        yield return new WaitForSeconds(2f);
        audioSource.Play();
        image.sprite = sprite;
        image.color  = new Color(255,255,255,255);
        yield return new WaitUntil(() => !audioSource.isPlaying);
        image.color  = new Color(255,255,255,0);
    }

    private IEnumerator ShootCoroutine()
    {
        yield return new WaitForSeconds(2f);
        //Setto audioclip di benvenuto nell'esperienza di sparo
        audioSource.clip = shootAudioClip;
        if(shootSprite != null)
        {
            image.sprite = shootSprite;
            image.color  = new Color(255,255,255,255);
        }
        audioSource.Play();
        yield return new WaitUntil(() => !audioSource.isPlaying);
        image.color  = new Color(255,255,255,0);
        yield return new WaitForSeconds(1f);
        //Setto l'audioclip della conclusione dell'esperienza
        audioSource.clip = audioClip;
        audioSource.Play();
        image.sprite = sprite;
        image.color  = new Color(255,255,255,255);
        yield return new WaitUntil(() => !audioSource.isPlaying);
        image.color  = new Color(255,255,255,0);
        
    }
    
    
}
