using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mirror;

/// <summary>
/// Spawns the players in the game scene / online scene
/// </summary>
public class Spawner : NetworkBehaviour
{
    public NetworkIdentity ammoPrefab;

    /// <summary>
    /// The location where ammo will be spawned.
    /// </summary>
    [SerializeField]
    public List<GameObject> ammoSpawnLocations;

    public override void OnStartServer()
    {
       for(int i = 0; i < ammoSpawnLocations.Count; i++)
       {
            Debug.Log("Spawning ammpo");
            GameObject newAmmo = Instantiate(ammoPrefab.gameObject, ammoSpawnLocations[i].transform.position, Quaternion.identity);
            NetworkServer.Spawn(newAmmo);
        }
    }
}