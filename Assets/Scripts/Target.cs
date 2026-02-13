using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// adding namespaces
using Unity.Netcode;

public class Target : NetworkBehaviour
{
    public int doorLink = 0;
    //this method is called whenever a collision is detected
    private void OnCollisionEnter(Collision collision)
    {

        // printing if collision is detected on the console
        Debug.Log("Collision Detected");

        // destroy the colliding object
        if (!collision.gameObject.CompareTag("Player"))
        {
            DestroyObjectServerRpc(collision.gameObject.GetComponent<NetworkObject>().NetworkObjectId);
            MoveAllDoorsServerRpc(); 
        }
    }

    // client can not spawn or destroy objects
    // so we need to use ServerRpc
    // we also need to add RequireOwnership = false
    // because we want to destroy the object even if the client is not the owner
    [ServerRpc(RequireOwnership = false)]
    public void DestroyObjectServerRpc(ulong networkObjectId)
    {
        if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(networkObjectId, out NetworkObject netObj))
        {
            netObj.Despawn(true);
        }
    }
    
    // ----------------------------------------------------
    // Door movement
    // ----------------------------------------------------

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