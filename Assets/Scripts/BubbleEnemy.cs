using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleEnemy : MonoBehaviour
{
    //[Header("Layer Mask")]
    //public LayerMask groundLayer;

    [Header("Components")]
    private Rigidbody2D rbEnemy;

    [Header("Follow")]
    [SerializeField] float moveSpeed;
    public Vector2 movement;
    private GameObject player;

    //OLd variables from trying a bounce but it didnt work well with following the player
    //[SerializeField] Transform player;
    //public bool isWallGround = false;
    // public Vector3 curPos;
    // public Vector3 lastPos;
    // public Vector3 lastVelocity;
    // public bool isBounced;
    // public bool isFollowing;
    // [SerializeField] float bounceForce;
    // [SerializeField] float bounceTime;
    // public float currentBounceTime;

    [Header("isSeen")]
    [SerializeField] inCamera inCamera;

    void Start()
    {
        inCamera = GetComponent<inCamera>();
        //lastPos = transform.position;
        rbEnemy = this.GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").gameObject;
    }


    private void Update()
    {
        //Get the direction from the enemy to the player
        Vector3 direction = player.transform.position - transform.position;

        //Normalise direction
        direction.Normalize();

        //assgin to movement as a vector2
        movement = direction; 
    }

    private void FixedUpdate()
    {
        if (inCamera.isSeen)
        {
            Follow(movement);
        }
    }

    void Follow(Vector2 direction)
    {
        //Use MovePosition to move enemy toward players position
        rbEnemy.MovePosition((Vector2)transform.position + (direction * moveSpeed * Time.deltaTime)); 
    }

    /*
    void Update()
    {
        if (isFollowing)
        {
            Vector3 direction = player.position - transform.position;
            direction.Normalize();
            movement = direction;
            
        }

    }

    private void FixedUpdate()
    {
        curPos = transform.position;
        Debug.Log("curPos" + curPos);
        //CheckCollisions();
        if (isWallGround)
        {
            isBounced = true;
            isFollowing = false;
            Bounce(movement);
        }
        if (isBounced)
        {
            currentBounceTime += Time.deltaTime;
            if(currentBounceTime >= bounceTime)
            {
                currentBounceTime = 0;
                isBounced = false;
            }
        }
        else
            Follow(movement);
        Debug.Log("lastPos" + lastPos);

    }

    void Follow(Vector2 direction)
    {
        isFollowing = true;
        rbEnemy.MovePosition((Vector2)transform.position + (direction * moveSpeed * Time.deltaTime));
    }
    /*void CheckCollisions()
    {
        isWallGround = Physics2D.OverlapArea(new Vector2(transform.position.x - 0.55f, transform.position.y - 0.55f),
                new Vector2(transform.position.x + 0.55f, transform.position.y + 0.55f), groundLayer);

    }
    void Bounce(Vector2 direction)
    {
        rbEnemy.velocity = new Vector2(0f, 0f);
        //var move = Vector3.Reflect(, coll.contacts[0.normal]);
        //rbEnemy.AddForce(direction * bounceForce, ForceMode2D.Impulse);
        //rbEnemy.AddForce(-direction * bounceForce, ForceMode2D.Impulse);
        //rbEnemy.AddForce((direction*-1) * bounceForce, ForceMode2D.Impulse);
    }
    */
}
