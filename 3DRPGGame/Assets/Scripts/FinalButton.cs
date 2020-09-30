using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalButton : MonoBehaviour
{
    public void Exit()
    {
        Application.Quit();
    }
    /// <summary>
    /// 延遲音效播放時間*3
    /// </summary>
    public void Replay()
    {
        Invoke("ReGame", 0.7f);
    }
    public void ReGame()
    {
        //SceneManager.LoadScene("遊戲場景");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void BlockMeun()
    {
        SceneManager.LoadScene("選單");
        //SceneManager.LoadScene(0);
    }
}
