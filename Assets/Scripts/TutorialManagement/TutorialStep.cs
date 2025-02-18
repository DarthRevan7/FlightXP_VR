using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "TutorialStep", menuName = "Scriptable Objects/TutorialStep")]
public class TutorialStep : ScriptableObject
{
    public AudioClip audioClip;
    public Sprite tutorialSprite;
    public int step;
}
