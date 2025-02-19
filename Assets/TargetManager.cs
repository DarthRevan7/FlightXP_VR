using System;
using UnityEngine;

public class TargetManager : MonoBehaviour
{

    public String victoryScene;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.childCount == 0)
        {
            SceneTransitioner2 tr = FindAnyObjectByType<SceneTransitioner2>();
            tr.sceneName = victoryScene;
            tr.StartTransition();
        }
    }
}
