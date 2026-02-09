using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Medicine
{
    [Header("基础信息")]
    [SerializeField] string medicineName;
    [SerializeField] string medicineDetail;
    [SerializeField] Sprite medicineSprite;

    [Header("内伤")]
    [SerializeField] int insideWound;
    [Header("外伤")]
    [SerializeField] int outsideWound;
    [Header("精神")]
    [SerializeField] int mindWound;
    

    //detail
    public string getMedicineName => medicineName;
    public string getMedicineDetail => medicineDetail;
    public Sprite getMedicineSprite => medicineSprite;

    //wound values
    public int getInsideWound => insideWound;
    public int getOutsideWound => outsideWound;
    public int getMindWound => mindWound;

    public void ChangeInternalWound(int wound)
    { insideWound = wound; }
    public void ChangeOutsideWound(int wound) { outsideWound = wound; }
    public void ChangeName(string name) {  medicineName = name; }
    public void ChangeDetail() { medicineDetail = "外伤" + outsideWound + "\n" + "内伤" + insideWound + "\n"; }
    public void ChangeSprite(Sprite sprite) { medicineSprite = sprite; }
    
}
