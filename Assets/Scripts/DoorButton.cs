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
        Debug.Log("Button is stepped on");
        MoveAllDoorsServerRpc();
    }
    private void OnCollisionExit(Collision collision)
    {
        Debug.Log("Button is no longer stepped on");
        MoveAllDoorsServerRpc();
    }

    [ServerRpc]
    private void MoveAllDoorsServerRpc()
    {
        DoorMovement[] allDoors = FindObjectsByType<DoorMovement>(FindObjectsSortMode.None);
        DoorMovement[] doors = new DoorMovement[allDoors.Length];
        int index = 0;
        foreach (DoorMovement door in allDoors)
        {
            if (door.doorGroup == doorLink) {
                doors[index] = door;
                index++;
            }
        }

        foreach (DoorMovement door in doors)
        {
            door.MoveDoor();
        }
    }
}
