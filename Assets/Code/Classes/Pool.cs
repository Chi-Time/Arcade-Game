using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Pool
{
    [SerializeField] private int _PoolSize = 0;
    [SerializeField] private GameObject _ProjectilePrefab = null;
    [SerializeField] private List<PoolObject> _ActivePool = new List<PoolObject> ();
    [SerializeField] private List<PoolObject> _InactivePool = new List<PoolObject> ();

    private string _PoolName = "Pool";

    /// <summary>
    /// Initialises the pool for later use. (Faux Constructor.)
    /// </summary>
    public void Intialise (string poolName)
    {
        _PoolName = poolName;
        GeneratePool ();
    }

    private void GeneratePool ()
    {
        for (int i = 0; i < _PoolSize; i++)
            _InactivePool.Add (SpawnPoolObject (i));
    }

    /// <summary>
    /// Spawns a defaulted projectile ready and sets it up ready to be used.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    private PoolObject SpawnPoolObject (int index)
    {
        var go = (GameObject)Object.Instantiate (_ProjectilePrefab.gameObject, Vector3.zero, Quaternion.identity);
        go.transform.SetParent (GetPoolHolder ());
        go.name = "Projectile " + index;
        go.SetActive (false);

        var poolObj = go.GetComponent<PoolObject> ();
        poolObj.Setup (this);

        return poolObj;
    }

    /// <summary>
    /// Retrieves the pool holder for this pool.
    /// </summary>
    /// <returns></returns>
    private Transform GetPoolHolder ()
    {
        if (!GameObject.Find (_PoolName))
            return new GameObject (_PoolName).transform;

        return GameObject.Find (_PoolName).transform;
    }

    /// <summary>
    /// Retrieves an object from the pool ready to go.
    /// </summary>
    /// <returns></returns>
    public PoolObject RetrieveFromPool ()
    {
        if (_InactivePool.Count > 0)
        {
            var poolObj = _InactivePool[0];
            _InactivePool.Remove (poolObj);
            _ActivePool.Add (poolObj);

            poolObj.gameObject.SetActive (true);

            return poolObj;
        }

        return null;
    }

    /// <summary>
    /// Returns the object back to the pool and resets it.
    /// </summary>
    /// <param name="poolObj"></param>
    public void ReturnToPool (PoolObject poolObj)
    {
        _ActivePool.Remove (poolObj);
        _InactivePool.Add (poolObj);

        poolObj.gameObject.SetActive (false);
        poolObj.transform.position = Vector3.zero;
        poolObj.transform.rotation = Quaternion.Euler (Vector3.zero);
    }
}
