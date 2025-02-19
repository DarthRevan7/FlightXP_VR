using UnityEngine;
using UnityEngine.UI;

public class BaseTutorialExtra : MonoBehaviour
{
    public Image image;
    public AudioClip audioClip;
    public AudioSource audioSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.PlayDelayed(1f);
    }

    // Update is called once per frame
    void Update()
    {
        ImageFading();
    }
    
    void ImageFading()
    {
        if(audioSource.isPlaying && image.color.a < 255)
        {
            image.color = new Color(255,255,255,image.color.a+1);
        }
        if(!audioSource.isPlaying && image.color.a > 0)
        {
            image.color = new Color(255,255,255,image.color.a-1);
        }
    }
}
