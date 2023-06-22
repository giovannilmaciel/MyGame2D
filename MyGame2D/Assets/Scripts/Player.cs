using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    [SerializeField] float runSpeed;
    [SerializeField] float jumpSpeed;
    [SerializeField] float climbingSpeed;

    Rigidbody2D myRigidbody2D;
    Animator myAnimator;
    BoxCollider2D myBoxCollider2D;
    PolygonCollider2D myPlayersFeet;

    float startingGravityScale;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBoxCollider2D = GetComponent<BoxCollider2D>();
        myPlayersFeet = GetComponent<PolygonCollider2D>();

        startingGravityScale = myRigidbody2D.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        Jump();
    }

    private void FixedUpdate()
    {
        Run();
        Climb();
    }

    private void Run()
    {
        float moveController = CrossPlatformInputManager.GetAxis("Horizontal");

        Vector2 playerVelocity = new Vector2(moveController * runSpeed, myRigidbody2D.velocity.y);
        myRigidbody2D.velocity = playerVelocity;

        if (moveController > 0)
        {
            myAnimator.SetBool("running", true);
            transform.eulerAngles = new Vector3(0, 0, 0);
        }

        if(moveController < 0)
        {
            myAnimator.SetBool("running", true);
            transform.eulerAngles = new Vector3(0, 180, 0);
        }

        if(moveController == 0)
        {
            myAnimator.SetBool("running", false);
            myAnimator.SetBool("idling", true);
        }

        
    }

    private void Jump()
    {
        if (!myPlayersFeet.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return; }

        bool isJumping = CrossPlatformInputManager.GetButtonDown("Jump");

        if (isJumping)
        {
            Vector2 jumpVelocity = new Vector2(myRigidbody2D.velocity.x, jumpSpeed);
            myRigidbody2D.velocity = jumpVelocity;
        }
    }
    
    private void Climb()
    {
        if (myBoxCollider2D.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            float moveController = CrossPlatformInputManager.GetAxis("Vertical");

            Vector2 climbingVelocity = new Vector2(myRigidbody2D.velocity.x, moveController * climbingSpeed);
            myRigidbody2D.velocity = climbingVelocity;

            myAnimator.SetBool("climbing", true);

            myRigidbody2D.gravityScale = 0f;
        }
        else
        {
            myAnimator.SetBool("climbing", false);
            myRigidbody2D.gravityScale = startingGravityScale;
        }
    }
}
