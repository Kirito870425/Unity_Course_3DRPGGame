using UnityEngine;
using System.Collections;   //使用協程必須引用此API

public class Looptest : MonoBehaviour
{
    public string Corotine;
    
    private int MethodB()
    {
        return 5;
    }

    /*協程方法
    1.傳回類型:IEnumerator
    2.null 一個影格的時間

    */
    private IEnumerator Test()
    {
        print("我是協程第一行");
        yield return new WaitForSeconds(3); //協程方法1
        print("我是兩秒後的城市");
    }

    private void Start()
    {
        StartCoroutine(Test());//協程專用呼叫StartCoroutine
        StartCoroutine(Big());
    }

    public Transform cube;

    private IEnumerator Big()////每0.2秒三圍向量全部+1
    {
        for (int i = 0; i < 10; i++)
        {
            cube.localScale += Vector3.one;
            yield return new WaitForSeconds(0.2f);
        }
    }
}
