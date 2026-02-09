using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Illness
{
    [Header("基础信息")]
    [SerializeField] string illnessName="unnamed";
    [SerializeField] string illnessDetail="no detail";
    [SerializeField] Sprite illnessSprite;
    
    [Header("内伤")]
    [SerializeField] int insideWound=0;

    [Header("外伤")]
    [SerializeField] int outsideWound=0;

    [Header("精神伤害")]
    [SerializeField] int spiritWound=0;

    public Illness(string name, string detail, Sprite sprite, int inside, int outside, int spirit)
    {
        illnessName = name;
        illnessDetail = detail;
        illnessSprite = sprite;
        insideWound = inside;
        outsideWound = outside;
        spiritWound = spirit;
    }
    //detail
    public string getIllnessName => illnessName;
    public string getIllnessDetail => illnessDetail;
    public Sprite getIllnessSprite => illnessSprite;

    //wound values
    public int getInsideWound => insideWound;
    public int getOutsideWound => outsideWound;
    public int getSpiritWound => spiritWound;
}
