using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectPoolManager : MonoBehaviour
{
    public static List<PooledObjectInfo> ObjectPools = new List<PooledObjectInfo>();
    public static GameObject SpawnObject(GameObject objectToSpawn, Vector3 spawnPos, Quaternion spawnRo)
    {
        PooledObjectInfo pool = null;
        foreach (PooledObjectInfo p in ObjectPools)
        {
            if (p.LookUpString == objectToSpawn.name)
            {
                pool = p;
                break;
            }
        }

        if (pool == null)
        {
            pool = new PooledObjectInfo() { LookUpString = objectToSpawn.name };
            ObjectPools.Add(pool);
        }

        GameObject spawnableObj = pool.InactiveObjects.FirstOrDefault();

        // GameObject spawnableObj = null;
        // foreach(GameObject obj in pool.InactiveObjects)
        // {
        //     if(obj != null)
        //     {
        //         spawnableObj = obj;
        //         break;
        //     }
        // }

        if(spawnableObj == null){
            spawnableObj = Instantiate(objectToSpawn, spawnPos, spawnRo);
        }
        else{
            spawnableObj.transform.position = spawnPos;
            spawnableObj.transform.rotation = spawnRo;
            pool.InactiveObjects.Remove(spawnableObj);
            spawnableObj.SetActive(true);
        }
        return spawnableObj;
    }

    public static void ReturnObjectToPool(GameObject obj)
    {
        string goName = obj.name.Substring(0, obj.name.Length - 7);
        PooledObjectInfo pool = ObjectPools.Find(p=>p.LookUpString == goName);
        if(pool == null)
        {
            Debug.LogWarning("Not polled : " + goName);
        }
        else {
            obj.SetActive(false);
            pool.InactiveObjects.Add(obj);
        }
    }
}

public class PooledObjectInfo
{
    public string LookUpString;
    public List<GameObject> InactiveObjects = new List<GameObject>();
}
