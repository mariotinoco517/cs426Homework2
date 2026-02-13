using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// adding namespaces
using Unity.Netcode;
public class DoorMovement : NetworkBehaviour
{
    // Update is called once per frame

    public bool doorMoved = false;
    public int doorGroup = 0;


    void Update()
    {

    }

    // this method will move the door up by 2 units
    public void MoveDoor() {
        if (!doorMoved) {
            transform.position += new Vector3(0, 2, 0);
            doorMoved = true;
        }
        else {
            transform.position -= new Vector3(0, 2, 0);
            doorMoved = false;
        }
    }
}