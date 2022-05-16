using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Tactical Programmer (2020) 2D Character Movement Tutorial in Unity! [Online Video] [Access Date: 21st April 2022] https://youtu.be/TTKPmPvekUY
//Brackeys (2018) 2D Shooting in Unity (Tutorial) [Online Video] [Access Date: 22th April 2022] https://youtu.be/wkKsl1Mfp5M
public class Movement : MonoBehaviour
{
    [Header("AmmoBar")]
    public AmmoBar ammoBar;

    [Header("Conductor")]
    public Conductor conductor;

    [Header("Experimental")]
    public GameManager GM;
    public bool experimental;

    [Header("Components")]
    private Rigidbody2D rb;

    [Header("Layer Masks/Groun Check")]
    [SerializeField] private LayerMask groundLayer;

    [Header("Movement Variables")]
    [SerializeField] private float Acceleration = 50f;
    [SerializeField] private float maxSpeed = 14f;
    [SerializeField] private float groundlinearDrag = 5f;
    private float horizontalDirection;
   
    //if velocity is moving the direction opposite to the inputed direction the player is changing direction
    private bool changingDirection => rb.velocity.x > 0f && horizontalDirection < 0f || (rb.velocity.x < 0f && horizontalDirection > 0f); 

    [Header("Jump Varibles")]
    [SerializeField] private float jumpForce = 12f; //jump force
    [SerializeField] private float bounceForce = 12f; //bounce force when stomping on enemys
    [SerializeField] private float airLinearDrag = 2.5f; //drag while airborne
    [SerializeField] private float fallMultiplier = 8f; //gravity scale changer
    [SerializeField] private float knockbackForce = 2f; //knockback force
    public bool isFalling; 
    public float lastYPos = 0;
    [SerializeField] public GameObject jumpParticle;
    [SerializeField] public GameObject landParticle;
    private bool canJump => Input.GetButtonDown("Jump") && onGround; //can jump bool true if pressing the jump button and on the ground

    [Header("Ground Collision Variables")]
    public bool onGround;
    [SerializeField] Transform groundCheck;
    [SerializeField] float checkDistance;

    [Header("Shooting")]
    [SerializeField] private LayerMask playerLayer; //used to let raycasting ignore the player layer
    [SerializeField] public int distance; //distance the raycast can travel
    [SerializeField] private Transform firePoint; //firepoint bullet is instantiated from
    [SerializeField] private Transform bulletPrefab; 
    [SerializeField] public GameObject shootParticle;
    [SerializeField] public GameObject brickParticle;
    [SerializeField] public GameObject hitGroundParticle;
    [SerializeField] public GameObject hitShellParticle;
    [SerializeField] public int damage = 1; //damage raycast shot does
    [SerializeField] private int currentAmmo; //current ammo the player has
    [SerializeField] public int maxAmmo = 10; //max ammo
    public LineRenderer lineRenderer; //line used to show ray cast line

    [Header("Shoot Movement")]
    [SerializeField] private float bulletForce = 4f; //the force shooting a applies 
    [SerializeField] private float stallForce = 1.5f; //the force shooting when empty applies
                                                    
    [Header("Score")]
    public ScoreKeeper scoreKeeper;
    [SerializeField] int points = 10;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lastYPos = 0; //initialise last position to 0
        ammoBar = GameObject.FindGameObjectWithTag("AmmoBar").GetComponent<AmmoBar>();
        conductor = GameObject.FindGameObjectWithTag("Conductor").GetComponent<Conductor>();
        scoreKeeper = GameObject.FindGameObjectWithTag("scoreKeeper").GetComponent<ScoreKeeper>(); //get score component from scoreKeeper
        Reload(); //reload at the start to ensure ammo and UI are updated
    }

    private void Update()
    {
        horizontalDirection = GetInput().x; //get current input of player
        if (canJump && conductor.CheckBeat() == true) //call jump function if canJump is true on beat
            Jump();
        
        if (!onGround && Input.GetButtonDown("Jump") && conductor.CheckBeat() == true) //if they player jumps while not on ground and on the beat 
        {
            if (currentAmmo > 0) //shoot if the player has ammo
            {
                if (experimental) //if in experimental mode raycast shoot
                    StartCoroutine(RaycastShoot());
                else //otherwise shoot a projectile
                    Shoot();
                FindObjectOfType<AudioManager>().Play("sShoot");
                Recoil(); //apply recoil after shooting
            }
            else
                Stall(); //apply stall force
        }
    }

    private void FixedUpdate()
    {
        CheckCollisions(); //check what the player is colliding with 
        lastYPos = transform.position.y; //check if the player Y is lower than it was last update
        MoveCharacter(); //move player function
        
        if(onGround) // if on the ground
        {
            Reload(); //reload boots 
            ApplyGroundLinearDrag(); //apply ground drag
            scoreKeeper.combo = 0;
        }
        else //while airborne
        {
            ApplyAirLinearDrag(); //apply air drag
            FallMultiplier(); //and change gravity scale
        }
    }

    private Vector2 GetInput()
    {
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")); //get input horizontal which unity assigns to A/D and left/right arrow keys
    }

    private void MoveCharacter()
    {
        //move player's horizontal direction by accelteration
        rb.AddForce(new Vector2(horizontalDirection, 0f) * Acceleration);

        if (Mathf.Abs(rb.velocity.x) > maxSpeed) //player can accelerate to maxspeed but not over it
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);
    }

    private void ApplyGroundLinearDrag()
    {
        if(Mathf.Abs(horizontalDirection) < 0.4f || changingDirection) //if moving apply drag
        {
            rb.drag = groundlinearDrag;
        }
        else //dont apply drag or player will drift
        {
            rb.drag = 0f;
        }
    }

    private void ApplyAirLinearDrag()
    {
        rb.drag = airLinearDrag;  //set drag to air linear drag
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0f); //halt vertial movement
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse); //apply jumpforce upward
        Instantiate(jumpParticle, firePoint.position, Quaternion.Euler(-90, 0 ,0)); //jump particle
        FindObjectOfType<AudioManager>().Play("sJump");
    }

    private void Recoil()
    {
       //same with Jump but apply bullet force
       rb.velocity = new Vector2(rb.velocity.x, 0f); 
       rb.AddForce(Vector2.up * bulletForce, ForceMode2D.Impulse);
    }

    private void Stall()
    {
        //same with Jump but apply stall force
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(Vector2.up * stallForce, ForceMode2D.Impulse);
    }

    public void stompBounce()
    {
        //on stomp reload and apply bounce force
        Reload();
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
    }

    public void knockBack(int moveDir)
    {
        //apply bounceforce upward and knockbackFroce in determined direction
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
        rb.AddForce(Vector2.right * (moveDir * knockbackForce), ForceMode2D.Impulse);
    }
    private void FallMultiplier()
    {
        //this section was used to experiment with differnet gravities when holding space but decided it was unnecessary
        if  (rb.velocity.y < 0) //if falling change gravity scale to fall multiplier
        {
            rb.gravityScale = fallMultiplier;
        }
        else
        {
            rb.gravityScale = 3f;
        }
    }

    private void CheckCollisions()
    {
        //check collision with ground layer using raycast
        onGround = Physics2D.Raycast(groundCheck.position, -transform.up, checkDistance, groundLayer);

        //if not on the ground and y posiiton is less than what it last was then the player is falling
        if (!onGround && transform.position.y < lastYPos)
            isFalling = true;

        if(onGround && isFalling) //if player has just landed
        {
            isFalling = false; //player is no longer falling
            Instantiate(landParticle, firePoint.position, Quaternion.Euler(-90, 0, 0)); //land particle
            FindObjectOfType<AudioManager>().Play("sLand");
        }
    }

    private void OnDrawGizmos()
    {
        //draw gizmos helps during testing, this shows the distance of raycast shooting
        Gizmos.color = Color.green;
        Gizmos.DrawLine(firePoint.position, new Vector2(firePoint.position.x, firePoint.position.y - distance));

    }

    void Shoot()
    {
        //instanitate a bullet and corrosponding particle at the firepoint position
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Instantiate(shootParticle, firePoint.position, Quaternion.identity);
        //reduce ammo by one and update ammo UI
        currentAmmo--;
        ammoBar.SetAmmo(currentAmmo, maxAmmo);
    }

    public void Reload()
    {
        if (currentAmmo != maxAmmo) //if not already at max ammo
        {
            FindObjectOfType<AudioManager>().Play("sReload");
            //set ammo to max and update UI
            currentAmmo = maxAmmo;
            ammoBar.SetMaxAmmo(maxAmmo);
        }
    }

    IEnumerator RaycastShoot()
    {
        //cay a ray from firepoint downward as far as distance defines, ignoring the player layer
        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, -firePoint.up, distance, ~playerLayer);
        Instantiate(shootParticle, firePoint.position, Quaternion.identity); //shoot particle
        //reduce ammo and update UI
        currentAmmo--;
        ammoBar.SetAmmo(currentAmmo, maxAmmo);
        if (hit) //if ray hits something that is not the player's layer
        {
            if (hit.transform.tag == "Enemy") //if that is an enemy
            {
                Enemy enemy = hit.transform.Find("Collider").GetComponent<Enemy>(); //find the compound collider tagged and get Enemy script component
                if (enemy.canGetShot) //if enemy can take damage via shooting
                {
                    enemy.TakeDamage(damage); //enemy takes damage
                    Instantiate(hitGroundParticle, hit.point, Quaternion.Euler(-90, 0, 0));
                    FindObjectOfType<AudioManager>().Play("sShootHit");
                }
                else
                {
                    //otherwise enemy is immune to shooting (turtle)
                    Instantiate(hitShellParticle, hit.point, Quaternion.Euler(-90, 0, 0));
                    FindObjectOfType<AudioManager>().Play("sShellMiss");
                }
            }
            else if (hit.transform.tag == "Brick")
            {
                //destory the brick and instantiate particle effect
                Instantiate(brickParticle, hit.transform.gameObject.transform.position, Quaternion.Euler(-270, 0, 0));
                Destroy(hit.transform.gameObject);
                FindObjectOfType<AudioManager>().Play("sBrick");
                scoreKeeper.addScore(points);
            }
            else if (hit.transform.tag == "Ground")
            {
                //instantiate particle effect, so the player feels ray hit something
                Instantiate(hitGroundParticle, hit.point, Quaternion.Euler(-90, 0, 0));
                FindObjectOfType<AudioManager>().Play("sShootHit");
            }

            //render the line from fire point position to where it detected a hit
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, hit.point);
            
        }
        else
        {
            //other wise line continues to distance and miss particle is instantiated
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, firePoint.position + -firePoint.up * distance);
            Instantiate(hitShellParticle, firePoint.position + -firePoint.up * distance, Quaternion.Euler(-90, 0, 0));
            FindObjectOfType<AudioManager>().Play("sShellMiss");
        }
        //render the line for 0.02 seconds works better than 0.01f or Time.deltatime;
        lineRenderer.enabled = true;
        yield return new WaitForSeconds(0.02f);
        lineRenderer.enabled = false;
    }
}
