using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinZone : MonoBehaviour
{
    public GameManager GM;
    private void OnTriggerEnter2D(Collider2D other)
    {
        //If the player touches the WinZone call the victory function 
        if(other.gameObject.tag == "Player")
        {
            GM.Victory();
        }
    }
}
