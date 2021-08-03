using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Trap Spawner", menuName = "Pool")]
public class Pool : ScriptableObject
{
    public GameObject obj;
    public bool preInstantiate = true;
    public int amount;
}
