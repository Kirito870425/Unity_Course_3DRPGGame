using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private GameObject[] points;
    public Transform enemy;
    [Header("生成間隔時間"), Range(0f, 5f)]
    public float interval = 2f;

    private void Awake()
    {
        points = GameObject.FindGameObjectsWithTag("生成點");  //注意SSSSSSSSSSSS 尋找所有標籤
        //重複延遲呼叫方式(函式名稱 延遲時間 間隔時間
        InvokeRepeating("Spawn", 0, interval);
    }
    private void Spawn()
    {
        int r = Random.Range(0, points.Length);
        
        Instantiate(enemy, points[r].transform.position, points[r].transform.rotation);
    }
}
