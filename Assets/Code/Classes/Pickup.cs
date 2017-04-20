using UnityEngine;

public class Pickup : MonoBehaviour, IPoolable
{
    [Tooltip("How much is the pickup worth in terms of score.")]
    [SerializeField] private int _Value = 0;
    [Tooltip("How long before the pickup disappears from the game.")]
    [SerializeField] private float _Lifetime = 0.0f;
    [Tooltip("How fast the pickup blinks when nearly gone.")]
    [SerializeField] private float _BlinkSpeed = 0.0f;

    private bool _IsOff = false;
    private float _Halfway = 0.0f;
    private bool _IsBlinking = false;
    private float _CachedLifetime = 0.0f;
    private SpriteRenderer _SpriteRenderer = null;
    private Transform _Transform = null;
    private Pool _Pool = null;

    private void Awake ()
    {
        _SpriteRenderer = GetComponent<SpriteRenderer> ();
        _Transform = GetComponent<Transform> ();
    }

    private void Start ()
    {
        _CachedLifetime = _Lifetime;
        _Halfway = _CachedLifetime / 2;
    }

    public void SetPool (Pool pool)
    {
        _Pool = pool;
    }

    private void Update ()
    {
        CalculateLife ();
    }

    private void CalculateLife ()
    {
        _Lifetime -= Time.deltaTime;

        if (_Lifetime <= _Halfway)
        {
            if(!_IsBlinking)
            {
                _IsBlinking = true;
                InvokeRepeating ("Blink", _BlinkSpeed, _BlinkSpeed);
            }
        }

        if (_Lifetime <= 0f)
            Cull ();
    }

    private void Blink ()
    {
        var color = _SpriteRenderer.color;

        _IsOff = !_IsOff;

        if(_IsOff)
            _SpriteRenderer.color = new Color (color.r, color.g, color.b, 0.0f);
        else
            _SpriteRenderer.color = new Color (color.r, color.g, color.b, 1.0f);
    }

    public void Cull ()
    {
        _IsBlinking = false;
        CancelInvoke ("Blink");
        _Lifetime = _CachedLifetime;
        _Pool.ReturnToPool (this.gameObject);
    }
}
