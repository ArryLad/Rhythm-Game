using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallPatrolEnemy : MonoBehaviour
{
    //Fundamentally the same as patrol enemy 
    //movement is done along the Y axis 

    [Header("LayerMask")]
    [SerializeField] LayerMask groundLayer;

    [Header("Components")]
    private Rigidbody2D rbEnemy;

    [Header("Patrol")]
    [SerializeField] private int moveSpeed;
    private float moveDirection = 1;
    private bool facingUp = true;
    [SerializeField] Transform groundCheck;
    [SerializeField] Transform wallCheck;
    [SerializeField] float checkRadius;
    private bool onGround;
    private bool onWall;

    [Header("isSeen")]
    [SerializeField] inCamera inCamera;
    void Start()
    {
        inCamera = GetComponent<inCamera>();
        rbEnemy = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (inCamera.isSeen)
        {
            CheckCollisions();
            Patrolling();
        }
        else
            rbEnemy.velocity = new Vector2(0f, 0f);
    }

    void CheckCollisions()
    {
        onGround = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
        onWall = Physics2D.OverlapCircle(wallCheck.position, checkRadius, groundLayer);
    }

    void Patrolling()
    {
        if (!onGround || onWall)
        {
            if (facingUp)
            {
                ChangeDirection();
            }
            else if (!facingUp)
            {
                ChangeDirection();
            }
        }
        //movement along the Y axis
        rbEnemy.velocity = new Vector2(rbEnemy.velocity.x, moveSpeed * moveDirection);
    }

    void ChangeDirection()
    {
        moveDirection *= -1;
        facingUp = !facingUp;
        transform.Rotate(180, 0, 0);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        Gizmos.DrawWireSphere(wallCheck.position, checkRadius);

    }
}
