using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// adding namespaces
using Unity.Netcode;
public class DoorMovement : NetworkBehaviour
{
    // Update is called once per frame

    public bool doorMoved = false;

    void Update()
    {

    }

    // this method will move the door up by 3 units
    public void MoveDoor() {
        if (!doorMoved) {
            transform.position += new Vector3(0, 3, 0);
            doorMoved = true;
        }
        else {
            transform.position -= new Vector3(0, 3, 0);
            doorMoved = false;
        }
    }
}