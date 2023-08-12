using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class SpawnIndexer
{
    public Dictionary<GameObject, bool> spawnInfo = new Dictionary<GameObject, bool>();

    public bool this[GameObject g]
    {
        set => spawnInfo[g] = value;
    }
}

public class SpawnController : MonoBehaviour
{
    public GameObject EnemyPrefab;
    public GameObject[] spawnPoints;
    public LayerMask playerMask;

    SpawnIndexer si = new SpawnIndexer();

    private void Start()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag("spawnpoint");

        foreach (var spawn in spawnPoints)
        {
            si.spawnInfo.Add(spawn, false);
        }
    }

    private void Update()
    {
        foreach (KeyValuePair<GameObject, bool> spawn in si.spawnInfo.ToList())
        {
            if (!spawn.Value)
            {
                Collider[] colliders = Physics.OverlapSphere(spawn.Key.transform.position, 5, playerMask);

                if (colliders != null && colliders.Length > 0 )
                {
                    Instantiate(EnemyPrefab, spawn.Key.transform.position, Quaternion.identity);
                    si[spawn.Key] = true;

                    Debug.Log("Enemy spawned");
                }
            }
        }
    }
}
