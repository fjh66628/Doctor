using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

[System.Serializable]
public class UIManager : MonoBehaviour
{
    [SerializeField] Image tipBackground;
    [SerializeField] TextMeshProUGUI tip;
    [SerializeField] float tipDuration = 2f; // 提示显示时长
    private float tipTimer = 0f;
    private bool isShowingTip = false;
    
    public void ShowTip(string message)
    {
        tipBackground.gameObject.SetActive(true);
        tip.text = message;
        tipTimer = tipDuration;
        isShowingTip = true;
    }
    
    public void CloseTip()
    {
        tipBackground.gameObject.SetActive(false);
        isShowingTip = false;
    }
    
    private void Update()
    {
        if (isShowingTip)
        {
            tipTimer -= Time.deltaTime;
            
            if (tipTimer <= 0f)
            {
                CloseTip();
            }
        }
    }
}