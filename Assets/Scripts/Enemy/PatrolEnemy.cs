using UnityEngine;

public class PatrolEnemy : MonoBehaviour
{
    [Header("Patrol")]
    [Tooltip("How far left and right the enemy walks from its starting position")]
    public float patrolRange = 4f;

    [Tooltip("How fast the enemy moves")]
    public float moveSpeed = 2f;

    [Header("Health")]
    public int maxHP = 3;

    [Header("Player Damage")]
    [Tooltip("How much damage this enemy deals to the Human")]
    public int damage = 1;

    int currentHP;
    Vector2 startPos;
    float direction = 1f; // +1 = right, -1 = left

    void Awake()
    {
        currentHP = maxHP;
        startPos = transform.position;
    }

    void Update()
    {
        // walk horizontally
        transform.Translate(Vector2.right * direction * moveSpeed * Time.deltaTime);

        // turn around
        float distFromStart = transform.position.x - startPos.x;
        if (distFromStart >= patrolRange)
            direction = -1f;
        else if (distFromStart <= -patrolRange)
            direction = 1f;
    }

    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        if (currentHP <= 0)
            Die();
    }

    void Die()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.AddScore(1);

        Destroy(gameObject);
    }

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