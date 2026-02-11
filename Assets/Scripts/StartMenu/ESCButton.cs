using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ExitGame : MonoBehaviour
{
    [Header("UI Elements")]
    public Image exitImage;
    public TextMeshProUGUI exitText;

    void Start()
    {
        // 确保UI元素不为空
        if (exitImage != null)
        {
            // 为Image添加点击事件监听
            Button imageButton = exitImage.GetComponent<Button>();
            if (imageButton == null)
            {
                imageButton = exitImage.gameObject.AddComponent<Button>();
            }
            imageButton.onClick.AddListener(QuitGame);
        }

        if (exitText != null)
        {
            // 为Text添加点击事件监听
            Button textButton = exitText.GetComponent<Button>();
            if (textButton == null)
            {
                textButton = exitText.gameObject.AddComponent<Button>();
            }
            textButton.onClick.AddListener(QuitGame);
        }
    }

    // 退出游戏的方法
    private void QuitGame()
    {
#if UNITY_EDITOR
        // 在编辑器中停止播放
        UnityEditor.EditorApplication.isPlaying = false;
#else
            // 在构建版本中退出应用
            Application.Quit();
#endif
    }
}