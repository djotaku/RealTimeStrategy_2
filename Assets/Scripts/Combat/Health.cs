using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : NetworkBehaviour
{
    [SerializeField] private int maxHealth = 100;

    [SyncVar(hook = nameof(HandleHealthUpdated))]
    private int currentHealth;

    public event Action ServerOnDie;

    public event Action<int, int> ClientOnHealthUpdated;

    #region Server

    public override void OnStartServer()
    {
        currentHealth = maxHealth;
    }

    [Server]
    public void DealDamage(int damageAmount)
    {
        if(currentHealth == 0) { return;  } // only die once!

        currentHealth = Mathf.Max(currentHealth - damageAmount, 0); // keeps it from going negative and saves a bunch of code
        
        if(currentHealth != 0) { return; }

        ServerOnDie?.Invoke(); // this raises an event that can be listened to elsewhere so each object can have a different death

        Debug.Log("We died.");
    }

    #endregion

    #region Cleint

    private void HandleHealthUpdated(int oldHealth, int newHealth)
    {
        ClientOnHealthUpdated?.Invoke(newHealth, maxHealth); // this allows the Healthdisplay to know the health has udpated and it should update the UI. Or anything else that wants to listen for the event
    }

    #endregion

}
