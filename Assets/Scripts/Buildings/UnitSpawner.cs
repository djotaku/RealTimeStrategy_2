using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitSpawner : NetworkBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject unitPrefab = null;
    [SerializeField] private Transform unitSpawnPoint = null;

    #region Server

    [Command]private void CmdSpawnUnit()
    {
        GameObject unitInstance = Instantiate(unitPrefab, unitSpawnPoint.position, unitSpawnPoint.rotation);

        NetworkServer.Spawn(unitInstance, connectionToClient); // connectionToClient lets the spawned unit belong to the player
    
    }

    #endregion

    #region Client
    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button != PointerEventData.InputButton.Left) { return;  } // if you don't click with left button - do nothing!

        if(!hasAuthority) { return;  } // if you click where you don't own, don't spawn a unit!

        CmdSpawnUnit();

        throw new System.NotImplementedException();
    }


    #endregion
}
