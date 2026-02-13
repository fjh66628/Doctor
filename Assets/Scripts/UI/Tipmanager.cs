using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // 添加UI命名空间

[System.Serializable]
public class Tipmanager : MonoBehaviour
{
    [SerializeField] Image image; // UnityEngine.UI.Image
    bool isClicked = false;
    
    public void Clicked()
    {
        // 切换image的激活状态
        if (image != null)
        {
            // 如果当前已激活，则卸载/隐藏
            if (image.gameObject.activeSelf)
            {
                image.gameObject.SetActive(false);
            }
            // 如果未激活，则激活/显示
            else
            {
                image.gameObject.SetActive(true);
            }
        }
    }

}