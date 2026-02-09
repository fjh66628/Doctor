using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Herb
{
    [Header("基础信息")]
    [SerializeField] string herbName;
    [SerializeField] string herbDetail;
    [SerializeField] Sprite herbSprite;
    [Header("精神")]
    [SerializeField] int mindWound;
    [Header("外伤")]
    [SerializeField] int outsideWound;
    [Header("内伤")]
    [SerializeField] int internalWound;

    public Herb(string name, string detail, Sprite sprite, int internalwound, int outside, int mind)
    {
        herbName = name;
        herbDetail = detail;
        herbSprite = sprite;

        internalWound = internalwound;
        outsideWound = outside;
        mindWound = mind;
    }
    public string getHerbName => herbName;
    public string getHerbDetail => herbDetail;
    public int getMindWound => mindWound;
    public int getOutsideWound => outsideWound;
    public int getInternalWound => internalWound;
    public Sprite getHerbSprite => herbSprite;
    public void ChangeInternalWound(int wound)
        { internalWound = wound; }
    public void ChangeOutsideWound(int wound) { outsideWound = wound; }
    public void ChangeName(string name) { herbName = name; }
    public void ChangeDetail() { herbDetail = "外伤" + outsideWound + "\n" + "内伤" + internalWound + "\n"; }
    public void ChangeSprite(Sprite sprite) { herbSprite = sprite; }
}
