using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    private Rigidbody2D myRb;
    public GameObject DivineDepature;
    void Start()
    {
        myRb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            SkillSentoryu();
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            SkillHoaDon();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            SkillDivineDepature();
        }
    }

    private void SkillSentoryu()
    {
        StartCoroutine(SpawnSentoryu());
    }

    private void SkillHoaDon()
    {
        StartCoroutine(SpawnHoaCau());
    }

    private void SkillDivineDepature()
    {
        DivineDepature = transform.Find("DivineDepature").gameObject;
        DivineDepature.SetActive(true);
        Divine d = DivineDepature.GetComponent<Divine>();
        d.Beng();
        StartCoroutine(SpawnDivine());  
    }

    private IEnumerator SpawnDivine()
    {
        myRb.velocity = new Vector2(0, 20f);
        yield return new WaitForSeconds(0.5f);
        myRb.velocity = new Vector2(0, -60f);
    }
    private IEnumerator SpawnHoaCau()
    {
        myRb.gravityScale = 0f;
        myRb.velocity = Vector3.zero;
        yield return new WaitForSeconds(0.5f);
        GameObject hoadon = BulletManager.Instance.TakeHoaDon();
        hoadon.transform.position = this.transform.position;
        HoaDon hd = hoadon.GetComponent<HoaDon>();
        hd.SetUp(1);
        yield return new WaitForSeconds(0.2f);
        myRb.gravityScale = 4f;
    }

    private IEnumerator SpawnSentoryu()
    {
        myRb.gravityScale = 0f;
        for(int i = 1; i <= 5; i++)
        {
            myRb.velocity = Vector3.zero;

            if (i == 1)
            {
                GameObject str1 = BulletManager.Instance.TakeSentoryu1();
                str1.transform.position = this.transform.position;
                Sentoryu s = str1.GetComponent<Sentoryu>();
                s.SetUp(1,1);
            }

            if (i == 2)
            {
                GameObject str2 = BulletManager.Instance.TakeSentoryu2();
                str2.transform.position = this.transform.position;
                Sentoryu s = str2.GetComponent<Sentoryu>();
                s.SetUp(1,2);
            }

            if (i == 3)
            {
                GameObject str3 = BulletManager.Instance.TakeSentoryu3();
                str3.transform.position = this.transform.position;
                Sentoryu s = str3.GetComponent<Sentoryu>();
                s.SetUp(1,3);
            }

            if(i < 2)
            {
                yield return new WaitForSeconds(0.2f);
            }
            else if(i == 2)
            {
                yield return new WaitForSeconds(0.5f);
            }
            else
            {
                yield return new WaitForSeconds(0.1f);
            }
        }
        myRb.gravityScale = 4f;
    }
}
