using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inCamera : MonoBehaviour
{
    Camera mainCam;
    public bool isSeen;

    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>(); //fetch the mainCamera's camera component

    }

    void Update()
    {
        Vector3 view = mainCam.WorldToViewportPoint(transform.position); //get the view port of the camera with transform.position
        //if object is not within the view port of the camera it is not Seen
        if (view.z > 0 && view.x > 0 && view.x < 1 && view.y > 0 && view.y < 1) 
        {
            isSeen = true;
        }
        else
        {
            isSeen = false;
        }
            
    }
}
