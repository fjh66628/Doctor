using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayButtonManager : MonoBehaviour
{
    // 点击按钮后推进到下一天
    public void OnNextDayButtonClicked()
    {
        if (GameManager.Instance == null) return;

        GameManager.Instance.GoToNextDay();
        
    }
}
