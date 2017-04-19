using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class BulletPool
{
    [SerializeField] private int _PoolSize = 0;
    [SerializeField] private PlayerProjectile _ProjectilePrefab = null;
    [SerializeField] private List<PlayerProjectile> _ActivePool = new List<PlayerProjectile> ();
    [SerializeField] private List<PlayerProjectile> _InactivePool = new List<PlayerProjectile> ();

    public void Intialise ()
    {
        GeneratePool ();
    }

    private void GeneratePool ()
    {
        for (int i = 0; i < _PoolSize; i++)
            _InactivePool.Add (SpawnProjectile (i));
    }

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

    private Transform GetPoolHolder ()
    {
        if (!GameObject.Find ("Bullet Pool"))
            return new GameObject ("Bullet Pool").transform;

        return GameObject.Find ("Bullet Pool").transform;
    }

    private PlayerProjectile RetrieveFromPool ()
    {
        return null;
    }

    private void ReturnToPool (PlayerProjectile projectile)
    {

    }
}
