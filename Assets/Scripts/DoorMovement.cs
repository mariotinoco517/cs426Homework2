// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// // adding namespaces
// using Unity.Netcode;
// public class DoorMovement : NetworkBehaviour
// {
//     // Update is called once per frame

//     public bool doorMoved = false;
//     public int doorGroup = 0;


//     void Update()
//     {

//     }

//     // this method will move the door up by 2 units
//     // [ServerRpc(RequireOwnership = false)]
//     public void MoveDoor() {
//         if (!doorMoved) {
//             transform.position += new Vector3(0, 10, 0);
//             doorMoved = true;
//         }
//         else {
//             transform.position -= new Vector3(0, 10, 0);
//             doorMoved = false;
//         }
//     }
// }

using Unity.Netcode;
using UnityEngine;

public class DoorMovement : NetworkBehaviour
{
    public int doorGroup = 0;

    private Vector3 closedPosition;
    private Vector3 openPosition;

    private NetworkVariable<bool> isOpen = new NetworkVariable<bool>();

    private void Start()
    {
        closedPosition = transform.position;
        openPosition = closedPosition + new Vector3(0, 10, 0);

        isOpen.OnValueChanged += OnDoorStateChanged;
    }

    private void OnDoorStateChanged(bool previous, bool current)
    {
        transform.position = current ? openPosition : closedPosition;
    }

    public void ToggleDoor()
    {
        if (!IsServer) return;

        isOpen.Value = !isOpen.Value;
    }
}
