using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerMovement : NetworkBehaviour
{
    public float speed = 10f;
    public float rotationSpeed = 90f;
    public float distance = 8f;

    public List<Color> colors = new List<Color>();

    [SerializeField] private GameObject spawnedPrefab;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform cannon;

    [SerializeField] private AudioListener audioListener;
    [SerializeField] private Camera playerCamera;

    void Update()
    {
        if (!IsOwner)
            return;

        HandleMovement();

        if (Input.GetKeyDown(KeyCode.I))
        {
            RequestSpawnObjectServerRpc();
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            RequestDespawnObjectServerRpc();
        }

        if (Input.GetButtonDown("Fire1"))
        {
            ShootServerRpc(cannon.position, cannon.rotation);
        }
    }

    private void HandleMovement()
    {
        Vector3 moveDirection = Vector3.zero;


        if (Input.GetKey(KeyCode.W))
        {
            GetComponent<Rigidbody>().AddForce(this.playerCamera.transform.forward * distance * Time.deltaTime);
        } 
        if (Input.GetKey(KeyCode.S))        {
            GetComponent<Rigidbody>().AddForce(-this.playerCamera.transform.forward * distance * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
            transform.rotation *= Quaternion.Euler(0, rotationSpeed * Time.deltaTime, 0);
        else if (Input.GetKey(KeyCode.A))
            transform.rotation *= Quaternion.Euler(0, - rotationSpeed * Time.deltaTime, 0);

        //ONLY FOR TESTING
        if (Input.GetKey(KeyCode.Space))
            GetComponent<Rigidbody>().AddForce(Vector3.up * 100f);
    }

    public override void OnNetworkSpawn()
    {
        GetComponent<MeshRenderer>().material.color =
            colors[(int)OwnerClientId % colors.Count];

        if (!IsOwner)
            return;

        audioListener.enabled = true;
        playerCamera.enabled = true;
    }

    // ----------------------------------------------------
    // Spawning a generic networked prefab
    // ----------------------------------------------------

    [ServerRpc]
    private void RequestSpawnObjectServerRpc(ServerRpcParams rpcParams = default)
    {
        GameObject obj = Instantiate(spawnedPrefab);
        obj.GetComponent<NetworkObject>().Spawn();
    }

    [ServerRpc]
    private void RequestDespawnObjectServerRpc(ServerRpcParams rpcParams = default)
    {
        // For demo purposes: despawn the first found instance
        var netObj = FindFirstObjectByType<NetworkObject>();

        if (netObj != null && netObj.IsSpawned)
        {
            netObj.Despawn(true);
        }
    }

    // ----------------------------------------------------
    // Bullet spawning
    // ----------------------------------------------------

    [ServerRpc]
    private void ShootServerRpc(Vector3 position, Quaternion rotation,
                                ServerRpcParams rpcParams = default)
    {
        GameObject newBullet = Instantiate(bullet, position, rotation);

        var netObj = newBullet.GetComponent<NetworkObject>();
        netObj.Spawn();

        Rigidbody rb = newBullet.GetComponent<Rigidbody>();
        rb.AddForce(newBullet.transform.forward * 1500f);
    }
}
