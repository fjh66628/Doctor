using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class DialogueManager : MonoBehaviour
{
    [SerializeField] DialogueSQ appriciate;
    [SerializeField] DialogueSQ rejected;
    [SerializeField] Image dialogue;  // 对话框背景
    [SerializeField] TextMeshProUGUI text;  // 文本组件
    [SerializeField] float time = 2f;  // 显示时间，默认为2秒
    
    private Coroutine currentDialogueCoroutine;  // 当前对话协程的引用
    
    void OnEnable()
    {
        EventManager.CureSuccessfullyEvent += Appreciate;
        EventManager.FailToCureEvent += Rejected;
    }
    
    void OnDisable()
    {
        EventManager.CureSuccessfullyEvent -= Appreciate;
        EventManager.FailToCureEvent -= Rejected;
        
        // 清理协程
        if (currentDialogueCoroutine != null)
        {
            StopCoroutine(currentDialogueCoroutine);
            currentDialogueCoroutine = null;
        }
    }
    
    void Appreciate()
    {
        ShowDialogue(appriciate);
    }
    
    void Rejected()
    {
        ShowDialogue(rejected);
    }
    
    // 显示对话框的主要方法
    private void ShowDialogue(DialogueSQ dialogueData)
    {
        // 如果已经有对话在显示，先停止它
        if (currentDialogueCoroutine != null)
        {
            StopCoroutine(currentDialogueCoroutine);
        }
        
        // 从列表中随机选择一个文本（或者按顺序选择）
        if (dialogueData != null && dialogueData.getDialogue != null && dialogueData.getDialogue.Count > 0)
        {
            // 随机选择一条文本
            int randomIndex = Random.Range(0, dialogueData.getDialogue.Count);
            string selectedText = dialogueData.getDialogue[randomIndex];
            
            // 启动协程显示对话框
            currentDialogueCoroutine = StartCoroutine(ShowAndHideDialogueCoroutine(selectedText));
        }
        else
        {
            Debug.LogWarning("对话数据为空或没有可用的对话文本！");
        }
    }
    
    // 协程：显示对话框并在指定时间后关闭
    IEnumerator ShowAndHideDialogueCoroutine(string displayText)
    {
        // 1. 显示对话框和文本
        if (dialogue != null)
        {
            dialogue.gameObject.SetActive(true);
        }
        
        if (text != null)
        {
            text.text = displayText;
        }
        
        // 2. 等待指定时间
        yield return new WaitForSeconds(time);
        
        // 3. 隐藏对话框
        if (dialogue != null)
        {
            dialogue.gameObject.SetActive(false);
        }
        
        // 清空文本
        if (text != null)
        {
            text.text = "";
        }
        
        currentDialogueCoroutine = null;
    }

    

    
    // 协程：显示自定义文本并在指定时间后关闭
    IEnumerator ShowAndHideDialogueCoroutine(string displayText, float displayTime)
    {
        if (dialogue != null)
        {
            dialogue.gameObject.SetActive(true);
        }
        
        if (text != null)
        {
            text.text = displayText;
        }
        
        yield return new WaitForSeconds(displayTime);
        
        if (dialogue != null)
        {
            dialogue.gameObject.SetActive(false);
        }
        
        if (text != null)
        {
            text.text = "";
        }
        
        currentDialogueCoroutine = null;
    }
}