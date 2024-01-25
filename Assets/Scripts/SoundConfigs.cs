using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundConfigs", menuName = "ScriptableObject/SoundConfigs")]
public class SoundConfigs : ScriptableObject
{
    public List<AudioClip> _listSound;

    public AudioClip GetSoundByKey(string soundFileName)
    {
        return _listSound.Find(x => x.name.Equals(soundFileName));
    }
}
