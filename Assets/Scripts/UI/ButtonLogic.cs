using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
[System.Serializable]
public class ButtonLogic : MonoBehaviour
{
    [SerializeField]TextMeshProUGUI text;//按钮的文本
    [SerializeField]Button targetButton;
    void Start()
    {
        text.gameObject.SetActive(false);
    }
    // 鼠标进入按钮
    public void OnPointerEnter()
    {
        text.gameObject.SetActive(true);
    }
    
    // 鼠标离开按钮
    public void OnPointerExit()
    {
        text.gameObject.SetActive(false);
    }
}
