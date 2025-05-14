using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Movement")]
    public float moveSpeed = 5f;
    public float runSpeed = 8f;
    public float jumpForce = 10f;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;
    private PlayerController playerController;

    private Vector2 moveInput;
    private bool isJumping = false;

    private enum MovementState { idle = 0, walk = 1, attack = 2, take_hit = 3 }

    [Header("Jump Settings")]
    [SerializeField] private LayerMask jumpableGround;
    private BoxCollider2D coll;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();

        playerController = new PlayerController(); // Inisialisasi PlayerInputActions
    }

    private void OnEnable()
    {
        playerController.Enable();

        // Binding untuk pergerakan
        playerController.Movement.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        playerController.Movement.Move.canceled += ctx => moveInput = Vector2.zero;

        // Binding untuk aksi Attack
        playerController.Attack.Attack.performed += ctx => Attack(); // Binding untuk Attack
    }

    private void OnDisable()
    {
        playerController.Disable();
    }

    private void Attack()
    {
        anim.SetInteger("state", (int)MovementState.attack);
    }

    private void Update()
    {
        moveInput = playerController.Movement.Move.ReadValue<Vector2>();

        // Menambahkan kondisi jika Attack dipicu
        if (playerController.Attack.Attack.triggered)
        {
            anim.SetInteger("state", (int)MovementState.attack);
        }
    }

    private void FixedUpdate()
    {
        // Mengatur pergerakan karakter
        Vector2 targetVelocity = new Vector2(moveInput.x * moveSpeed, rb.velocity.y);
        rb.velocity = targetVelocity;
        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        MovementState state = MovementState.idle;

        if (playerController.Attack.Attack.triggered) // Jika aksi Attack dipicu
        {
            state = MovementState.attack;
        }
        else if (moveInput.x != 0f) // Jika bergerak
        {
            state = MovementState.walk;
            sprite.flipX = moveInput.x < 0f; // Flip sprite jika bergerak ke kiri
        }

        anim.SetInteger("state", (int)state);
    }

    private bool isGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }

    private void Jump()
    {
        if (isGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }
}
