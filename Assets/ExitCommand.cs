using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ExitCommand : MonoBehaviour
{
    public InputAction exit_action;
    public String sceneToExitTo;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(exit_action!=null && exit_action.IsPressed()) 
        {
            SceneTransitioner2 tr = FindAnyObjectByType<SceneTransitioner2>();
            tr.sceneName = sceneToExitTo;
            tr.StartTransition();
        }
    }
}
