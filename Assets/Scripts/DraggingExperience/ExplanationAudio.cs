using UnityEngine;
using UnityEngine.Timeline;

[CreateAssetMenu(fileName = "ExplanationAudio", menuName = "Scriptable Objects/ExplanationAudio")]
public class ExplanationAudio : ScriptableObject
{
    public AudioClip audioClip;
    public int UICardIndex;
}
