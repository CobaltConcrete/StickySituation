using UnityEngine;

public class SpikeEnemy : MonoBehaviour
{
    [SerializeField] int damage = 1;

    void OnCollisionEnter2D(Collision2D col)
    {
        HumanPlayerController player = col.gameObject.GetComponent<HumanPlayerController>();
        if (player != null)
            player.TakeDamage(damage);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        HumanPlayerController player = col.GetComponent<HumanPlayerController>();
        if (player != null)
            player.TakeDamage(damage);
    }
}