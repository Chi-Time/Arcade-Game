using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class Entity : MonoBehaviour
{
    public int Health { get { return _Health; } set { UpdateHealth (value); } }

    [Tooltip("The movement speed.")]
    [SerializeField] protected float _Speed = 0.0f;
    [Tooltip("The total amount of health.")]
    [SerializeField] protected int _Health = 0;

    protected Rigidbody2D _Rigidbody2D = null;
    protected Transform _Transform = null;

    private void Awake ()
    {
        Initialise ();
    }

    protected virtual void Initialise ()
    {
        _Rigidbody2D = GetComponent<Rigidbody2D> ();
        _Transform = GetComponent<Transform> ();
    }

    private void Start ()
    {
        Setup ();
    }

    protected virtual void Setup ()
    {
        _Rigidbody2D.gravityScale = 0f;
        _Rigidbody2D.isKinematic = true;
        _Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    protected abstract void Move (Vector2 dir);

    protected virtual void UpdateHealth (int health)
    {
        _Health = health;
        
        if (_Health <= 0)
            Die ();
    }

    protected virtual void Die ()
    {
        Destroy (this.gameObject);
    }
}
