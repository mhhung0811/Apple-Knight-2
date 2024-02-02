using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{ 
    private static AudioManager _instance;
    public static AudioManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindAnyObjectByType<AudioManager>();
            }
            return _instance;
        }
    }

    [SerializeField]
    private SoundConfigs _soundConfigs;

    [SerializeField]
    private Queue<GameObject> _activeSound;
    [SerializeField]
    private GameObject _soundPrefab;
    private int _soundPrepare;

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        _soundPrepare = 10;
        _activeSound = new Queue<GameObject>();
        Prepare();
    }

    public void PlaySound(string soundKey)
    {
        AudioClip clip = _soundConfigs.GetSoundByKey(soundKey);
        if(clip != null)
        {
            GameObject sound = AudioManager.Instance.Take();

            sound.GetComponent<AudioSource>().clip = clip;

            sound.GetComponent<AudioSource>().Play();
        }
    }
    public void PlayMusic(string soundKey)
    {
        AudioClip clip = _soundConfigs.GetSoundByKey(soundKey);
        if (clip != null)
        {
            GameObject sound = AudioManager.Instance.Take();

            sound.GetComponent<AudioSource>().clip = clip;

            sound.GetComponent<AudioSource>().loop = true;

            sound.GetComponent<AudioSource>().Play();
        }
    }


    public void Prepare()
    {
        for(int i= 0; i< _soundPrepare; i++)
        {
            GameObject sound = Instantiate(_soundPrefab, transform);
            sound.gameObject.SetActive(false);
            _activeSound.Enqueue(sound);
        }
    }

    public GameObject Take()
    {
        if( _activeSound.Count <= 0)
        {
            Prepare();
        }
        GameObject sound = this._activeSound.Dequeue();
        sound.gameObject.SetActive(true);
        return sound;
    }

    public void Return(GameObject sound)
    {
        this._activeSound.Enqueue(sound);
        sound.SetActive(false);
    }
}
