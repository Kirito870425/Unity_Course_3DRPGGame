using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    //載入畫面 進度 八條 提示文字
    public GameObject panel;
    public Text textLoading;
    public Text textTip;
    public Image imageLoading;

    public void Quit()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        StartCoroutine(Loading());
    }
    private IEnumerator Loading()
    {
        panel.SetActive(true);
        AsyncOperation ao = SceneManager.LoadSceneAsync("遊戲場景");    //非同步載入資訊 = 非同步載入
        ao.allowSceneActivation = false;                                //不允許自動載入


        while (!ao.isDone)
        {
            //progress 載入場景的進度值為0~1 如果設定allow為false會卡在0.9 SO /0.9 = 1
            textLoading.text = "載入進度:" + (ao.progress / 0.9f * 100).ToString("F2") + "%";
            imageLoading.fillAmount = ao.progress / 0.9f;
            yield return null;//一影格執行
        }
    }
}
