using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatEnemy : MonoBehaviour
{
    [Header("Ground Check")]
    public LayerMask groundLayer;
    [SerializeField] Transform groundCheck;
    [SerializeField] float checkDistance;
    public bool onCeiling;

    [Header("Components")]
    private Rigidbody2D rbEnemy;

    [Header("Follow")]
    [SerializeField] float moveSpeed;
    public Vector2 movement;
    private GameObject player;
    //Bats behaviour is actived by certain conditions
    public bool isActive; 

    [Header("isSeen")]
    [SerializeField] inCamera inCamera;

    // Start is called before the first frame update
    void Start()
    {
        inCamera = GetComponent<inCamera>();
        isActive = false; //bat is not active
        rbEnemy = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").gameObject;
    }

    private void Update()
    {
        //Get direction same as Bubble Enemy
        Vector3 direction = player.transform.position - transform.position;
        direction.Normalize();
        movement = direction;
    }

    private void FixedUpdate()
    {
        if (inCamera.isSeen)
        {
            CheckCollisions(); //check for collisions
            if (isActive)
                Follow(movement);
        }
    }

    void CheckCollisions()
    {
        //Check if there is ground above the bat
        onCeiling = Physics2D.Raycast(groundCheck.position, transform.up, checkDistance, groundLayer);

        //if the bat is inactive and player position is lower than or the same the bats or there is no more ground above the bat
        if ((player.transform.position.y <= transform.position.y || !onCeiling) && !isActive)
        {
            //bat is active and rotate to reflect not hanging upside down
            isActive = true;
            transform.Rotate(0, 0, 180);
        }
    }

    void Follow(Vector2 direction)
    {
        //move same as bubble enemy
        rbEnemy.MovePosition((Vector2)transform.position + (direction * moveSpeed * Time.deltaTime));
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector2(groundCheck.position.x, groundCheck.position.y + checkDistance));
    }
}