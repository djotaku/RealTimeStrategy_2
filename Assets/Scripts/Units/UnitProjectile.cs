using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitProjectile : NetworkBehaviour
{
    [SerializeField] private Rigidbody rb = null;
    [SerializeField] private int damateToDeal = 20;
    [SerializeField] private float destroyAfterSeconds = 5f;
    [SerializeField] private float lauchForce = 10f;

    private void Start()
    {
        rb.velocity = transform.forward * lauchForce;
    }

    public override void OnStartServer()
    {
        Invoke(nameof(DestroySelf), destroyAfterSeconds);
    }

    [ServerCallback] // don't call on the client
    private void OnTriggerEnter(Collider other) // called when this item collides with soemthing else
    {
        if(other.TryGetComponent<NetworkIdentity>(out NetworkIdentity networkIdentity))
        {
            if(networkIdentity.connectionToClient == connectionToClient) { return; }  // don't allow your units to hurt each other
        }

        if(other.TryGetComponent<Health>(out Health health))
        {
            health.DealDamage(damateToDeal);
        }

        DestroySelf(); // get rid of the projectile
    }

    [Server]
    private void DestroySelf()
    {
        NetworkServer.Destroy(gameObject);
    }
}
