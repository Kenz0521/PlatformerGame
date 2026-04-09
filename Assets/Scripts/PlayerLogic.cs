using Unity.VisualScripting.InputSystem;
using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    public float speed = 6f;
    public float jumpForce = 8f;
    public float gravity = -20f;

    CharacterController controller;
    Vector3 velocity;

    public Transform groundCheck;
    public float groundDistance = 0.3f;
    public LayerMask groundMask;

    bool isGrounded;

    [SerializeField] Animator animator;
    private int x;
    private int z;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    void Update()
    {
        GroundCheck();
        Move();
        Jump();
        UpdateAnimations();
    }

    void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = 0f; // reset downward velocity when on the ground
        }
    }

    void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = Camera.main.transform.right * x + Camera.main.transform.forward * z;
        move.y = 0f;

        if (move != Vector3.zero)
        {
            transform.forward = move; // rotate player towards movement
            animator.SetBool("IsRunning", true);
        }
        else
        {
            animator.SetBool("IsRunning", false);
        }

        controller.Move(move * speed * Time.deltaTime);
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = jumpForce;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void UpdateAnimations()
    {
        // RUNNING / IDLE
        float speedValue = Mathf.Abs(Input.GetAxis("Horizontal")) + Mathf.Abs(Input.GetAxis("Vertical"));
        animator.SetFloat("Speed", speedValue);

        // FALLING (up or down)
        bool isFalling = !isGrounded; // triggers Falling animation whenever in the air
        animator.SetBool("IsFalling", isFalling);

        // GROUNDED
        animator.SetBool("IsGrounded", isGrounded);
    }
}
