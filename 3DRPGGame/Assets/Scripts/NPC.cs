using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NPC : MonoBehaviour
{
    public NPCData data;
    public GameObject panel;
    public Text textName;                                       //名稱
    public Text textContent;                                    //內容
    private AudioSource aud;

    /// <summary>
    /// 打字效果
    /// </summary>
    /// <returns></returns>
    private IEnumerator Type()
    {
        textContent.text = "";                                  //清空

        string dialog = data.dialogs[0];                        //取得內容

        for (int i = 0; i < dialog.Length; i++)                 //執行對話
        {
            textContent.text += dialog[i];                      //遞增
            aud.PlayOneShot(data.sound, 0.5f);
            yield return new WaitForSeconds(data.speed);        //協程等待
        }
    }

    private void Awake()
    {
        aud = GetComponent<AudioSource>();
    }

    private void NoMission()
    {

    }

    private void Missioning()
    {

    }

    private void Finish()
    {

    }
    #region 對話內容

    private void DialogStart()
    {
        panel.SetActive(true);
        textName.text = name;                                   //更新名稱
        StartCoroutine(Type());                                 //啟動協程:打字效果
    }

    private void DialogStop()
    {
        panel.SetActive(false);
    }

    #endregion
    /// <summary>
    /// 面向玩家
    /// </summary>
    /// <param name="other">玩家</param>
    private void LookAtPlayer(Collider other)
    {
        Quaternion angle = Quaternion.LookRotation(other.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, angle, Time.deltaTime * 5);
    }

    #region 進出入觸發區域

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "快樂瘋男") DialogStart();
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.name == "快樂瘋男") DialogStop();
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.name == "快樂瘋男")
            LookAtPlayer(other);
    }

    #endregion
}
