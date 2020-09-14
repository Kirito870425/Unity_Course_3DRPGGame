using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NPC : MonoBehaviour
{
    public NPCData data;
    public GameObject panel;
    public Text textName;                                       //名稱
    public Text textContent;                                    //內容

    public RectTransform rectmisstion;

    public GameObject objectshow;

    private Animator ani;
    private AudioSource aud;
    private Player player;

    #region 協程

    /// <summary>
    /// 打字效果，動畫效果
    /// </summary>
    /// <returns></returns>
    private IEnumerator Type()
    {
        PlayAnimation();

        player.stop = true;

        textContent.text = "";                                  //清空

        string dialog = data.dialogs[(int)data.state];          //取得內容***取得列舉的整數方式

        for (int i = 0; i < dialog.Length; i++)                 //執行對話
        {
            textContent.text += dialog[i];                      //遞增
            aud.PlayOneShot(data.sound, 0.5f);
            yield return new WaitForSeconds(data.speed);        //協程等待
        }
        player.stop = false;

        NoMission();
    }
    /// <summary>
    /// 顯示任務欄
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShowMission()
    {
        while (rectmisstion.anchoredPosition.x > 0)
        {
            rectmisstion.anchoredPosition -= new Vector2(500 * Time.deltaTime, 0);
            yield return null;//等待
        }
    }

    #endregion
    /// <summary>
    /// 對話動畫
    /// </summary>
    private void PlayAnimation()
    {
        if (data.state != StateNPC.finish)
        {
            ani.SetBool("對話開關", true);
        }
        else
        {
            ani.SetTrigger("完成任務");
        }
    }

    private void NoMission()
    {
        if (data.state != StateNPC.NoMission) return;

        data.state = StateNPC.missioning;
        objectshow.SetActive(true);

        StartCoroutine(ShowMission());
    }

    private void Missioning()
    {

    }

    public void Finish()
    {
        data.state = StateNPC.finish;
        ani.SetBool("對話開關", false);
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
    private void Awake()
    {
        aud = GetComponent<AudioSource>();
        ani = GetComponent<Animator>();
        player = FindObjectOfType<Player>();

        data.state = StateNPC.NoMission;
    }
}
