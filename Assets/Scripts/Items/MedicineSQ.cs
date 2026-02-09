using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "MedicineSQ", menuName = "Game Data/Mesicines")]
public class MedicineSQ : ScriptableObject
{
    [SerializeField] private List<Medicine> medicineList = new List<Medicine>();
    
    // 通过属性公开列表，以便在其他脚本中访问
    public List<Medicine> getMedicinesList => medicineList;
}
