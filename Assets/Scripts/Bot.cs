using System;
using UnityEngine;
using UnityRandom = UnityEngine.Random;

public sealed class Bot : MonoBehaviour, IDamageable
{
    public int Health
    {
        get => _healthInternalValue;
        set
        {
            int oldHealth = _healthInternalValue;
            _healthInternalValue = value;
            OnHealthChanged?.Invoke(oldHealth, value);
        }
    }

    public event Action<int, int> OnHealthChanged;

    [SerializeField] private float _bodyMinimumLocalScale;
    [SerializeField] private float _bodyMaximumLocalScale;

    [Space]
    [SerializeField] private float _motionVelocityMagnitude = 2.5f;

    [Space]
    [SerializeField] private int _initialHealth = 100;

    private Rigidbody2D _rigidbody2D;
    private FunButton _funButton;

    private int _healthInternalValue;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        if (_funButton == null)
        {
            _funButton = GameObject.FindObjectOfType<FunButton>();
        }

        this.OnHealthChanged += Bot_OnHealthChanged;

        float bodyLocalScale = UnityRandom.Range(_bodyMinimumLocalScale, _bodyMaximumLocalScale);
        transform.localScale = Vector3.one * bodyLocalScale;

        Health = (int)(_initialHealth * bodyLocalScale);
    }

    private void Update()
    {
        transform.rotation = Quaternion.FromToRotation(Vector2.right, _funButton.transform.position - transform.position);
    }

    private void FixedUpdate()
    {
        _rigidbody2D.velocity = transform.rotation * Vector2.right * _motionVelocityMagnitude;
    }

    public void ApplyDamage(int damage)
    {
        if (isActiveAndEnabled)
        {
            Health -= damage;
        }
    }

    private void Bot_OnHealthChanged(int oldHealth, int newHealth)
    {
        if (newHealth <= 0)
        {
            GameObject.Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        this.OnHealthChanged -= Bot_OnHealthChanged;
    }
}
