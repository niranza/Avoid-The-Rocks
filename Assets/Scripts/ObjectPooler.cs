using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public Pool pool;
    private GameObject obj;
    private bool preInstantiation;
    private int amount;
    private List<GameObject> objList = new List<GameObject>();
    private void Awake()
    {
        obj = pool.obj;
        preInstantiation = pool.preInstantiate;
        amount = pool.amount;
    }
    private void Start()
    {
        if (preInstantiation)
            for (int i = 0; i < amount; i++)
            {
                GameObject newObj = createObj(Vector3.zero, Quaternion.identity);
                newObj.SetActive(false);
            }
    }
    public GameObject getObject(Vector3 position, Quaternion rotation)
    {
        if (objList.Count != 0)
            for (int i = 0; i < objList.Count; i++)
            {
                if (!objList[i].activeInHierarchy)
                {
                    objList[i].transform.position = position;
                    objList[i].SetActive(true);
                    return objList[i];
                }
            }
        GameObject newObj = createObj(position, rotation);
        newObj.SetActive(true);
        return newObj;
    }
    private GameObject createObj(Vector3 pos, Quaternion rot)
    {
        GameObject newObj = Instantiate(obj, pos, rot);
        objList.Add(newObj);
        return newObj;
    }
}
