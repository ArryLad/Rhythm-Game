using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;
    [SerializeField] float offset;

    void LateUpdate()
    {
        //Camera follows player in late updatye to not compete with player moving in Fixed Update
        //Camera position is the same as players + an offset but the X remains independant (camera should be unaffected by player's x)
        transform.position = new Vector3(transform.position.x, player.position.y + offset, -100);
    }
}
