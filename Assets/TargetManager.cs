using System;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    public List<Vector3> targetPositions;
    public String victoryScene;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targetPositions = new List<Vector3>();
        for(int i = 0; i < transform.childCount; i++)
        {
            targetPositions.Add(transform.GetChild(i).position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        targetPositions = new List<Vector3>();
        for(int i = 0; i < transform.childCount; i++)
        {
            targetPositions.Add(transform.GetChild(i).position);
        }
        if(transform.childCount == 0)
        {
            SceneTransitioner2 tr = FindAnyObjectByType<SceneTransitioner2>();
            tr.sceneName = victoryScene;
            tr.StartTransition();
        }
    }
}
