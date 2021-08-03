using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static T findComponent<T>(string objectName)
    {
        GameObject obj = GameObject.Find(objectName);
        if (obj != null)
            return obj.GetComponent<T>();
        return default;
    }
}
