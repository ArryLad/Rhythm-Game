using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Brackeys (2018) 2D Shooting in Unity (Tutorial) [Online Video] [Access Date: 22nd April 2022] https://youtu.be/wkKsl1Mfp5M
public class Bullet : MonoBehaviour
{
    [Header("Bullet")]
    public float speed = 20f; //speed the bullet will travel
    public int damage = 1; //damage bullet deals
    public Vector2 initPos; //initial position of bullet
    public float distance; //distance the bullet can travel
    public Rigidbody2D rb;
    [SerializeField] public GameObject brickParticle;
    [SerializeField] public GameObject hitGroundParticle;
    [SerializeField] public GameObject hitShellParticle;

    [Header("Score")]
    public ScoreKeeper scoreKeeper;
    [SerializeField] int points = 10;

    void Start()
    {
        scoreKeeper = GameObject.FindGameObjectWithTag("scoreKeeper").GetComponent<ScoreKeeper>();
        initPos = transform.position; //store initial position
        rb.velocity = transform.up * -speed; //apply velocity downwards at speed
    }


    void FixedUpdate()
    {
        float dist = Vector2.Distance(initPos, transform.position); //get the distance between initial position and current position of bullet
        if(dist >= distance) //if travelled the smae or more than distance
        {
            //desotry bullet, play particle effect and sound
            Instantiate(hitShellParticle, transform.position, Quaternion.Euler(-90, 0, 0));
            FindObjectOfType<AudioManager>().Play("sShellMiss");
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //If bullet collides with something, find Enemy component
        Enemy enemy = other.GetComponent<Enemy>();

        //if collision has that script the bullet has hit an enemy
        if(enemy != null)
        {
            if (enemy.canGetShot) //if that enemy takes bullet damage
            {
                enemy.TakeDamage(damage); //take damage
                Instantiate(hitGroundParticle, transform.position, Quaternion.Euler(-90, 0, 0)); 
                FindObjectOfType<AudioManager>().Play("sShootHit");
            }
            else //it cannot be damaged by bullets
            {
                Instantiate(hitShellParticle, transform.position, Quaternion.Euler(-90, 0, 0));
                FindObjectOfType<AudioManager>().Play("sShellMiss");
            }
            Destroy(gameObject); //destroy bullet
        }
        else if(other.gameObject.tag == "Brick") //if bullet hits a brick
        {
            Instantiate(brickParticle, other.transform.position, Quaternion.Euler(-270, 0, 0));
            Destroy(other.gameObject); //destroy brick
            FindObjectOfType<AudioManager>().Play("sBrick");
            scoreKeeper.addScore(points);
            Destroy(gameObject); //destroy bullet
        }
        else if(other.gameObject.tag == "Ground") //if bullet hits the ground
        {
            Instantiate(hitGroundParticle, transform.position, Quaternion.Euler(-90, 0, 0));
            FindObjectOfType<AudioManager>().Play("sShootHit");
            Destroy(gameObject);//destroy bullet

        }
 
    }
}
