using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolEnemy : MonoBehaviour
{
    [Header("LayerMask")]
    [SerializeField] LayerMask groundLayer; //get ground layer as layermask

    [Header("Components")]
    private Rigidbody2D rbEnemy;

    [Header("Patrol")]
    [SerializeField] private int moveSpeed; //patrol enemy movement speed
    private float moveDirection = 1; //the direction it choses to move in
    [SerializeField] Transform groundCheck; //gizmo position
    [SerializeField] Transform wallCheck;
    [SerializeField] float checkRadius; //radius to check 
    public bool onGround;
    private bool onWall;
    public bool isFalling;
    public float lastYPos = 0;

    [Header("isSeen")]
    [SerializeField] inCamera inCamera;

    void Start()
    {
        inCamera = GetComponent<inCamera>(); //get inCamera script
        lastYPos = transform.position.y;
        rbEnemy = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (inCamera.isSeen) //if the enemy is inside the viewport of the camera then do its behaviours
        {
            CheckCollisions(); //check collisions
            lastYPos = transform.position.y; //update lastYPos with currect transform y position
            Patrolling(); //patrol
        }
        else //dont do anything as they are not in view of the player
            rbEnemy.velocity = new Vector2(0f, rbEnemy.velocity.y);

    }

    void CheckCollisions()
    {
        onGround = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer); //uses radius at point of groundCheck to check if contacting ground Layer
        onWall = Physics2D.OverlapCircle(wallCheck.position, checkRadius, groundLayer); //uses radius at point of wallCheck to check if contacting ground Layer

        if (!onGround && transform.position.y < lastYPos) 
            isFalling = true;
    }

    void Patrolling()
    {
        if (!isFalling) //patroll is not falling
        {
            if (!onGround || onWall) //change direction if at the edge of a platform or hitting a wall
                ChangeDirection();
            rbEnemy.velocity = new Vector2(moveSpeed * moveDirection, rbEnemy.velocity.y); //move enemy in moveDirection at moveSpeed
        }
        else if(isFalling && onGround) //if just landed
        {
            isFalling = false; //no longer falling
            ChangeDirection(); //change direction (it will have flipped just before it detects its falling)
        }
        else
            rbEnemy.velocity = new Vector2(0f, rbEnemy.velocity.y);
    }

    void ChangeDirection()
    {
        moveDirection *= -1; //reverse moveDirection
        transform.Rotate(0, 180, 0); //flip enemy 
    }

    private void OnDrawGizmosSelected()
    {
        //draw gizmos for testing to see where groundcheck and wall check's radius
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        Gizmos.DrawWireSphere(wallCheck.position, checkRadius);

    }
}
