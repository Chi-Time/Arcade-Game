using UnityEngine;
using System.Collections;

enum States
{
    Patrol,
    Attack
};

public class BasicEnemy : Entity
{
    [SerializeField] private float _ChangeRate = 0.5f;

    private Vector3 _CurrentDirection = Vector3.zero;
    private States _CurrentState = States.Attack;
    private Transform _Target = null;
    private bool _CanChange = true;

    protected override void Setup ()
    {
        base.Setup ();

        _Target = GameObject.FindGameObjectWithTag ("Player").transform;
    }

    private void Update ()
    {
        if (_CurrentState == States.Patrol)
            Patrol ();

        Attack ();
    }

    private void Patrol ()
    {
        if (_CanChange)
            StartCoroutine ("ChangeDirection");
    }

    private IEnumerator ChangeDirection ()
    {
        _CanChange = false;

        SelectDirection ();

        Move (_CurrentDirection);

        yield return new WaitForSeconds (_ChangeRate);

        StopAllCoroutines ();
        _CanChange = true;
    }

    private void SelectDirection ()
    {
        var index = Random.Range (1, 8);

        switch (index)
        {
            case 1:
                _CurrentDirection = Vector3.right;
                break;
            case 2:
                _CurrentDirection = Vector3.left;
                break;
            case 3:
                _CurrentDirection = Vector3.up;
                break;
            case 4:
                _CurrentDirection = Vector3.down;
                break;
            case 5:
                _CurrentDirection = new Vector3 (1, 1);
                break;
            case 6:
                _CurrentDirection = new Vector3 (-1, 1);
                break;
            case 7:
                _CurrentDirection = new Vector3 (-1, -1);
                break;
            case 8:
                _CurrentDirection = new Vector3 (1, -1);
                break;
        }
    }

    private void Attack ()
    {
        if(_Target)
        {
            float step = _Speed * Time.deltaTime;
            _Rigidbody2D.MovePosition (Vector3.MoveTowards (_Transform.position, _Target.position, step));
        }

        _CurrentState = States.Patrol;
    }

    protected override void Move (Vector2 dir)
    {
        _Rigidbody2D.velocity = Vector3.zero;
        _Rigidbody2D.AddForce (_CurrentDirection * _Speed, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D (Collider2D other)
    {
        if (other.CompareTag ("Projectile"))
            Health--;
    }
}
