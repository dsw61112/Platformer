using System.Collections;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class Teleporter : MonoBehaviour
{
    public Transform position;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.transform.position = position.position;
    }
}
