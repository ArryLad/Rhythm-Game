using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Brilliant Labs / Labos Créatifs (2020) Creating A Frog Enemy | Unity 2D Platformer Tutorial Part 5 [Online Video] [Access Date: 26th April 2022] https://youtu.be/1e-9Zbam_s0 
public class frogController : MonoBehaviour
{
    [Header("Layer Mask")]
    public LayerMask groundLayer;

    [Header("Components")]
    private Rigidbody2D rbEnemy;
    private SpriteRenderer sprite;

    [Header("Face Player Variables")]
    private GameObject player;
    public bool directionPicked = false; //bool to detect if direction has been picked
    public bool facingRight = false; //used to determine when to flip sprite

    [Header("Boolean Collision Variables")]
    //varaibles used to control states the frog is in so the appropriate behaviour is executed
    public bool isGrounded = false;
    public bool isFalling = false;
    public bool isJumping = false;
    public bool onWall = false;

    public float lastYPos = 0;

    [Header("Jump Variables")]
    public float jumpForceX = 2f; //jump force used horizontally
    public float jumpForceY = 4f; //jump force used vertically
    public float bounceForce; 
    public int jumpDirection = 0; //similar use to moveDirection in other enemies


    [Header("Idle Variables")]
    public float idleTime = 2f; 
    public float currentIdleTime = 0;
    public bool isIdle = true;

    [Header("isSeen")]
    [SerializeField] inCamera inCamera;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").gameObject;
        inCamera = GetComponent<inCamera>();
        lastYPos = transform.position.y;
        rbEnemy = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>(); //get spreite to be flipped
        bounceForce = jumpForceX/2; //bounceForce is half the X jumpForce for bouncing off walls
    }
    void FixedUpdate()
    {
        if (inCamera.isSeen)
        {
            CheckCollisions();
            State();
            //When the frog is idle execute wait then jump
            if (isIdle && !isFalling)
            {
                if (!directionPicked)
                {
                    FacePlayer();
                }
                currentIdleTime += Time.deltaTime;
                //Once two seconds have passed
                if (currentIdleTime >= idleTime)
                {
                    currentIdleTime = 0;
                    Jump();
                }
            }
            
        }
    }

        void CheckCollisions()
        {
            //Determine if frog is grounded
            isGrounded = Physics2D.OverlapArea(new Vector2(transform.position.x - 0.45f, transform.position.y - 0.5f),
                new Vector2(transform.position.x + 0.45f, transform.position.y - 0.51f), groundLayer);
        //Determine if frog is on a wall in the direction the enemy if facing
        if (facingRight)
            onWall = Physics2D.OverlapArea(new Vector2(transform.position.x + 0.5f, transform.position.y + 0.49f),
                new Vector2(transform.position.x + 0.51f, transform.position.y - 0.5f), groundLayer);
        else
                onWall = Physics2D.OverlapArea(new Vector2(transform.position.x - 0.5f, transform.position.y - 0.49f),
                        new Vector2(transform.position.x - 0.51f, transform.position.y - 0.5f), groundLayer);
        }

        void State()
        {
            //We have just fallen onto the ground, now the frog is Idle
            if (isGrounded && isFalling)
            {
                rbEnemy.velocity = new Vector2(0f, 0f); //halt velocity
                isFalling = false;
                isJumping = false;
                directionPicked = false;
                isIdle = true;
            }
            //if greater than lastYPos frog is jumping 
            else if (transform.position.y > lastYPos && !isGrounded && !isIdle)
            {
                isJumping = true;
                isFalling = false;
            }
            //if smaller than lastYPos frog is falling 
            else if (transform.position.y < lastYPos && !isGrounded)
            {
                isJumping = false;
                isFalling = true;
            }

            //if the enemy touches a wall and is falling or jumping
            if (onWall && ( isJumping || isFalling)) 
            {
                //flip the sprite to its corrosponding direction
                if (facingRight)
                {
                    facingRight = false;
                    sprite.flipX = true;
                }
                else
                {
                    facingRight = true;
                    sprite.flipX = false;
                }
                //stop whatever X velocity the enemy had, so it can bounce in the other direction without competing with other force
                rbEnemy.velocity = new Vector2(0f, rbEnemy.velocity.y);
                //add force in jumpDirection with bounceForce
                rbEnemy.AddForce(Vector2.right * (-jumpDirection * bounceForce), ForceMode2D.Impulse);
                //enemy has changed direction so must jumpDirection (in case on multiple rebounds);
                jumpDirection = -jumpDirection;
            }

            lastYPos = transform.position.y;
        }
    

    void FacePlayer()
    {
        //flip the sprite, assign facingRight to true or false, and jumpDirection to 1 or -1
        //to reflect the direction the enemy should jump/look toward
        if(transform.position.x < player.transform.position.x)
        {
            facingRight = true;
            jumpDirection = 1;
            sprite.flipX = false;
        }
        else
        {
            facingRight = false;
            jumpDirection = -1;
            sprite.flipX = true;

        }
        //a direction has been picked
        directionPicked = true;
    }

    void Jump()
    {
        //now enemy has jumped it is not idle
        isJumping = true;
        isIdle = false;
        //halt horizontal velocity and add force upward and horizontally in jumpDirection
        rbEnemy.velocity = new Vector2(0f, rbEnemy.velocity.y);
        rbEnemy.AddForce(Vector2.up * jumpForceY, ForceMode2D.Impulse);
        rbEnemy.AddForce(Vector2.right * (jumpDirection * jumpForceX), ForceMode2D.Impulse);
    }

}
