using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<UIManager>();
            }
            return _instance;
        }
    }

    public GameObject MenuGameOver;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void GameOver()
    {

    }
}
