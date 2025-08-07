using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerInput playerInput;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;

    [SerializeField] private Animator Animator;
    private Vector2 input;

    public bool isGrounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        input = playerInput.actions["Move"].ReadValue<Vector2>();

        // Activar animación si se está moviendo
        Animator.SetBool("isRunning", Mathf.Abs(input.x) > 0.01f);

        // Animación de salto (cuando está en el aire)
        Animator.SetBool("IsJumping", !isGrounded);
        Animator.SetBool("IsFalling", rb.linearVelocity.y < -0.1f && !isGrounded);
  
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(input.x * moveSpeed, rb.linearVelocity.y);

        // Flip del sprite si se mueve a la izquierda o derecha
        if (input.x > 0.01f)
        {
            GetComponent<SpriteRenderer>().flipX = false; // Mira a la derecha
        }
        else if (input.x < -0.01f)
        {
            GetComponent<SpriteRenderer>().flipX = true; // Mira a la izquierda
        }

    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            Debug.Log("Jump pressed");
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            Animator.SetBool("IsJumping", !isGrounded);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
    
        }
    }
}