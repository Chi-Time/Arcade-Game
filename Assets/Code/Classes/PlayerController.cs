using System.Collections;
using UnityEngine;

public class PlayerController : Entity
{
    [SerializeField] private float _FireRate = 0.0f;
    [SerializeField] private Pool _Pool = new Pool();
    //[SerializeField] private BulletPool _Pool = new BulletPool ();

    private bool _CanFire = true;
    private Vector2 _CurrentDirection = Vector2.up;

    protected override void Initialise ()
    {
        base.Initialise ();

        _Pool.Intialise ("Bullet Pool");
    }

    private void Update ()
    {
        GetInput ();

        if (_CanFire)
            StartCoroutine ("FireProjectile");
    } 

    private void GetInput ()
    {
        float x = Input.GetAxis ("Horizontal");
        float y = Input.GetAxis ("Vertical");

        Move (_CurrentDirection);

        if (x > 0.0f)
            _CurrentDirection = Vector2.right;
        else if (x < -0.0f)
            _CurrentDirection = Vector2.left;
        else if (y > 0.0f)
            _CurrentDirection = Vector2.up;
        else if (y < -0.0f)
            _CurrentDirection = Vector2.down;
        else
            Move (Vector2.zero);

        SetRotation ();
    }

    protected override void Move (Vector2 dir)
    {
        _Rigidbody2D.velocity = dir * Time.fixedDeltaTime * _Speed;
    }

    private void SetRotation ()
    {
        if (_CurrentDirection == Vector2.right)
            _Transform.rotation = Quaternion.Euler (0f, 0f, 270f);
        else if (_CurrentDirection == Vector2.left)
            _Transform.rotation = Quaternion.Euler (0f, 0f, 90f);
        else if (_CurrentDirection == Vector2.up)
            _Transform.rotation = Quaternion.Euler (0f, 0f, 0f);
        else if (_CurrentDirection == Vector2.down)
            _Transform.rotation = Quaternion.Euler (0f, 0f, 180f);
    }

    private IEnumerator FireProjectile ()
    {
        _CanFire = false;

        var projectile = _Pool.RetrieveFromPool ();

        if (projectile)
            SetProjectileRotation (projectile.gameObject);

        yield return new WaitForSeconds (_FireRate);

        _CanFire = true;
    }

    private void SetProjectileRotation (GameObject projectile)
    {
        projectile.transform.position = _Transform.position;

        if (_CurrentDirection == Vector2.right)
            projectile.transform.rotation = Quaternion.Euler (0f, 0f, Random.Range (133.5f, 136.5f));
        else if (_CurrentDirection == Vector2.left)
            projectile.transform.rotation = Quaternion.Euler (0f, 0f, Random.Range (223.5f, 226.5f));
        else if (_CurrentDirection == Vector2.up)
            projectile.transform.rotation = Quaternion.Euler (0f, 0f, Random.Range (-1.5f, 1.5f));
        else if (_CurrentDirection == Vector2.down)
            projectile.transform.rotation = Quaternion.Euler (0f, 0f, Random.Range (88.5f, 91.5f));
    }

    private void OnCollisionEnter2D (Collision2D collision)
    {
        if (collision.gameObject.CompareTag ("Basic Enemy"))
            Health--;
    }
}
