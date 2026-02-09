using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    [Header("按钮引用")]
    public Button startButton;
    
    [Header("场景设置")]
    public string targetScene = "GameScene";
    
    /// <summary>
    /// 按钮点击事件
    /// </summary>
    public void OnClick()
    {
        
        // 使用单场景模式加载
        StartCoroutine(LoadSingleScene());
    }
    
    /// <summary>
    /// 单场景模式加载（直接替换当前场景）
    /// </summary>
    private IEnumerator LoadSingleScene()
    {
        // 创建异步加载操作
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(targetScene);
        asyncLoad.allowSceneActivation = true;
        

            // 不显示进度，直接等待加载完成
            yield return asyncLoad;
    }
    
    /// <summary>
    /// 检查场景是否存在
    /// </summary>
    private bool SceneExists(string sceneName)
    {
        // 检查Build Settings中的所有场景
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string nameFromPath = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            
            if (nameFromPath == sceneName)
            {
                return true;
            }
        }
        return false;
    }
    
    /// <summary>
    /// 在Inspector中测试场景切换
    /// </summary>
    [ContextMenu("测试加载场景")]
    private void TestSceneLoad()
    {
        if (Application.isPlaying)
        {
            OnClick();
        }
        else
        {
            Debug.Log("请在运行模式下测试");
        }
    }
    

}