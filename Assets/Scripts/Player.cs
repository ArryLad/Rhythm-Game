using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("GameManager")]
    public GameManager GM; //GameManager

    [Header("Player Variables")]
    public int maxHealth = 4; //Set maxhealth 
    public int currentHealth; 

    [Header("iFrames")]
    [SerializeField] private Color flashColor; //Color for the player to flash 
    private Color regularColor; //regular colour of sprite will be assigned to this
    [SerializeField] SpriteRenderer sprite; //reference to spriteRenderer
    [SerializeField] float flashDuration; //flash duration
    [SerializeField] int numberOfFlashes; //the number of times the player will flicker
    [SerializeField] bool invincible; //determine if player can be damaged

    [Header("Player  Health UI")]
    public HealthBar healthBar; //Health Bar in UI

    [Header("Movement Script")]
    public Movement movement; //Script responsible for player movement

    [Header("Conductor")]
    public Conductor conductor; //Conductor class responsible to finding beats in music

    private void Start()
    {
        conductor = GameObject.FindGameObjectWithTag("Conductor").GetComponent<Conductor>();
        healthBar = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<HealthBar>();
        currentHealth = maxHealth; //set current health to maxHealth
        healthBar.SetMaxHealth(maxHealth); //Update health UI
        regularColor = sprite.color; //get colour of sprite
    }
    public void TakeDamage(int damage, int maxHealth)
    {
        currentHealth -= damage; //reduce current health
        healthBar.SetHealth(currentHealth, maxHealth); //update health in UI
        FindObjectOfType<AudioManager>().Play("sHurt");
        if (currentHealth <= 0) //if no health
        {
            GM.GameOver(); //Cause a game over
        }
        else
            StartCoroutine(iFrame()); //otherwise flash to show inviniciblity
    }

    public void Heal()
    {
        //heal and update health
        if (currentHealth < maxHealth)
        {
            currentHealth += 1;
            healthBar.SetHealth(currentHealth, maxHealth);
        }
    }

    
    private void Update()
    {
        //Solves build issue which prevented some collisions, likely IgnoreLayerCollisions was remaining true on a GameOver.
       if(invincible)
            Physics2D.IgnoreLayerCollision(6, 10, true);
       else
            Physics2D.IgnoreLayerCollision(6, 10, false);

       //If the player misses twice in a row then they take Damage, this also prevents spam by adding a consequence
       if(conductor.missCount >= 2)
       {
            conductor.missCount = 0;
            TakeDamage(1, maxHealth);
        }
    }


    private IEnumerator iFrame()
    {
        //Change the sprites colour repeatedily imitating that classic super mario style
        int temp = 0;
        invincible = true; //now player has been damaged they should be invinsible 
        while (temp < numberOfFlashes)
        {
            sprite.color = flashColor; //turn to flash colour
            yield return new WaitForSeconds(flashDuration); //how long to remain that sprite colour
            sprite.color = regularColor; //revert to regular colour
            yield return new WaitForSeconds(flashDuration);
            temp++;
        }
        invincible = false; //when flashing is over they are able to be damaged again
    }
    
    
    private void DirectionofDamage(Collider2D enemy)
    {
        //Determine the direction the damage came from to knock the player away from the enemy
        int moveDir = 0;
        if (enemy.transform.position.x < transform.position.x) //enemy player coliided with is to the left
        {
            moveDir = 1;
            
        }
        else if (enemy.transform.position.x > transform.position.x) //enemy player coliided with is to the right
        {
            moveDir = -1;
        }
        movement.knockBack(moveDir); //call knockback function
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
       
        if (!invincible && other.gameObject.CompareTag("Enemy")) //whe not invincible and the object colliding with is an Enemy 
        {
            TakeDamage(1, maxHealth); //take damage function
            

            if (movement.onGround == true) //if on the ground
            {
                DirectionofDamage(other); //call function to knock the player away from enemy
            }
        }
        else if(other.gameObject.CompareTag("Reload")) //if colliding with a reload orb
        {
            Destroy(other.gameObject);
            movement.maxAmmo++; //increase max ammo
            movement.Reload(); //reload
            FindObjectOfType<AudioManager>().Play("sReload");
        }
    }
}
