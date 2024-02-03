using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public RawImage MenuGameOver;
    public Image MenuImage;
    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void GameOver()
    {
        MenuGameOver.gameObject.SetActive(true);
    }
    public void Victory()
    {
        MenuImage.gameObject.SetActive(true);
    }
}
