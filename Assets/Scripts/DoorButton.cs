using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// adding namespaces
using Unity.Netcode;

public class DoorButton : NetworkBehaviour
{
    public int doorLink = 0;
    private void OnCollisionEnter(Collision collision)
    {
        if (IsServer)
        {
            MoveAllDoors();
        }
        else
        {
             Debug.Log("Button is stepped on nonServer");
            MoveAllDoors();
        }
       
    }
    // private void OnCollisionExit(Collision collision)
    // {
    //     Debug.Log("Button is no longer stepped on");
    //     MoveAllDoorsServerRpc();
    // }

    private void MoveAllDoors()
{
    DoorMovement[] allDoors = FindObjectsByType<DoorMovement>(FindObjectsSortMode.None);

    foreach (DoorMovement door in allDoors)
    {
        if (door.doorGroup == doorLink)
        {
            door.ToggleDoor();
        }
    }
}


    // private void MoveAllDoors()
    // {
    //     DoorMovement[] allDoors = FindObjectsByType<DoorMovement>(FindObjectsSortMode.None);
    //     DoorMovement[] doors = new DoorMovement[allDoors.Length];
    //     int index = 0;
    //     foreach (DoorMovement door in allDoors)
    //     {
    //         if (door.doorGroup == doorLink)
    //         {
    //             if (doorLink == -1 || doorLink == -2)
    //             {
    //                 Destroy(door);
    //             }
    //             doors[index] = door;
    //             index++;
    //         }
    //     }

    //     for (int i = 0; i < index; i++)
    //     {
    //         doors[i].MoveDoor();
    //     }
    // }

    // [ServerRpc(RequireOwnership = false)]
    // private void MoveAllDoorsServerRpc()
    // {   
    //     DoorMovement[] allDoors = FindObjectsByType<DoorMovement>(FindObjectsSortMode.None);
    //     DoorMovement[] doors = new DoorMovement[allDoors.Length];
    //     int index = 0;
    //     foreach (DoorMovement door in allDoors)
    //     {
    //         if (door.doorGroup == doorLink)
    //         {
    //             if (doorLink == -1 || doorLink == -2)
    //             {
    //                 Destroy(door);
    //             }
    //             doors[index] = door;
    //             index++;
    //         }
    //     }

    //     for (int i = 0; i < index; i++)
    //     {
    //         doors[i].MoveDoor();
    //     }
    // }
}
