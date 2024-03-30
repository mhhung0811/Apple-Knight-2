using System;
using UnityEngine;

[Serializable]
public class PlayerGameData
{
    public int _score;
    public int _totalTime;
    public int _exp;
    public int _HP;
    public int _Mana;
    public Vector3 _Position;

    public PlayerGameData()
    {
        _score = 0;
        _totalTime = 0;
        _exp = 0;
        _HP = 100;
        _Mana = 100;
        _Position = new Vector3();
    }
}
