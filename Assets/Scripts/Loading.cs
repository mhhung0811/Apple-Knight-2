using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loading : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(ClosePanelLoading());
    }

    void Update()
    {
        
    }

    public IEnumerator ClosePanelLoading()
    {
        yield return new WaitForSeconds(1);
        this.gameObject.SetActive(false);
    }
}
