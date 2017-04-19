using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class BulletPool
{
    [SerializeField] private int _PoolSize = 0;
    [SerializeField] private PlayerProjectile _ProjectilePrefab = null;
    [SerializeField] private List<PlayerProjectile> _ActivePool = new List<PlayerProjectile> ();
    [SerializeField] private List<PlayerProjectile> _InactivePool = new List<PlayerProjectile> ();

    /// <summary>
    /// Initialises the pool for later use. (Faux Constructor.)
    /// </summary>
    public void Intialise ()
    {
        GeneratePool ();
    }

    private void GeneratePool ()
    {
        for (int i = 0; i < _PoolSize; i++)
            _InactivePool.Add (SpawnProjectile (i));
    }

    /// <summary>
    /// Spawns a defaulted projectile ready and sets it up ready to be used.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    private PlayerProjectile SpawnProjectile (int index)
    {
        var go = (GameObject)Object.Instantiate (_ProjectilePrefab.gameObject, Vector3.zero, Quaternion.identity);
        go.transform.SetParent (GetPoolHolder ());
        go.name = "Projectile " + index;
        go.SetActive (false);

        var projectile = go.GetComponent<PlayerProjectile> ();
        projectile.SetReference (this);

        return projectile;
    }

    /// <summary>
    /// Retrieves the pool holder for this pool.
    /// </summary>
    /// <returns></returns>
    private Transform GetPoolHolder ()
    {
        if (!GameObject.Find ("Bullet Pool"))
            return new GameObject ("Bullet Pool").transform;

        return GameObject.Find ("Bullet Pool").transform;
    }

    /// <summary>
    /// Retrieves an object from the pool ready to go.
    /// </summary>
    /// <returns></returns>
    public PlayerProjectile RetrieveFromPool ()
    {
        if(_InactivePool.Count > 0)
        {
            var projectile = _InactivePool[0];
            _InactivePool.Remove (projectile);
            _ActivePool.Add (projectile);

            projectile.gameObject.SetActive (true);

            return projectile;
        }

        return null;
    }

    /// <summary>
    /// Returns the object back to the pool and resets it.
    /// </summary>
    /// <param name="projectile"></param>
    public void ReturnToPool (PlayerProjectile projectile)
    {
        _ActivePool.Remove (projectile);
        _InactivePool.Add (projectile);

        projectile.gameObject.SetActive (false);
        projectile.transform.position = Vector3.zero;
        projectile.transform.rotation = Quaternion.Euler (Vector3.zero);
    }
}
