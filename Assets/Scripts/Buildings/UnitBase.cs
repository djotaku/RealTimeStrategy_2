using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBase : NetworkBehaviour
{
    [SerializeField] private Health health = null;

    public static event Action<UnitBase> ServerOnBaseSpawned; // list will store remaining players
    public static event Action<UnitBase> ServerOnBaseDespawned; // if base destroyed, it is removed from list of bases. When only one, then game's done.

    #region Server
    public override void OnStartServer()
    {
        health.ServerOnDie += ServerHandleDie; // subscribing to the event

        ServerOnBaseSpawned?.Invoke(this);
    }


    public override void OnStopServer()
    {
        health.ServerOnDie -= ServerHandleDie; // unsubscribing from the event

        ServerOnBaseDespawned?.Invoke(this);
    }

    [Server]
    private void ServerHandleDie()
    {
        NetworkServer.Destroy(gameObject);
    }
    #endregion

    #region Client

    #endregion
}
