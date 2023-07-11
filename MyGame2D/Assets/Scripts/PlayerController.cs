using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float jumpForce;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float groundRadius = 0.1f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private Vector2 newMovement;

    private bool facingRight = true;

    private bool jump;
    private bool grounded;
    private bool doubleJump;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();    
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);

        if (grounded)
            doubleJump = false;
    }

    void FixedUpdate()
    {
        rb.velocity = newMovement;

        if (jump)
        {
            jump = false;
            rb.velocity = Vector2.zero;
            rb.AddForce(Vector2.up * jumpForce);

            if(!doubleJump && !grounded)
            {
                doubleJump = true;
            }
        }
    }

    public void Jump()
    {
        if (grounded || !doubleJump)
            jump = true;
    }
    public void Move(float direction)
    {
        float currentSpeed = walkSpeed;
        newMovement = new Vector2(direction * currentSpeed, rb.velocity.y);

        if (facingRight && direction < 0)
        {
            Flip();
        }
        else if (!facingRight && direction > 0)
        {
            Flip();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;

        transform.Rotate(0, 180, 0);
    }
}
