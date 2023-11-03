using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils 
{
   public static Vector3 GetRandomSpawnPoint()
    {
        return new Vector3(Random.Range(-20, 20), 1, Random.Range(-20, 20));
    }

    public static void SetLayerOfChildren(Transform model,int layerNo)
    {
        foreach(Transform child in model.GetComponentInChildren<Transform>(true))
        {
            child.gameObject.layer = layerNo;
        }
    }
}
