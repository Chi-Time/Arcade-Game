using UnityEngine;

public class PoolObject : MonoBehaviour
{
    protected Pool _Pool = null;

    public void Setup (Pool pool)
    {
        _Pool = pool;
    }

    public virtual void Cull ()
    {
        _Pool.ReturnToPool (this);
    }
}
