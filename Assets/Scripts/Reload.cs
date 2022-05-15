using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reload : MonoBehaviour
{
    [Header("Movement Script")]
    public Movement movement; //Script contains ammo 

    private void Start()
    {
        movement = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player") //if colliding with a reload orb
        {
            Destroy(gameObject);
            movement.maxAmmo = movement.maxAmmo + 1; //increase max ammo
            movement.Reload(); //reload
        }
    }
    
}
