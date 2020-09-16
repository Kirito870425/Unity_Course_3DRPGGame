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
    [Header("被獲得經驗值"), Range(0, 100)]
    public float exp;
    [Header("掉落的道具")]
    public Transform skull;
    [Header("停止距離:攻擊距離"), Range(0, 10)]
    public float rangeAttack = 1.5f;
    [Header("攻擊冷卻時間"), Range(0, 10)]
    public float cd = 3.5f;

    private float timer;
    private NavMeshAgent nma;
    private Animator ani;
    private Player player;

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
        timer += Time.deltaTime;
        if (timer >= cd)
        {
            timer = 0;
            ani.SetTrigger("SlendAttack");
        }
    }

    #endregion

    #region 事件

    private void Awake()
    {
        nma = GetComponent<NavMeshAgent>();
        ani = GetComponent<Animator>();
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
        if (other.name =="快樂瘋男")
        {
            other.GetComponent<Player>().Hit(attack, transform);
        }
    }
    #endregion
}
