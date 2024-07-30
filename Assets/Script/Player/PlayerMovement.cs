using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent (typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Parameters")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;

    [Header("Coyote Jump")]
    [SerializeField] private float coyoteTime; // Time That Player Can Hang in the air before Jumping
    private float coyoteCounter; // Time Passed player tunn off edge

    [Header("Multiple Jumps")]
    [SerializeField] private int extraJumps;
    private int jumpCounter;

    [Header("Wall Jumping")]
    [SerializeField] private float wallJumpX; //Horizontal WallJump
    [SerializeField] private float wallJumpY; //Vertical WallJump

    [Header("Layers")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    [Header("Sounds")]
    [SerializeField] private AudioClip jumpSound;

    private Rigidbody2D body;
    private Animator animator;
    private BoxCollider2D boxCollider;
    private float wallJumpCooldown;
    private float horizontalInput;

    private void Awake()
    {
        // References RigidBody, Animator, and BoxCollider from Object
        if (!TryGetComponent<Rigidbody2D>(out body))
        {
            Debug.LogError("Rigidbody2D component not found on this GameObject.");
        }

        if (!TryGetComponent<Animator>(out animator))
        {
            Debug.LogError("Animator component not found on this GameObject.");
        }

        if (!TryGetComponent<BoxCollider2D>(out boxCollider))
        {
            Debug.LogError("BoxCollider2D component not found on this GameObject.");
        }
    }

    private void Update()
    {
        // Setting Animator Parameters
        animator.SetBool("Walk", horizontalInput != 0);
        animator.SetBool("Grounded", IsGrounded());

        if (OnWall())
        {
            body.gravityScale = 0;
            body.velocity = Vector3.zero;
        }
        else
        {
            body.gravityScale = 7;
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

            if (IsGrounded())
            {
                coyoteCounter = coyoteTime; // Reset Coyote Counter when on the ground Value will never be 0 or less if we are grounded
                jumpCounter = extraJumps; // Reset Extra Jumps when player touches ground
            }
            else
                coyoteCounter -= Time.deltaTime; // decrease coyote counter when not in the ground until it is >0 u can jump again
        }
    }

    public void SetHorizontalInput(float input)
    {
        horizontalInput = input;
        //Flip Player Upon Moving Left\Right
        if (horizontalInput > 0.01f)
        {
            transform.localScale = Vector3.one;
        }
        else if (horizontalInput < -0.01f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    public void Jump()
    {
        if (coyoteCounter < 0 && !OnWall() && jumpCounter <= 0) // if Coyote counter is 0 ore less and not onWall dont do anything Jump Counter reaches Zero Extra Jumps Disable
        {
            return;
        }

        SoundManager.instance.PlaySound(jumpSound);

        if (OnWall())
        {
            WallJump();
        }
        else
        {
            if (IsGrounded())
            {
                body.velocity = new Vector2(body.velocity.x, jumpPower);
            }
            else
            {
                //If not on the ground and coyote counter >0 do a normal jump
                if (coyoteCounter > 0)
                {
                    body.velocity = new Vector2(body.velocity.x, jumpPower);

                    if (jumpCounter > 0) // If we have extra jumps Jump and decrease them
                    {
                        body.velocity = new Vector2(body.velocity.x, jumpPower);
                        jumpCounter--;
                    }
                }
            }
            // Reset Coyote Jump Avoid Double Jumps
            coyoteCounter = 0;
        }
    }

    public void AdjustableJump()
    {
        if (body.velocity.y > 0)
        {
            body.velocity = new Vector2(body.velocity.x, body.velocity.y / 2);
        }
    }

    private void WallJump()
    {
        body.AddForce(new Vector2(-Mathf.Sign(transform.localScale.x) * wallJumpX, wallJumpY));
        wallJumpCooldown = 0;
    }

    private bool IsGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    private bool OnWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }

    public bool CanAttack()
    {
        return horizontalInput == 0 && IsGrounded() && !OnWall();
    }
}
