using UnityEngine;

public class CameraControl : MonoBehaviour
{
    #region 欄位

    [Header("目標物件")]
    public Transform target;
    [Header("追蹤速度"), Range(0, 500)]
    public float speed;
    [Header("旋轉速度"), Range(0, 1000)]
    public float turn;
    [Header("上下角度限制")]
    public Vector2 limit = new Vector2(45, -30);

    private Quaternion rot;

    #endregion

    #region 方法

    public void Track()
    {
        Vector3 posA = transform.position;
        Vector3 posB = target.position;

        transform.position = Vector3.Lerp(posA, posB, Time.deltaTime * speed);

        rot.x -= Input.GetAxis("Mouse Y") * turn * Time.deltaTime;  //上下角度
        rot.y += Input.GetAxis("Mouse X") * turn * Time.deltaTime;  //左右

        rot.x = Mathf.Clamp(rot.x, limit.y, limit.x);               //夾住X在限制內
        //四元轉歐拉角度(0-360)
        transform.rotation = Quaternion.Euler(rot.x, rot.y + 180, rot.z);
    }

    #endregion

    #region 事件
    private void Awake()
    {
        Cursor.visible = false; //指標.可視性
    }

    private void LateUpdate()
    {
        Track();
    }

    #endregion
}
