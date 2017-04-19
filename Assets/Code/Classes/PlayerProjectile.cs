using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Rigidbody2D))]
public class PlayerProjectile : MonoBehaviour
{
    [Tooltip("The speed of the projectile's movement.")]
    [SerializeField] private float _Speed = 0.0f;
    [Tooltip("The length of time before the bullet is culled.")]
    [SerializeField] private float _Lifetime = 0.0f;

    private Rigidbody2D _Rigidbody2D = null;
    private Transform _Transform = null;
    private BulletPool _Pool = null;

    private void Awake ()
    {
        Initialise ();
    }

    private void Initialise ()
    {
        _Rigidbody2D = GetComponent<Rigidbody2D> ();
        _Transform = GetComponent<Transform> ();
    }

    public void SetReference (BulletPool pool)
    {
        _Pool = pool;
    }

    private void Start ()
    {
        Setup ();
    }

    private void Setup ()
    {
        _Rigidbody2D.gravityScale = 0f;
        _Rigidbody2D.isKinematic = true;
        _Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;

        Invoke ("Cull", _Lifetime);
    }

    private void Update ()
    {
        Move ();
    }

    private void Move ()
    {
        _Transform.Translate (_Transform.up * Time.deltaTime * _Speed);
    }

    private void OnTriggerEnter2D (Collider2D other)
    {
        //TODO: Implement collision logic with enemies and walls.
        if (!other.CompareTag ("Player"))
            Cull ();
    }

    private void Cull ()
    {
        //TOOD: Destroy for now, consider pool eventually.
        Destroy (this.gameObject);
    } 
}
