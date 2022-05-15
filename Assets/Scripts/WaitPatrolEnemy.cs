using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitPatrolEnemy : MonoBehaviour
{
    [Header("LayerMask")]
    [SerializeField] LayerMask groundLayer;

    [Header("Components")]
    private Rigidbody2D rbEnemy;

    [Header("Patrol")]
    [SerializeField] private int moveSpeed;
    [SerializeField] float patrolTime = 2f; //how long the enemy will patrol for 
    [SerializeField] float currentPatrolTime = 0; //stores length of time enemy has been patrolling
    private float moveDirection = 1; 
    [SerializeField] Transform groundCheck;
    [SerializeField] Transform wallCheck;
    [SerializeField] float checkRadius;
    private bool onGround;
    private bool onWall;
    public bool isFalling;
    public float lastYPos = 0;

    [Header("Idle")]
    [SerializeField] float idleTime = 2f; //time to remain idle 
    [SerializeField] float currentIdleTime = 0; //stores how long enemy has been idle
    [SerializeField] bool isIdle = true; //is enemy idle

    [Header("isSeen")]
    [SerializeField] inCamera inCamera;

    void Start()
    {
        inCamera = GetComponent<inCamera>();
        lastYPos = transform.position.y;
        rbEnemy = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (inCamera.isSeen)
        {
            CheckCollisions();
            if (isIdle && !isFalling) //if enemy is not falling and is idle
            {
                currentIdleTime += Time.deltaTime; //count up with Time
                if (currentIdleTime >= idleTime) //if the enemy has been idle for idle time or longer
                {
                    currentIdleTime = 0; //reset currentIdleTime
                    isIdle = false; //enemy no longer idle
                }
            }
            else if (!isIdle && !isFalling) //if not idle and not falling
            {
                Patrolling(); //patrol
                currentPatrolTime += Time.deltaTime; //count up with time
                if (currentPatrolTime >= patrolTime) //if the enemy has been patrolling for patrol time or longer
                {
                    currentPatrolTime = 0; //reset currentPatrolTime
                    isIdle = true; //enemy is idle 
                }
            }
            else
                Patrolling(); //patroll if falling as patrolling() stops movement when falling
        }
        else
            rbEnemy.velocity = new Vector2(0f, rbEnemy.velocity.y);
    }

    //BELOW IS THE SAME AS PATROLLING ENEMY COMMENTS
    void CheckCollisions()
    {
        onGround = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
        onWall = Physics2D.OverlapCircle(wallCheck.position, checkRadius, groundLayer);

        if (!onGround && transform.position.y < lastYPos)
            isFalling = true;
    }

    void Patrolling()
    {
        if (!isFalling)
        {
            if (!onGround || onWall)
                ChangeDirection();
            rbEnemy.velocity = new Vector2(moveSpeed * moveDirection, rbEnemy.velocity.y);
        }
        else if (isFalling && onGround)
        {
            isFalling = false;
            ChangeDirection();
        }
        else
            rbEnemy.velocity = new Vector2(0f, rbEnemy.velocity.y);
    }

    void ChangeDirection()
    {
        moveDirection *= -1;
        transform.Rotate(0, 180, 0);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        Gizmos.DrawWireSphere(wallCheck.position, checkRadius);

    }
}
