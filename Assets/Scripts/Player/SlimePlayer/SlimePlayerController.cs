using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimePlayerController : MonoBehaviour
{
	[Header("Movement")]
    [SerializeField] public float movespeed;
    [SerializeField] public float maxspeed;
    [SerializeField] public float jumpforce;

    [Header("Stamina")]
    [SerializeField] public float maxStamina = 5f;
    [SerializeField] public float staminaDrainRate = 1f;
    [SerializeField] public float staminaRegenRate = 1.5f;

	[SerializeField] 
    [Tooltip("Stamina must be at or above this value to begin or continue sticking")]
    public float staminaStickThreshold = 1f;

	[SerializeField]
    [Tooltip("HUDController")]
    public HUDController hud;

    [SerializeField]
    [Tooltip("Animator")]
    public Animator animator;

    int PlatformLayer;

    Rigidbody2D playerRB;
    private SpriteRenderer sr;


    public bool feetContact;

    int platformContactCount = 0; // use count so that as long as one contact => platformContactCount >= 1
    bool touchingPlatform => platformContactCount > 0;

    float originalGravity;
    RigidbodyConstraints2D originalConstraints;
    float currentStamina;
    bool isSticking = false;

    void Awake()
    {
        playerRB = GetComponent<Rigidbody2D>();
        PlatformLayer = LayerMask.NameToLayer("Platform");
        originalGravity = playerRB.gravityScale;
        originalConstraints = playerRB.constraints;
        currentStamina = maxStamina;

        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();


        if (hud != null)
            hud.UpdateMP(1f);
    }

    void Update()
    {
        HandleStick();
        HandleStamina();

        if (playerRB.constraints == RigidbodyConstraints2D.FreezeAll)
            return;

        float MoveHor = 0;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            // animator.SetFloat("Speed", 1);
            sr.flipX = true;
            MoveHor = -1;

        } 
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            // animator.SetFloat("Speed", 1);
            sr.flipX = false;
            MoveHor = 1;
        }
        else
        {
            animator.SetFloat("Speed", 0);
        }

        Vector2 movement = new Vector2(MoveHor * movespeed, 0) * Time.deltaTime;
        playerRB.AddForce(movement);

        if (playerRB.linearVelocity.x > maxspeed)
            playerRB.linearVelocity = new Vector2(maxspeed, playerRB.linearVelocity.y);
        if (playerRB.linearVelocity.x < -maxspeed)
            playerRB.linearVelocity = new Vector2(-maxspeed, playerRB.linearVelocity.y);

        if (Input.GetKeyDown(KeyCode.UpArrow) && canJump())
        {
            playerRB.linearVelocity = new Vector2(playerRB.linearVelocity.x, 0);
            playerRB.AddForce(Vector2.up * jumpforce);
            animator.SetTrigger("Jump");
        }
    }

    void HandleStick()
    {
        bool wantsToStick = Input.GetKey(KeyCode.DownArrow) && touchingPlatform;

        // Can only START sticking if stamina is above the threshold.
        // Once sticking, keep going until stamina hits 0 or the key is released.
        if (wantsToStick && !isSticking && currentStamina < staminaStickThreshold)
            wantsToStick = false;

        if (wantsToStick && currentStamina > 0f)
        {
            isSticking = true;
            playerRB.linearVelocity = Vector2.zero;
            playerRB.angularVelocity = 0f;
            playerRB.gravityScale = 0f;
            playerRB.constraints = RigidbodyConstraints2D.FreezeAll;

            currentStamina -= staminaDrainRate * Time.deltaTime;
            if (currentStamina < 0f)
                currentStamina = 0f;

            if (hud != null)
                hud.UpdateMP(currentStamina / maxStamina);
        }
        else
        {
            isSticking = false;
            if (playerRB.constraints == RigidbodyConstraints2D.FreezeAll)
            {
                playerRB.constraints = originalConstraints;
                playerRB.gravityScale = originalGravity;
            }
        }
    }

    void HandleStamina()
    {
        // Stamina only regens when the slime is not sticking
        if (!isSticking)
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
            if (currentStamina > maxStamina)
                currentStamina = maxStamina;

            if (hud != null)
                hud.UpdateMP(currentStamina / maxStamina);
        }
    }

    bool canJump()
    {
        return feetContact;
    }

    public void OnPlatformContactEnter()
    {
        platformContactCount++;
    }

    public void OnPlatformContactExit()
    {
        platformContactCount = Mathf.Max(0, platformContactCount - 1);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.layer == PlatformLayer)
            OnPlatformContactEnter();
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.layer == PlatformLayer)
            OnPlatformContactExit();
    }
}