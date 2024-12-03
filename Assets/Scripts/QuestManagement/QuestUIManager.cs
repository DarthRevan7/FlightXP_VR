using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class QuestUIManager : MonoBehaviour
{

    [SerializeField] private List<QuestUIElement> questUIElements;


    private void Awake() 
    {
        questUIElements = Resources.LoadAll<QuestUIElement>("Quest\\Movement Tutorial").ToList();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
