using System.Collections;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public Teleporter teleport;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        teleport.gameObject.SetActive(false);
        collision.transform.position = teleport.transform.position;
        StartCoroutine(TeleportPlayer(1.0f));
    }

    IEnumerator TeleportPlayer(float timeToTeleport)
    {
        yield return new WaitForSeconds(timeToTeleport);
        teleport.gameObject.SetActive(true);
    }
}
