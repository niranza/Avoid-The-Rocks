using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Trap Spawner", menuName = "Trap Spawner")]
public class TrapSpawner : ScriptableObject
{
    public Vector3 offSetFromPlayer;
    public int minBlocksBetweenTraps;
    public int maxBlocksBetweenTraps;
    public int trapsAhead;
    public bool spawnObject = false;
}
