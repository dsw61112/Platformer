using UnityEngine;

public class HeallthPack : MonoBehaviour
{
    public float health = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        other.GetComponent<HealthComponent>().AddHealth(health);
        Destroy(gameObject);
    }
}