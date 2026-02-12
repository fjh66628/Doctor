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

    [SerializeField]InventoryManager inventoryManager;
    [SerializeField]List<Image> images;
    [SerializeField] MedicineSQ medicineExamples;

    int craftSize;
    int HRTindex= 0 ;//暂存待合成的草药列表指数
    void OnEnable()
    {
        EventManager.SeleckedMainHerb += AddHerbsReadyToCombin;
        EventManager.UpdateDayEvent += CleanCraft;

    }
    void OnDisable()
    {
        EventManager.SeleckedMainHerb -= AddHerbsReadyToCombin;
        EventManager.UpdateDayEvent -= CleanCraft;


    }
    private void Start()
    {
        craftSize = GetCraftSize(GameManager.Instance.GetCurrentDay());
        CleanCraft();
    }//开始的时候重置各组件状态

    void AddHerbsReadyToCombin(Herb herb)
    {

        herbsReadyToCombin[HRTindex] = herb;
        images[HRTindex].sprite = herb.getHerbSprite;
        HRTindex++;
        HRTindex %= craftSize;
    }

    int GetCraftSize(int day)
    {
        switch (day)
        {
            case 1: return 1;
            case 2: return 1;
            case 3: return 2;
            case 4: return 2;
            case 5: return 3;
            case 6: return 3;
        }
        return -1;
    }//从gamemanager中获取天数解锁对应合成台
    public void CleanCraft()
    {

        herbsReadyToCombin[0] = null;
        herbsReadyToCombin[1] = null;
        herbsReadyToCombin[2] = null;
        craftSize = GetCraftSize(GameManager.Instance.GetCurrentDay());
        ImageUpDate(); 
        EventManager.CallMedicineInventoryUPdate();
    }
    public void ComplitCombination()
    {
        int count = 0;
        int medicineOutWound=0;
        int medicineInternalWound=0;
        int mindWound = 0;
        for (int i = 0; i < 3; i++)
        {
            if (herbsReadyToCombin[i] != null)
            {
                count++;

                medicineOutWound += herbsReadyToCombin[i].getOutsideWound;
                medicineInternalWound += herbsReadyToCombin[i].getInternalWound;
                mindWound += herbsReadyToCombin[i].getMindWound;
            }

        }
        if (count > 0)
        {
            // 使用 Mathf.RoundToInt 进行四舍五入
            medicineInternalWound = Mathf.RoundToInt((float)medicineInternalWound / count);
            medicineOutWound = Mathf.RoundToInt((float)medicineOutWound / count);
            mindWound = Mathf.RoundToInt((float)mindWound / count);
        }
        Medicine medicine = new Medicine(); 
        medicine.ChangeInternalWound(medicineInternalWound);
        medicine.ChangeOutsideWound(medicineOutWound);
        medicine.ChangeMindWound(mindWound);
        medicine.ChangeSprite(medicineExamples.getMedicinesList[count].getMedicineSprite);//从1开始后是药品样例
        medicine.ChangeName(medicineExamples.getMedicinesList[count].getMedicineName);
        medicine.ChangeDetail();
        if(count > 0)
        {
            inventoryManager.AddComebineMedicine(medicine);
        }
        CleanCraft();
        EventManager.CallMedicineInventoryUPdate();
        ImageUpDate();

    }//合成函数
    public void ImageUpDate()
    {
        for (int i = 0; i < 3; i++) 
        {
            if(herbsReadyToCombin[i] != null && herbsReadyToCombin[i].getHerbSprite != null)
                images[i].sprite = herbsReadyToCombin[i].getHerbSprite;
            else
                images[i].sprite = null;
        }
    }//点击按钮传入图标
}
