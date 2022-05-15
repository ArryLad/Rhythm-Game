using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawlEnemy : MonoBehaviour
{
    [Header("LayerMask")]
    [SerializeField] LayerMask groundLayer;

    [Header("Components")]
    private Rigidbody2D rbEnemy;

    [Header("Crawl")]
    [SerializeField] private int moveSpeed;
    private int startDirection = 1; //assing direction (right)
    public bool hasTurned; //boolean to check if enemy has turned
    private float rotateZ; //variable used to rotate Z axis
    [SerializeField] float offset;
    public int Direction; //direction the enemy is going in
    [SerializeField] Transform groundCheck;
    [SerializeField] Transform wallCheck;
    [SerializeField] float checkDistance;

    public bool onGround;
    public bool onWall;
    //public bool isFalling;
    //public float lastYPos = 0;
    //[SerializeField] float gravity;

    [Header("isSeen")]
    [SerializeField] inCamera inCamera;

    void Start()
    {
        inCamera = GetComponent<inCamera>();
        rbEnemy = GetComponent<Rigidbody2D>();
        rbEnemy.gravityScale = 0;
        hasTurned = false;
        Direction = 1;
        //use random value to pick a starting direction
        if (Random.value < 0.5f)
        {
            startDirection = -1;
        }
    }

    private void FixedUpdate()
    {
        if (inCamera.isSeen)
        {
            CheckCollisions(); 
            /*lastYPos = transform.position.y;
            if (!isFalling)
            {
                CheckFalling();
                Movement();
                Crawling();
            //}
            else if (isFalling && onGround)
            {
                isFalling = false;
                rbEnemy.gravityScale = 0;
            }
            else
                rbEnemy.velocity = new Vector2(0f, rbEnemy.velocity.y);*/
            Movement();
            Crawling();
        }
        else
            rbEnemy.velocity = new Vector2(0f, 0f);
    }

    void CheckCollisions()
    {
        onGround = Physics2D.Raycast(groundCheck.position, -transform.up,checkDistance, groundLayer);
        if (startDirection == 1) //right side check
            onWall = Physics2D.Raycast(wallCheck.position, transform.right ,checkDistance, groundLayer);
        else //left side check if moving to the left
            onWall = Physics2D.Raycast(wallCheck.position, -transform.right, checkDistance, groundLayer);

        /*if (!onGround && transform.position.y < lastYPos && !hasTurned)
        {
            isFalling = true;
            transform.rotation = Quaternion.identity;
            rbEnemy.gravityScale = gravity;
        }*/
    }

    /*void CheckFalling()
    {
        if (!onGround && transform.position.y < lastYPos && !hasTurned)
        {
            isFalling = true;
            transform.rotation = Quaternion.identity;
            rbEnemy.gravityScale = gravity;
        }
    }*/

    void Crawling()
    {
        //rbEnemy.velocity = new Vector2(moveSpeed * moveDirection, rbEnemy.velocity.y);
        if (!onGround) //if not on the ground
        {
            if (hasTurned == false) //if the enemy has not turned
            {
                rotateZ += (startDirection * -90); //rotate by 90 degree in the way dictated by startDirection 
                transform.eulerAngles = new Vector3(0, 0, rotateZ); //Z axis should reflect amount rotated
                if (startDirection == 1) //Depending on start direction call Crawl functions
                    CrawlGroundRight();
                else
                    CrawlGroundLeft();
                hasTurned = true; //enemy has turned
            }
        }
        else //while enemy is on the ground it shouldn't turn
            hasTurned = false;

        if(onWall) //if in contact with a wall, do the same code as above but rotate in opposite direction
        {
            if(hasTurned == false)
            {
                rotateZ += (startDirection * 90);
                transform.eulerAngles = new Vector3(0, 0, rotateZ);
                if (startDirection == 1)
                    CrawlWallRight(); 
                else
                    CrawlWallLeft();
                hasTurned = true;
            }
        }
    }

    //WITH EACH CRAWL FUNCTION
    //check Direction, dpending on the value of direction, add or subtract an offset tp the transform position
    //so that the enemy is on the surface of the ground/wall its turning to.

    void CrawlGroundRight()
    {
        if (Direction == 1) //Moving right on ground
        {
            //stick enemy to surface going down
            transform.position = new Vector2(transform.position.x + offset, transform.position.y - offset);
            Direction = 2;
        }
        else if (Direction == 2) //Moving down on the right side of ground
        {
            //stick enemy to surface going left
            transform.position = new Vector2(transform.position.x - offset, transform.position.y - offset);
            Direction = 3;
        }
        else if (Direction == 3) //Moving left underneath ground
        {
            //stick enemy to surface going upward
            transform.position = new Vector2(transform.position.x - offset, transform.position.y + offset);
            Direction = 4;
        }
        else if (Direction == 4) //Moving up on the left side of ground
        {
            //stick enemy to surface going right
            transform.position = new Vector2(transform.position.x + offset, transform.position.y + offset);
            Direction = 1;
        }
    }

    //Same as CrawlGround Right but with direction informs different changes to transform position of enemy
    void CrawlGroundLeft()
    {
        if (Direction == 1)
        {
            transform.position = new Vector2(transform.position.x - offset, transform.position.y - offset);
            Direction = 2;
        }
        else if (Direction == 2)
        {
            transform.position = new Vector2(transform.position.x + offset, transform.position.y - offset);
            Direction = 3;
        }
        else if (Direction == 3)
        {
            transform.position = new Vector2(transform.position.x + offset, transform.position.y + offset);
            Direction = 4;
        }
        else if (Direction == 4)
        {
            transform.position = new Vector2(transform.position.x - offset, transform.position.y + offset);
            Direction = 1;
        }
    }

    //Fundamentally same as ground crawl but applied to walls
    void CrawlWallRight()
    {
        if (Direction == 1)
        {
            transform.position = new Vector2(transform.position.x + offset, transform.position.y);
            Direction = 4;
        }
        else if (Direction == 2)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y - offset);
            Direction = 1;
        }
        else if (Direction == 3)
        {
            transform.position = new Vector2(transform.position.x - offset, transform.position.y);
            Direction = 2;
        }
        else if (Direction == 4)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + offset);
            Direction = 3;
        }
    }

    void CrawlWallLeft()
    {
        if (Direction == 1)
        {
            transform.position = new Vector2(transform.position.x - offset, transform.position.y);
            Direction = 4;
        }
        else if (Direction == 2)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y - offset);
            Direction = 1;
        }
        else if (Direction == 3)
        {
            transform.position = new Vector2(transform.position.x + offset, transform.position.y);
            Direction = 2;
        }
        else if (Direction == 4)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + offset);
            Direction = 3;
        }
    }

    void Movement()
    {
        rbEnemy.velocity = transform.right * (moveSpeed * startDirection);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector2(groundCheck.position.x, groundCheck.position.y - checkDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector2(wallCheck.position.x + checkDistance, wallCheck.position.y));
    }
}
