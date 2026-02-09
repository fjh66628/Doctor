using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class CraftManager : MonoBehaviour
{
    [SerializeField]HerbSQ mainHerbs;
    [SerializeField]AssistHerbSQ assistHerbs;
    [SerializeField]List<Herb> herbsReadyToCombin;//暂存待合成的草药列表
    [SerializeField]Medicine medicineReadyToCombin;//暂存待合成的药品
    [SerializeField]List<AssistHerb> assistHerbToCombin;//暂存待合成的副药
    [SerializeField]InventoryManager inventoryManager;
    [SerializeField]List<Image> images;
    [SerializeField] MedicineSQ medicineExamples;
    int HRTindex= 0 ;
    int AHTindex = 0 ;//两个列表的指数
    void OnEnable()
    {
        EventManager.SeleckedMainHerb += AddHerbsReadyToCombin;
        EventManager.SeleckedAssistHerb += AddAssistHerbToCombin;
        EventManager.SeleckedMedicine += AddMedicineReadyToCombin;
    }
    void OnDisable()
    {
        EventManager.SeleckedMainHerb -= AddHerbsReadyToCombin;
        EventManager.SeleckedAssistHerb -= AddAssistHerbToCombin;
        EventManager.SeleckedMedicine -= AddMedicineReadyToCombin;

    }
    private void Start()
    {

    }//开始的时候重置各组件状态
    void AddHerbsReadyToCombin(int i)
    {

        herbsReadyToCombin[HRTindex] = inventoryManager.GetHerb(i);
        images[HRTindex].sprite = herbsReadyToCombin[i].getHerbSprite;
        HRTindex++;
        HRTindex %= 3;
    }
    void AddMedicineReadyToCombin(int i)
    {
        medicineReadyToCombin = inventoryManager.GetMedicine(i);
        inventoryManager.DeleteMedicine(medicineExamples.getMedicinesList[0], i);
        images[3].sprite = medicineReadyToCombin.getMedicineSprite;
    }
    void AddAssistHerbToCombin(int i)
    {
        assistHerbToCombin[AHTindex] = inventoryManager.GetAssistHerb(i);
        images[AHTindex+4].sprite = assistHerbToCombin[i].getAssistHerbSprite;
        AHTindex++; AHTindex %= 2;
    }//添加到合成台缓冲

    public void CleanCraft()
    {
        medicineReadyToCombin = medicineExamples.getMedicinesList[0];
        herbsReadyToCombin[0] = mainHerbs.getHerbList[0];
        herbsReadyToCombin[1] = mainHerbs.getHerbList[0];
        herbsReadyToCombin[2] = mainHerbs.getHerbList[0];
        assistHerbToCombin[0] = assistHerbs.getAssistHerbList[0];
        assistHerbToCombin[1] = assistHerbs.getAssistHerbList[0];
        ImageUpDate();
        EventManager.CallMedicineInventoryUPdate();
    }//清理合成台(所有类的【0】都是空，即基本对象)
    public void ComplitCombination()
    {
        int count = 0;
        int medicineOutWound=0;
        int medicineInternalWound=0;
        for (int i = 0; i < 3; i++)
        {
            if (herbsReadyToCombin[i].getHerbName != "空")
            {
                count++;
                medicineOutWound += herbsReadyToCombin[i].getOutsideWound;
                medicineInternalWound += herbsReadyToCombin[i].getInternalWound;
            }
            herbsReadyToCombin[i] = mainHerbs.getHerbList[0];
        }
        if (count > 0)
        {
            medicineInternalWound /= count;
            medicineOutWound /= count;
        }
        Medicine medicine = new Medicine(); 
        medicine.ChangeInternalWound(medicineInternalWound);
        medicine.ChangeOutsideWound(medicineOutWound);
        medicine.ChangeSprite(medicineExamples.getMedicinesList[count].getMedicineSprite);//从1开始后是药品样例
        medicine.ChangeName(medicineExamples.getMedicinesList[count].getMedicineName);
        medicine.ChangeDetail();
        if(count > 0)
            inventoryManager.AddComebineMedicine(medicine);
        EventManager.CallMedicineInventoryUPdate();
        ImageUpDate();
        if (medicineReadyToCombin.getMedicineName != "空")
        {
            medicineOutWound = 0;
            medicineInternalWound = 0;
            for (int i = 0; i < 2; i++)
            {
                medicineInternalWound += assistHerbToCombin[i].getInternalWound;
                medicineOutWound += assistHerbToCombin[i].getOutsideWound;
                assistHerbToCombin[i] = assistHerbs.getAssistHerbList[0];
            }
            medicineReadyToCombin.ChangeOutsideWound(medicineOutWound + medicineReadyToCombin.getOutsideWound);
            medicineReadyToCombin.ChangeInternalWound(medicineInternalWound + medicineReadyToCombin.getInsideWound);
            medicineReadyToCombin.ChangeDetail();
            inventoryManager.AddComebineMedicine(medicineReadyToCombin);
            medicineReadyToCombin = medicineExamples.getMedicinesList[0];
            EventManager.CallMedicineInventoryUPdate();
            ImageUpDate();
        }
    }//合成函数
    public void ImageUpDate()
    {
        for (int i = 0; i < 3; i++) 
        {
            images[i].sprite = herbsReadyToCombin[i].getHerbSprite;
        }
        images[3].sprite = medicineReadyToCombin.getMedicineSprite;
        for(int i = 0;i < 2;i++)
        {
            images[i + 4].sprite = assistHerbToCombin[i].getAssistHerbSprite;
        }
    }//点击按钮传入图标
}
