using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Entity
{
    [SerializeField] private float _FireRate = 0.0f;
    [SerializeField] private PlayerProjectile _Projectile = null;

    private bool _CanFire = true;

    private void GetInput ()
    {
        if(Input.GetButtonDown("Fire1") && _CanFire)
            StartCoroutine ("FireProjectile");

        float x = Input.GetAxis ("Horizontal");
        float y = Input.GetAxis ("Vertical");

        //TODO: Lock player to fixed axis based on movement.
        //TODO: Rotate player sprite.
        Move (new Vector2 (x, y));
    }

    protected override void Move (Vector2 dir)
    {
        _Transform.Translate (dir * Time.deltaTime * _Speed);
    }

    private IEnumerator FireProjectile ()
    {
        _CanFire = false;

        var go = (GameObject)Instantiate (_Projectile.gameObject, _Transform.position, Quaternion.identity);

        //TODO: Implement logic with bullet and possible bullet pool.

        yield return new WaitForSeconds (_FireRate);

        _CanFire = true;
    }
}
