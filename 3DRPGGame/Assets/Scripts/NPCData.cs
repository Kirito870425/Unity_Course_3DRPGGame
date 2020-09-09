using UnityEngine;

//列舉:下拉式選單
public enum StateNPC
{
    NoMission, missioning, finish
}

//CreateAssetMenu:建立菜單("檔案名稱"[可改], "菜單"名稱)
[CreateAssetMenu(fileName = "NPCData", menuName = "Roy/NPC_Data")]

public class NPCData : ScriptableObject //ScriptableObject:腳本化物件(將資料儲存於專案)
{
    [Header("NPC狀態")]
    public StateNPC state;
    [Header("打字速度"), Range(0, 5)]
    public float speed;
    [Header("打字音效")]
    public AudioClip sound;
    [Header("任務需求數量"), Range(0, 5)]
    public int count;

    [Header("對話"), TextArea(3, 10)]
    public string[] dialogs = new string[3];
}
