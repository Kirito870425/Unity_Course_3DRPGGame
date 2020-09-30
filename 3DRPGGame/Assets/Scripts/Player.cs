using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : MonoBehaviour
{
    #region 欄位
    [Header("移動速度"), Range(0, 1000)]
    public float m_speed = 5;
    [Header("旋轉速度"), Range(0, 1000)]
    public float m_turn = 5;
    [Header("攻擊力"), Range(0, 1000)]
    public float m_attack = 20;
    [Header("血量"), Range(0, 1000)]
    public float m_hp = 250;
    [Header("魔力"), Range(0, 1000)]
    public float m_mp = 250;

    public int lv = 1;
    public float exp = 0;
    public Image barexp;
    public Text lvtext;

    public Text textmission;
    public int count;
    public GameObject rock;
    public Transform pointRock;
    public float costRock = 20;
    public float damageRock = 100;

    [Header("音效區")]
    public AudioClip soundprop;

    [Header("吧條")]
    public Image barhp;
    public Image barmp;

    private Animator ani;
    private Rigidbody rigi;
    private NPC npc;
    [HideInInspector]       //在屬性面板上隱藏
    public bool stop;
    private AudioSource aud;
    private float maxhp, maxmp, maxexp = 100;
    /// <summary>攝影機根物件</summary>
    private Transform cam;
    private float[] exps;

    #endregion

    #region 方法

    public void Move()
    {
        //自動判斷A左D右
        float h = Input.GetAxis("Horizontal");
        //上下
        float v = Input.GetAxis("Vertical");

        //Vector3 pos = new Vector3(h, 0, v);                       //世界座標
        //Vector3 pos = transform.forward * v + transform.right * h;  //區域作標
        Vector3 pos = cam.forward * v + cam.right * h;  //攝影機的區域作標
        rigi.MovePosition(transform.position + pos * m_speed * Time.fixedDeltaTime);
        //Abs絕對值
        ani.SetFloat("PlayerMove", Mathf.Abs(v) + Mathf.Abs(h));
        //如果有控制，再轉向:避免沒移動時轉回原點
        if (h != 0 || v != 0)
        {
            pos.y = 0;                                                                                  //鎖定Y軸，避免吃土
            Quaternion angle = Quaternion.LookRotation(pos);                                            //將前往的座標轉為角度
            transform.rotation = Quaternion.Slerp(transform.rotation, angle, m_turn * Time.deltaTime);  //角度差值
        }
    }
    public void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            ani.SetTrigger("PlayerAttack");
        }
    }
    /// <summary>
    /// 玩家受傷
    /// </summary>
    /// <param name="damage">傷害直</param>
    /// <param name="direction">擊退方向</param>
    public void Hit(float damage, Transform direction)
    {
        m_hp -= damage;
        rigi.AddForce(direction.forward * 100 + direction.up * 150);//受傷效果，往後往上

        barhp.fillAmount = m_hp / maxhp;
        ani.SetTrigger("PlayerHit");

        if (m_hp <= 0) Dead();
    }
    /// <summary>
    /// 玩家死亡
    /// </summary>
    public void Dead()
    {
        //this.enabled = false; //第一種
        enabled = false;        //第二種
        ani.SetBool("PlayerDead", true);

        StartCoroutine(ShowFinal());
    }
    [Header("遊戲結束畫面")]
    public CanvasGroup final;
    public Text textFinal;
    /// <summary>
    /// 顯示結束畫面
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShowFinal()
    {
        yield return new WaitForSeconds(0.5f);  //等0.5S

        textFinal.text = "新細明體 你死了";

        while (final.alpha < 1)
        {
            final.alpha += 0.5f * Time.deltaTime;
            yield return null;
        }
        Cursor.visible = true;                  //顯示滑鼠
        final.interactable = true;              //結束才可互動
        final.blocksRaycasts = true;            //開啟滑鼠阻擋
    }
    /// <summary>
    /// 取得道具
    /// </summary>
    public void GetProp()
    {
        count++;
        textmission.text = "打敗Sland" + count + "/" + npc.data.count;

        if (count == npc.data.count)
        {
            npc.Finish();
        }
    }
    /// <summary>
    /// 經驗值
    /// </summary>
    /// <param name="expGet">取得的經驗值</param>
    public void Exp(float expGet)
    {
        exp += expGet;
        barexp.fillAmount = exp / maxexp;

        while (exp >= maxexp) LevelUp();
    }
    /// <summary>
    /// 升級
    /// </summary>
    private void LevelUp()
    {
        lv++;
        lvtext.text = "Lv" + lv;

        //升級後最大數值提升
        maxhp += 20;
        maxmp += 5;
        m_attack += 10;
        //升級後全滿
        m_hp = maxhp;
        m_mp = maxmp;

        barhp.fillAmount = 1;
        barmp.fillAmount = 1;

        //經驗值合理化
        exp -= maxexp;
        maxexp = exps[lv - 1];  //LV1抓第0筆資料，以此類推，等合理化後再進行經驗值調整
        barexp.fillAmount = exp / maxexp;
    }
    /// <summary>
    /// 流星雨
    /// </summary>
    private void SkillRock()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1) && m_mp >= costRock)
        {
            ani.SetTrigger("Playerboll");
            Instantiate(rock, pointRock.position, pointRock.rotation);
            m_mp -= costRock;
            barmp.fillAmount = m_mp / maxmp;
        }
    }
    private void RestoreMp()
    {
        float restoremp = 5;
        m_mp += restoremp * Time.deltaTime;
        m_mp = Mathf.Clamp(m_mp, 0, maxmp);
        barmp.fillAmount = m_mp / maxmp;
    }
    [Header("回血量/每秒")]
    public float restoreHp = 5;
    [Header("回魔量/每秒")]
    public float restoreMp = 10;

    /// <summary>
    /// 恢復系統
    /// </summary>
    /// <param name="value">恢復值</param>
    /// <param name="restore">每秒恢復</param>
    /// <param name="max">最大值</param>
    /// <param name="bar">更新吧條</param>
    private void Restore(float value, float restore, float max, Image bar)
    {
        value += restore * Time.deltaTime;
        value = Mathf.Clamp(value, 0, maxmp);
        bar.fillAmount = value / max;
    }
    #endregion

    #region 事件

    private void Awake()
    {
        exps = new float[99];
        for (int i = 0; i < exps.Length; i++) exps[i] = maxexp * (i + 1);
        maxhp = m_hp;
        maxmp = m_mp;

        ani = GetComponent<Animator>();
        rigi = GetComponent<Rigidbody>();
        aud = GetComponent<AudioSource>();
        npc = FindObjectOfType<NPC>();

        cam = GameObject.Find("攝影機根物件").transform;  //建議不要在update使用 占用效能
    }

    private void FixedUpdate()//固定50FPS更新,推薦物理運動
    {
        if (stop) return;

        Move();
    }

    private void Update()
    {
        Attack();
        SkillRock();
        //回血魔
        Restore(m_hp, restoreHp, maxhp, barhp);
        Restore(m_mp, restoreMp, maxmp, barmp);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "道具_骷髏頭")
        {
            aud.PlayOneShot(soundprop);
            Destroy(collision.gameObject);
            GetProp();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "怪物")
        {
            other.GetComponent<Enemy>().Hit(m_attack, transform);
        }
    }

    #endregion

}
