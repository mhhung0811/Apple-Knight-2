using System;


[Serializable]
public class PlayerGameData
{
    public int _score;
    public int _totalTime;
    public int _exp;
    public int _HP;
    public int _Mana;

    public PlayerGameData()
    {
        _score = 0;
        _totalTime = 0;
        _exp = 0;
        _HP = 0;
        _Mana = 0;
    }
}
