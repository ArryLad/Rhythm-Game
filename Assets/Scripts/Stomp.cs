using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stomp : MonoBehaviour
{
    public float bounceForce;
    public Movement movement;

    void OnTriggerEnter2D(Collider2D other)
    {
        
        Enemy enemy = other.GetComponent<Enemy>();
        //If stomp collider detects collision with other containing enemy script
        //and the enemy can get stomped
        if (enemy != null && enemy.canGetStomped)
        {
            //kill enemy
            enemy.Death();
            //apply stompBounce to player
            movement.stompBounce();
        }
        
    }
    
}
