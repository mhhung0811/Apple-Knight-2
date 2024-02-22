using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillSound : MonoBehaviour
{
    private AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (InGameManager.Instance.PauseGame())
        {
            return;
        }
        if (!audioSource.isPlaying)
        {
            AudioManager.Instance.Return(this.gameObject);
        }
    }
}
