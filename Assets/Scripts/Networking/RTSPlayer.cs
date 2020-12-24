using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTSPlayer : NetworkBehaviour
{
    [SerializeField] private List<Unit> myUnits = new List<Unit>();

    public List<Unit> GetMyUnits()
    {
        return myUnits;
    }

    #region Server
    public override void OnStartServer()
    {
        Unit.ServerOnUnitSpawned += ServerHandleUnitsSpawned; // subscribing to an event
        Unit.ServerOnUnitDespawned += ServerHandleUnitsDespawned; // subscribing to an event
    }

    public override void OnStopServer()
    {
        Unit.ServerOnUnitSpawned -= ServerHandleUnitsSpawned; // unsubscribing to an event
        Unit.ServerOnUnitDespawned -= ServerHandleUnitsDespawned; // unsubscribing to an event
    }

    private void ServerHandleUnitsSpawned(Unit unit)
    {
        if(unit.connectionToClient.connectionId != connectionToClient.connectionId) { return;  }

        myUnits.Add(unit);
    }

    private void ServerHandleUnitsDespawned(Unit unit)
    {
        if (unit.connectionToClient.connectionId != connectionToClient.connectionId) { return; }

        myUnits.Remove(unit);
    }

    #endregion

    #region Client

    public override void OnStartAuthority()
    {
        if (NetworkServer.active) { return; } // true if this machine is running as the server
        Unit.AuthorityOnUnitSpawned += AuthorityHandleUnitsSpawned; // subscribing to an event
        Unit.AuthorityOnUnitDespawned += AuthorityHandleUnitsDespawned; // subscribing to an event
    }

    public override void OnStopClient()
    {
        if (!isClientOnly || !hasAuthority) { return; }

        Unit.AuthorityOnUnitSpawned -= AuthorityHandleUnitsSpawned; // subscribing to an event
        Unit.AuthorityOnUnitDespawned -= AuthorityHandleUnitsDespawned; // subscribing to an event
    }

    private void AuthorityHandleUnitsSpawned(Unit unit)
    {
        myUnits.Add(unit);
    }

    private void AuthorityHandleUnitsDespawned(Unit unit)
    {
        myUnits.Remove(unit);
    }


    #endregion

}
