using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Transform target;

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "快樂瘋男")
        {
            target.GetComponent<CapsuleCollider>().enabled = false;
            other.transform.position = target.position;
            Invoke("OpenCollider", 3f);
        }
    }
    private void OpenCollider()
    {
            target.GetComponent<CapsuleCollider>().enabled = true;
    }
}
