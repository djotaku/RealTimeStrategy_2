using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitFiring : NetworkBehaviour
{
    [SerializeField] private Targeter targeter = null;
    [SerializeField] private GameObject projectilePrefab = null;
    [SerializeField] private Transform projectileSpawnPoint = null; // so that it comes out of the barrel of the gun, not the middle of the unit
    [SerializeField] private float fireRange = 5f;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private float rotationSpeed = 20f;

    private float lastFireTime;

    [ServerCallback]
    private void Update()
    {
        Targetable target = targeter.GetTarget();

        if(target == null) { return;  }

        if(!CanFireAtTarget()) { return; }

        // if we are here, we are in range. Now we want to face

        Quaternion targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime); // using rotation speed * time.delta helps with lag

        if(Time.time > (1 / fireRate) + lastFireTime)
        {

            Quaternion projectileRotation = Quaternion.LookRotation(target.GetAimAtPoint().position - projectileSpawnPoint.position);

            GameObject projectileInstance = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileRotation);

            NetworkServer.Spawn(projectileInstance, connectionToClient);

            lastFireTime = Time.time;
        }
    }

    [Server]
    private bool CanFireAtTarget()
    {
        return (targeter.GetTarget().transform.position - transform.position).sqrMagnitude <= fireRange * fireRange;
    }    
}
