using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class DTextManager : SingletonMono<DTextManager>
{
    public GameObject textPrefab;
    public void CreateText()
    {
        var dt = Instantiate(textPrefab);
    }
    public void CreateText(string s)
    {
        var dt = Instantiate(textPrefab).GetComponent<DynamicText>();
        dt.tmp.text = s;
    }
    public void CreateText(Vector3 pos, string s)
    {
        var dt = Instantiate(textPrefab, transform).GetComponent<DynamicText>();
        dt.gameObject.transform.position = pos;
        dt.tmp.text = s;
    }
    public void CreateText(Transform t, string s)
    {
        var dt = Instantiate(textPrefab, transform).GetComponent<DynamicText>();
        dt.gameObject.transform.position = t.transform.position;
        dt.tmp.text = s;
    }
    /// <summary>
    /// 创建受伤或者治疗的动态文本,负数为伤害
    /// </summary>
    /// <param name="t"></param>
    /// <param name="i"></param>
    public void CreateHurtText(Transform t, int i)
    {
        var dt = Instantiate(textPrefab, t).GetComponent<DynamicText>();
        //dt.tmp.text = i.ToString();
        dt.ChangeToHurtText(i);
    }
    public void CreateHurtText(Vector3 pos, int i)
    {
        var dt = Instantiate(textPrefab, transform).GetComponent<DynamicText>();
        dt.gameObject.transform.position = pos;
        //dt.tmp.text = i.ToString();
        dt.ChangeToHurtText(i);
    }
}
