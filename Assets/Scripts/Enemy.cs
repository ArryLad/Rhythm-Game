using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Variables")]
    public int health = 3;
    public int damage = 1;
    public int points;

    public bool canGetStomped;
    public bool canGetShot;

    [Header("Flash")]
    private SpriteRenderer sprite;
    private Color flashColor = Color.clear;
    private Color regularColor;
    private float flashTime = 0.05f;
    public GameObject deathParticle;

    [Header("Player")]
    public Player player;

    [Header("Score")]
    public ScoreKeeper scoreKeeper;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>(); //get player component from player
        sprite = gameObject.GetComponentInParent(typeof(SpriteRenderer)) as SpriteRenderer; //get parent sprite renderer
        scoreKeeper = GameObject.FindGameObjectWithTag("scoreKeeper").GetComponent<ScoreKeeper>(); //get score component from scoreKeeper
        regularColor = sprite.color; //get colour of sprite
    }

    public void TakeDamage(int damage)
    {
        health -= damage; //reduce health when taken damage
        StartCoroutine(Flash()); //call flash co-routine
        if (health <= 0) //if health 0 or lower kill then enemy
        {
            Death(); 
        }
    }

    public void Death()
    {
        player.Heal(); //heal player from enemy death
        scoreKeeper.addScoreCombo(points); //increase score & combo from killing enemy
        Instantiate(deathParticle, transform.position, Quaternion.identity); //instantiate death particle
        FindObjectOfType<AudioManager>().Play("sEnemyDeath");
        Destroy(transform.parent.gameObject); //destroy parent 
    }

    private IEnumerator Flash()
    {
        //enemies dont get invincibility but they breifly flicker to signify they have taken damage
        sprite.color = flashColor; //sprite uses flash colour 
        yield return new WaitForSeconds(flashTime); //for flashTime amount of seconds
        sprite.color = regularColor; //then returns to its regular colouring
    }
}
