using UnityEngine;
using UnityEngine.Timeline;

[CreateAssetMenu(fileName = "TutorialStep", menuName = "Scriptable Objects/TutorialStep")]
public class TutorialStep : ScriptableObject
{
    public AudioTrack track;
    public int step;
}
