using UnityEngine;

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

    public float m_exp = 0;
    public int m_lv = 1;

    private Animator ani;
    private Rigidbody rigi;

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
        //ABS絕對值
        ani.SetFloat("PlayerMove", Mathf.Abs(v) + Mathf.Abs(h));
    }
    public void Attack()
    {

    }
    public void Hit()
    {

    }
    public void Dead()
    {

    }
    public void GetProp()
    {

    }

    #endregion
    /// <summary>攝影機根物件</summary>
    private Transform cam;

    #region 事件

    private void Awake()
    {
        ani = GetComponent<Animator>();
        rigi = GetComponent<Rigidbody>();

        cam = GameObject.Find("攝影機根物件").transform;  //建議不要在update使用 占用效能
    }

    private void FixedUpdate()//固定50FPS更新,推薦物理運動
    {
        Move();
    }

    #endregion

}
