using UnityEngine;

public class Arrow : MonoBehaviour
{
    [Tooltip("How much damage the arrow deals to enemies")]
    public int damage = 1;

    [Tooltip("How fast the arrow travels")]
    public float speed = 15f;

    [Tooltip("How many seconds before the arrow destroys itself if it hits nothing")]
    public float lifetime = 3f;

    // +1 = flying right, -1 = flying left. Set by HumanPlayerController on spawn.
    [HideInInspector]
    public float direction = 1f;

    [Tooltip("Arrows are destroyed when hitting objects on these layers (e.g. Platform)")]
    public LayerMask destroyOnLayers;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(Vector2.right * direction * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        PatrolEnemy enemy = other.GetComponentInParent<PatrolEnemy>();
        if (enemy != null)
        {
            Debug.Log("Arrow Hit enemy, dealing " + damage + " damage.");
            enemy.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        if (((1 << other.gameObject.layer) & destroyOnLayers) != 0)
        {
            Destroy(gameObject);
        }
    }
}