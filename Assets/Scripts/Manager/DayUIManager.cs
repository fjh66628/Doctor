using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DayUIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI dayText;

    private void OnEnable()
    {
        EventManager.UpdateDayEvent += OnUpdateDay;
    }

    private void OnDisable()
    {
        EventManager.UpdateDayEvent -= OnUpdateDay;
    }

    private void Start()
    {
        UpdateDayUI();
    }

    private void OnUpdateDay()
    {
        UpdateDayUI();
    }

    private void UpdateDayUI()
    {
        if (dayText == null) return;

        if (GameManager.Instance != null)
        {
            int day = GameManager.Instance.GetCurrentDay();
            int total = GameManager.Instance.totalDays;
            dayText.text = $"day {day}/6";
        }
        else
        {
            dayText.text = "第1天 1/6";
        }
    }
}
