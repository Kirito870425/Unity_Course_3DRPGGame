using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    #region 欄位
    [Header("移動速度"), Range(0, 100)]
    public float speed;
    [Header("攻擊力"), Range(0, 100)]
    public float attack;
    [Header("Sland血量"), Range(0, 1000)]
    public float hp;
    [Header("掉落道具的機率"), Range(0f, 1f)]
    public float prop;
    [Header("掉落的道具")]
    public Transform skull;
    [Header("被獲得經驗值"), Range(0, 100)]
    public float exp;
    [Header("停止距離:攻擊距離"), Range(0, 10)]
    public float rangeAttack = 1.5f;
    [Header("攻擊冷卻時間"), Range(0, 10)]
    public float cd = 3.5f;
    [Header("面向玩家的速度"), Range(0, 100)]
    public float trun = 10f;

    private float timer;
    private NavMeshAgent nma;
    private Animator ani;
    private Player player;
    private Rigidbody rigi;

    #endregion

    #region 方法

    private void Move()
    {
        //代理器.設定目的地(玩家:快樂瘋男)
        nma.SetDestination(player.transform.position);
        //三為向量轉float(加速度.長度)
        ani.SetFloat("SlendMove", nma.velocity.magnitude);

        if (nma.remainingDistance <= rangeAttack)   //抵達設定的攻擊距離
        {
            Attack();
        }
    }

    private void Attack()
    {
        Quaternion look = Quaternion.LookRotation(player.transform.position - transform.position);  //B角度  = 四元.面向角度(豐南-Slender)
        transform.rotation = Quaternion.Slerp(transform.rotation, look, Time.deltaTime * trun);     //轉角度 = 四元.差值(A ,B, 百分比)

        timer += Time.deltaTime;
        if (timer >= cd)
        {
            timer = 0;
            ani.SetTrigger("SlendAttack");
        }
    }
    public void Hit(float damage, Transform direction)
    {
        hp -= damage;
        rigi.AddForce(direction.forward * 50 + direction.up * 100);//受傷效果，往後往上

        ani.SetTrigger("SlendHit");

        if (hp <= 0) Dead();
    }
    public void Dead()
    {
        //this.enabled = false; //第一種
        enabled = false;        //第二種
        GetComponent<Collider>().enabled = false;
        ani.SetBool("SlendDead", true);

        DropProp();
        player.Exp(exp);
    }
    private void DropProp()
    {
        float r = Random.Range(0f, 1f);//給予機率
        if (r <= prop)
        {
            Instantiate(skull, transform.position + transform.up * 1.5f, transform.rotation);
        }
    }

    #endregion

    #region 事件

    private void Awake()
    {
        nma = GetComponent<NavMeshAgent>();
        ani = GetComponent<Animator>();
        rigi = GetComponent<Rigidbody>();
        player = FindObjectOfType<Player>();    //FOOT只能抓同一個類型

        nma.speed = speed;                      //將nma的速度改成我們的速度
        nma.stoppingDistance = rangeAttack;
    }

    private void Update()
    {
        Move();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 0, 0, 0.5f);
        Gizmos.DrawSphere(transform.position, rangeAttack);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "快樂瘋男")
        {
            other.GetComponent<Player>().Hit(attack, transform);
        }
    }
    /// <summary>
    /// 有勾選collision send collisio messages 的粒子碰到後會執行一次
    /// </summary>
    /// <param name="other"></param>
    private void OnParticleCollision(GameObject other)
    {
        if (other.name == "碎石群效果")
        {
            float damage = player.damageRock;
            Hit(damage, player.transform);
        } 
    }
    #endregion
}
