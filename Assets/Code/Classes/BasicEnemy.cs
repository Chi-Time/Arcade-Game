using UnityEngine;
using System.Collections;

enum States
{
    Patrol,
    Attack
};

public class BasicEnemy : Entity, IPoolable
{
    [SerializeField] private float _ChangeRate = 0.5f;
    [SerializeField] private float _FadeSpeed = .25f;

    private Vector3 _CurrentDirection = Vector3.zero;
    private SpriteRenderer _SpriteRenderer = null;
    private States _CurrentState = States.Attack;
    private Transform _Target = null;
    private bool _CanChange = true;
    private Pool _Pool = null;

    protected override void Initialise ()
    {
        base.Initialise ();

        _SpriteRenderer = GetComponent<SpriteRenderer> ();
        _Target = GameObject.FindGameObjectWithTag ("Player").transform;
    }

    protected override void Setup ()
    {
        base.Setup ();
        SelectState ();
    }

    public void SetPool (Pool pool)
    {
        _Pool = pool;
    }

    private void SelectState ()
    {
        int index = Random.Range (0, 2);

        switch (index)
        {
            case 0:
                _CurrentState = States.Patrol;
                break;
            case 1:
                _CurrentState = States.Attack;
                break;
        }
    } 

    private void Update ()
    {
        if (_CurrentState == States.Patrol)
            Patrol ();
        else
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
        if (_Target)
        {
            float step = _Speed * Time.deltaTime;
            _Rigidbody2D.MovePosition (Vector3.MoveTowards (_Transform.position, _Target.position, step));
        }
        else
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

    protected override void Die ()
    {
        Cull ();
    }

    public void Cull ()
    {
        var coin = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ().CoinPool.RetrieveFromPool ();
        coin.transform.position = this.transform.position;
        coin.SetActive (true);
        GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ().Kills += 1;
        gameObject.SetActive (false);
        _Pool.ReturnToPool (this.gameObject);
    }

    private void OnEnable ()
    {
        StartCoroutine (FadeTo (1.0f, _FadeSpeed));
    }

    private void OnDisable ()
    {
        StopAllCoroutines ();
        _SpriteRenderer.material.color = new Color (Color.white.r, Color.white.g, Color.white.b, 0.0f);
    }

    private IEnumerator FadeTo (float aValue, float aTime)
    {
        var t = 0.0f;
        var alpha = _SpriteRenderer.material.color.a;

        while (t < 1.0f)
        {
            t += Time.deltaTime / aTime;

            var newAlpha = new Color (1, 1, 1, Mathf.Lerp (alpha, aValue, t));
            _SpriteRenderer.material.color = newAlpha;

            yield return null;
        }
    }
}
