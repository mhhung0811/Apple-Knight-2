using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private bool isLoad;
    private void Awake()
    {
        if(_instance == null)
            _instance = this;
        else
            Destroy(this.gameObject);
    }
    void Start()
    {
        isLoad = true;
        //StartCoroutine(Loadata());
       // LoadPlayerGameData();
        //LoadTreeSkillData();
    }

    void Update()
    {
        if(isLoad)
        {
            StartCoroutine(Loadata());
        }
    }
    public IEnumerator Loadata()
    {
        isLoad = false;
        yield return new WaitForSeconds(0.1f);
        LoadPlayerGameData();
        //yield return new WaitForSeconds(01f);
        LoadTreeSkillData();
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
        }
    }
    public void SaveScore(int newScore)
    {
        _data ??= new PlayerGameData();
        _data._score = newScore; 

        PlayerPrefs.SetString(key: "DATA",value: JsonUtility.ToJson(_data));
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
   
}
