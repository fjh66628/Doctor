using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class EventManager
{
    public static event Action<Herb> AddHerbEvent;
    public static event Action<Medicine> AddMedicineEvent;//仓库增加物品事件
    public static event Action MedicineInventoryUpdateEvent;//药品仓库物品更新事件
    public static event Action HerbInventoryUpdateEvent;//草药仓库物品更新事件
    public static event Action<int> SeleckedMainHerb;
    public static event Action<int> SeleckedAssistHerb;
    public static event Action<int> SeleckedMedicine;//添加选中主药，副药和药品
    public static event Action FailToCureEvent;//给药失败的事件
    public static event Action SkipThePatientEvent;//跳过病人事件
    public static event Action CureSuccessfullyEvent;//治好病人事件
    public static event Action UpdateDayEvent;//天数更新事件







    public static void CallAddHerbEvent(Herb herb)
    {
        AddHerbEvent?.Invoke(herb);
    }
    public static void CallAddMedicineEvent(Medicine medicine)
    {
        AddMedicineEvent?.Invoke(medicine);
    }//仓库增加物品事件激活函数
    public static void CallMedicineInventoryUPdate()
    {
        MedicineInventoryUpdateEvent?.Invoke();
    }//仓库物品更新的激活函数
    public static void CallHerbInventoryUpdate()
    {
        HerbInventoryUpdateEvent?.Invoke();
    }//仓库药草更新激活函数
    public static void CallSeleckedMainHerb(int index)
    {
        SeleckedMainHerb?.Invoke(index);
    }
    public static void CallSeleckedAssistHerb(int index)
    {
        SeleckedAssistHerb?.Invoke(index);
    }
    public static void CallSeleckedMedicine(int index)
    {
        SeleckedMedicine?.Invoke(index);
    }//添加对应的激活函数
    public static void CallFailToCure()
    {
        FailToCureEvent?.Invoke();
    }
    public static void CallSkipPatient()
    {
        SkipThePatientEvent?.Invoke();
    }
    public static void CallCureSuccess()
    {
        CureSuccessfullyEvent?.Invoke();
    }
    public static void CallUpdateDay()
    {
        UpdateDayEvent?.Invoke();
    }
}
