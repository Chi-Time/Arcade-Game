using System.Collections;
using UnityEngine;

public class PlayerController : Entity
{
    [SerializeField] private float _FireRate = 0.0f;
    [SerializeField] private PlayerProjectile _Projectile = null;

    private bool _CanFire = true;
    private Vector2 _CurrentDirection = Vector2.up;

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

        //TODO: Lock player to fixed axis based on movement.
        //TODO: Get angle of player sprite for projectile.
        //TODO: Rotate player sprite.

        if (x > 0.0f)
            Move (Vector2.right);
        else if (x < -0.0f)
            Move (Vector2.left);
        else if (y > 0.0f)
            Move (Vector2.up);
        else if (y < -0.0f)
            Move (Vector2.down);
    }

    protected override void Move (Vector2 dir)
    {
        _CurrentDirection = dir;
        _Transform.Translate (dir * Time.deltaTime * _Speed);
    }

    private IEnumerator FireProjectile ()
    {
        _CanFire = false;

        var go = (GameObject)Instantiate (_Projectile.gameObject, _Transform.position, Quaternion.identity);

        if (_CurrentDirection == Vector2.right)
            go.transform.rotation = Quaternion.Euler (0f, 0f, Random.Range (133.5f, 136.5f));
        else if (_CurrentDirection == Vector2.left)
            go.transform.rotation = Quaternion.Euler (0f, 0f, Random.Range (223.5f, 226.5f));
        else if (_CurrentDirection == Vector2.up)
            go.transform.rotation = Quaternion.Euler (0f, 0f, Random.Range(-1.5f, 1.5f));
        else if (_CurrentDirection == Vector2.down)
            go.transform.rotation = Quaternion.Euler (0f, 0f, Random.Range (88.5f, 91.5f));

        //TODO: Implement logic with bullet and possible bullet pool.

        yield return new WaitForSeconds (_FireRate);

        _CanFire = true;
    }
}
