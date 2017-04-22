using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Pool
{
    public List<GameObject> ActivePool { get { return _ActivePool; } }
    public List<GameObject> InactivePool { get { return _InactivePool; } }

    [SerializeField] private int _PoolSize = 0;
    [SerializeField] private GameObject[] _Prefabs = null;
    [SerializeField] private List<GameObject> _ActivePool = new List<GameObject> ();
    [SerializeField] private List<GameObject> _InactivePool = new List<GameObject> ();

    private string _PoolName = "Pool";
    private string _ObjectName = "Object";

    /// <summary>
    /// Initialises the pool for later use. (Faux Constructor.)
    /// </summary>
    public void Intialise (string poolName, string objectName)
    {
        _PoolName = poolName;
        _ObjectName = objectName;

        GeneratePool ();
    }

    private void GeneratePool ()
    {
        for (int i = 0; i < _PoolSize; i++)
        {
            for (int j = 0; j < _Prefabs.Length; j++)
                _InactivePool.Add (SpawnPoolObject (_Prefabs[j], i));
        }
    }

    /// <summary>
    /// Spawns a defaulted projectile ready and sets it up ready to be used.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    private GameObject SpawnPoolObject (GameObject obj, int index)
    {
        var go = (GameObject)Object.Instantiate (obj, Vector3.zero, Quaternion.identity);
        go.transform.SetParent (GetPoolHolder ());
        go.name = _ObjectName + ": " + index;
        go.SetActive (false);

        var poolObj = go.GetComponent<IPoolable> ();
        poolObj.SetPool (this);

        return go;
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
    public GameObject RetrieveFromPool ()
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
    public void ReturnToPool (GameObject poolObj)
    {
        _ActivePool.Remove (poolObj);
        _InactivePool.Add (poolObj);

        poolObj.gameObject.SetActive (false);
        poolObj.transform.position = Vector3.zero;
        poolObj.transform.rotation = Quaternion.Euler (Vector3.zero);
    }
}
