using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveDataManager : MonoBehaviour
{
    private static SaveDataManager _instance;
    public static SaveDataManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SaveDataManager>();
            }
            return _instance;
        }
    }

    private PlayerGameData _data;
    private TreeSkillData _skillData;
    private OptionPlayData _playModeData;
    private EnemyInGameData _enemyIGData;
    private HighScoreData _highScoreData;
    private bool isLoad;

    [SerializeField]
    private GameObject Player;
    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(this.gameObject);
    }
    void Start()
    {
        isLoad = true;
    }

    void Update()
    {
        if (isLoad)
        {
            StartCoroutine(Loadata());
        }
    }
    public IEnumerator Loadata()
    {
        isLoad = false;
        if (PlayerPrefs.HasKey("CONTINUE"))
        {
            //Kiểm tra người dùng có ấn vô nút Play continue không
            string save = PlayerPrefs.GetString("CONTINUE");
            Debug.Log(save);
            _playModeData = JsonUtility.FromJson<OptionPlayData>(save);

            Scene currentScene = SceneManager.GetActiveScene();

            if (_playModeData._isContinue == true && currentScene.name == "Level 1")
            {
                yield return new WaitForSeconds(0.1f);
                LoadPlayerGameData();
                LoadTreeSkillData();
                LoadEnemyIngame();
                //SaveOptionPlay(false);
            }
            if(currentScene.name == "Level 1")
            {
                LoadHighScore();
                if(_playModeData._isContinue == false)
                {
                    GameManager.Instance.DeleteKey();
                }
            }
        }

    }


    public void LoadPlayerGameData()
    {
        if (PlayerPrefs.HasKey("DATA"))
        {
            string save = PlayerPrefs.GetString("DATA");
            Debug.Log(save);

            _data = JsonUtility.FromJson<PlayerGameData>(save);

            // exp va score
            InGameManager.Instance.IncreaseExp(_data._exp, _data._score);
            // totalTime
            InGameManager.Instance.SetTotalTime(_data._totalTime);

            if (Player != null)
            {
                Player.GetComponent<PlayerBehavior>().SetMana(_data._Mana);
                Player.GetComponent<PlayerBehavior>().SetPosition(_data._Position);
                Player.GetComponent<PlayerCombatController>().SetHP(_data._HP);
            }
        }
    }
    public void SaveScore(int newScore)
    {
        _data ??= new PlayerGameData();
        _data._score = newScore;

        PlayerPrefs.SetString(key: "DATA", value: JsonUtility.ToJson(_data));
        PlayerPrefs.Save();
    }

    public void SaveExp(int newExp)
    {
        _data ??= new PlayerGameData();
        _data._exp = newExp;

        PlayerPrefs.SetString(key: "DATA", value: JsonUtility.ToJson(_data));
        PlayerPrefs.Save();
    }

    public void SaveTotalTime(int newTotalTime)
    {
        _data ??= new PlayerGameData();
        _data._totalTime = newTotalTime;

        PlayerPrefs.SetString(key: "DATA", value: JsonUtility.ToJson(_data));
        PlayerPrefs.Save();
    }

    public void SaveHP(int newHP)
    {
        _data ??= new PlayerGameData();
        _data._HP = newHP;

        PlayerPrefs.SetString(key: "DATA", value: JsonUtility.ToJson(_data));
        PlayerPrefs.Save();
    }

    public void SaveMana(int newMana)
    {
        _data ??= new PlayerGameData();
        _data._Mana = newMana;

        PlayerPrefs.SetString(key: "DATA", value: JsonUtility.ToJson(_data));
        PlayerPrefs.Save();
    }
    public void SavePosition(Vector3 newPosition)
    {
        _data ??= new PlayerGameData();
        _data._Position = newPosition;

        PlayerPrefs.SetString(key: "DATA", value: JsonUtility.ToJson(_data));
        PlayerPrefs.Save();
    }

    //Tree Skill Save
    public void LoadTreeSkillData()
    {
        if (PlayerPrefs.HasKey("SKILL_DATA"))
        {
            string save = PlayerPrefs.GetString("SKILL_DATA");
            Debug.Log(save);

            _skillData = JsonUtility.FromJson<TreeSkillData>(save);

            foreach(int id in _skillData._idSkill)
            {
                SkillManager.Instance.SetUpgradeSkill(id);
            }

            SkillManager.Instance.SetPointUpgrade(_data._exp, _skillData._idSkill.Count);
        }
    }
    public void SaveSkillUpgraded(int newIdSkillUpgraded)
    {
        _skillData ??= new TreeSkillData();
        _skillData._idSkill.Add(newIdSkillUpgraded);

        PlayerPrefs.SetString(key: "SKILL_DATA", value: JsonUtility.ToJson(_skillData));
        PlayerPrefs.Save();
    }
    // Load and Save play new or continue
    public void SaveOptionPlay(bool newIsContinue)
    {
        _playModeData ??= new OptionPlayData();
        _playModeData._isContinue = newIsContinue;

        PlayerPrefs.SetString(key: "CONTINUE", value: JsonUtility.ToJson(_playModeData));
        PlayerPrefs.Save();
    }
    // Load and save Enemy
    public void LoadEnemyIngame()
    {
        if (PlayerPrefs.HasKey("ENEMY"))
        {
            string save = PlayerPrefs.GetString("ENEMY");
            Debug.Log(save);

            _enemyIGData = JsonUtility.FromJson<EnemyInGameData>(save);
            for(int i = 0; i < _enemyIGData._enemy.Count; i++)
            {
                EnemyManager.Instance.SetAactiveEnemyDie(_enemyIGData._enemy[i]);
            }
        }
    }
    public void SaveEnemyInGameData(int idEnemy)
    {
        _enemyIGData ??= new EnemyInGameData();
        _enemyIGData._enemy.Add(idEnemy);

        PlayerPrefs.SetString(key: "ENEMY", value: JsonUtility.ToJson(_enemyIGData));
        PlayerPrefs.Save();
    }
    //Load and Save highScore
    public void LoadHighScore()
    {
        if (PlayerPrefs.HasKey("HIGH_SCORE"))
        {
            string save = PlayerPrefs.GetString("HIGH_SCORE");
            Debug.Log(save);

            _highScoreData = JsonUtility.FromJson<HighScoreData>(save);
            InGameManager.Instance.SetHighScore(_highScoreData._highScore);
        }
    }
    public void SaveHighScore(int newHighScore)
    {
        _highScoreData ??= new HighScoreData();
        _highScoreData._highScore = newHighScore;

        PlayerPrefs.SetString(key: "HIGH_SCORE",value: JsonUtility.ToJson(_highScoreData));
        PlayerPrefs.Save();
    }
}

[Serializable]
public class OptionPlayData
{
    public bool _isContinue;
    public OptionPlayData()
    {
        _isContinue = false;
    }
}

[Serializable]
public class EnemyInGameData
{
    public List<int> _enemy;
    public EnemyInGameData()
    {
        _enemy = new List<int>();
    }
}

[Serializable]
public class HighScoreData
{
    public int _highScore;
    public HighScoreData()
    {
        _highScore = 0;
    }
}
