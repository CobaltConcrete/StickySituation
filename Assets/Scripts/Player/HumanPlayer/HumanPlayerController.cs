using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanPlayerController : MonoBehaviour
{
    [Header("Movement")]
	[SerializeField] public float movespeed;
    [SerializeField] public float maxspeed;
    [SerializeField] public float jumpforce;

    Rigidbody2D playerRB;
    private SpriteRenderer sr;

    int groundContactCount = 0;
    public bool feetContact => groundContactCount > 0;

    [Header("Health")]
    [SerializeField] public int maxHP = 5;
    int currentHP;

    [Header("Connected Objects")]

	[SerializeField]
    [Tooltip("Slime GameObject")]
    public GameObject slime;

	[SerializeField]
    [Tooltip("Rope GameObject")]
    public GameObject rope;

	[SerializeField]
    [Tooltip("HUDController")]
    public HUDController hud;

    [Header("Shooting")]

	[SerializeField]
    [Tooltip("Arrow prefab")]
    public GameObject arrowPrefab;

	[SerializeField]
    [Tooltip("Arrow Spawn Point")]
    public Transform arrowSpawnPoint;

	[SerializeField]
    [Tooltip("Minimum seconds between shots")]
    public float shootCooldown = 0.3f;

    [SerializeField]
    [Tooltip("Animator")]
    public Animator animator;


    float facingDirection = 1f; // +1 = right, -1 = left
    float shootTimer = 0f;

    void Awake()
    {
        playerRB = GetComponent<Rigidbody2D>();
        currentHP = maxHP;
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

		hud.UpdateHP(1f);

    }

    void Update()
    {
        float MoveHor = 0;
        if (Input.GetKey(KeyCode.A))
        {
            animator.SetFloat("Speed", 1);
            MoveHor = -1;
            sr.flipX = true;
        }
            
        else if (Input.GetKey(KeyCode.D))
        {
            animator.SetFloat("Speed", 1);
            MoveHor = 1;
            sr.flipX = false;
        } 
        else
        {
            animator.SetFloat("Speed", 0);
        }

        // Track which way player facing for arrow direction
        if (MoveHor != 0)
            facingDirection = Mathf.Sign(MoveHor);

        Vector2 movement = new Vector2(MoveHor * movespeed, 0) * Time.deltaTime;
        playerRB.AddForce(movement);

        if (playerRB.linearVelocity.x > maxspeed)
            playerRB.linearVelocity = new Vector2(maxspeed, playerRB.linearVelocity.y);
        if (playerRB.linearVelocity.x < -maxspeed)
            playerRB.linearVelocity = new Vector2(-maxspeed, playerRB.linearVelocity.y);

        if (Input.GetKeyDown(KeyCode.W) && canJump())
        {
            playerRB.linearVelocity = new Vector2(playerRB.linearVelocity.x, 0);
            playerRB.AddForce(Vector2.up * jumpforce);
            animator.SetTrigger("Jump");
        }

        // Tick cooldown every frame
        if (shootTimer > 0f)
            shootTimer -= Time.deltaTime;

        if (Input.GetKey(KeyCode.Space) && shootTimer <= 0f)
        {
            Shoot();
            shootTimer = shootCooldown;
        }
        else
        {
            
        }
    }

    void Shoot()
    {
        if (arrowPrefab == null) return;

        Vector3 spawnPos =  arrowSpawnPoint.position;

        GameObject arrowGO = Instantiate(arrowPrefab, spawnPos, Quaternion.identity);

        // arrow dont trigger collide human
        Collider2D arrowCollider = arrowGO.GetComponent<Collider2D>();
        Collider2D humanCollider = GetComponent<Collider2D>();
        if (arrowCollider != null && humanCollider != null)
            Physics2D.IgnoreCollision(arrowCollider, humanCollider);

        Arrow arrow = arrowGO.GetComponent<Arrow>();
        if (arrow != null)
            arrow.direction = facingDirection;

        animator.SetTrigger("Shoot");
    }

    bool canJump()
    {
        return feetContact;
    }

    public void OnGroundContactEnter()
    {
        groundContactCount++;
    }

    public void OnGroundContactExit()
    {
        groundContactCount = Mathf.Max(0, groundContactCount - 1);
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        currentHP = Mathf.Max(currentHP, 0);

        float percent = (float)currentHP / maxHP;

        if (hud != null)
            hud.UpdateHP(percent);

        if (currentHP <= 0)
            Die();
    }

    void Die()
    {
        if (slime != null) Destroy(slime);
        if (rope != null) Destroy(rope);

        if (GameManager.Instance != null)
            GameManager.Instance.EndRun();

        Destroy(gameObject);
    }
}