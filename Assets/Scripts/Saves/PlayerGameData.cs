using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerGameData
{
    public float Hp;
    public int Mana;
    public int Exp;
    public Vector3 PlayerPosition;
    public PlayerGameData()
    {
        Hp = 100;
        Mana = 100;
        Exp = 0;
        PlayerPosition = Vector3.zero;

    }
}
