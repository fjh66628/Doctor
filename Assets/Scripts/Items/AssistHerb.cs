using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class AssistHerb
{
    
    [Header("基础信息")]
    [SerializeField] string assistHerbName;
    [SerializeField] string assistHerbDetail;
    [SerializeField] Sprite assistHerbSprite;
    [Header("精神")]
    [SerializeField] int mindWound;
    [Header("外伤")]
    [SerializeField] int outsideWound;
    [Header("内伤")]
    [SerializeField] int internalWound;

    public AssistHerb(string name, string detail, Sprite sprite, int internalwound, int outside, int mind)
    {
        assistHerbName = name;
        assistHerbDetail = detail;
        assistHerbSprite = sprite;

        internalWound = internalwound;
        outsideWound = outside;
        mindWound = mind;
    }
    public string getAssistHerbName => assistHerbName;
    public string getAssistHerbDetail => assistHerbDetail;
    public int getMindWound => mindWound;
    public int getOutsideWound => outsideWound;
    public int getInternalWound => internalWound;
    public Sprite getAssistHerbSprite => assistHerbSprite;
    public void ChangeInternalWound(int wound)
        { internalWound = wound; }
    public void ChangeOutsideWound(int wound) { outsideWound = wound; }
    public void ChangeName(string name) { assistHerbName = name; }
    public void ChangeDetail() { assistHerbDetail = "外伤" + outsideWound + "\n" + "内伤" + internalWound + "\n"; }
    public void ChangeSprite(Sprite sprite) { assistHerbSprite = sprite; }
    
}
