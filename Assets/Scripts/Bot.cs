using System;
using UnityEngine;
using UnityRandom = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
public sealed class Bot : MonoBehaviour, IDamageable
{
    public SpriteRenderer SpriteRenderer => _spriteRenderer;

    public float LocalScale
    {
        get => _localScale;
        set
        {
            _localScale = value;
            transform.localScale = Vector3.one * _localScale;
        }
    }

    public int Health
    {
        get => _health;
        set
        {
            int oldHealth = _health;
            _health = value;
            OnHealthChanged?.Invoke(oldHealth, value);
        }
    }

    public event Action<int, int> OnHealthChanged;

    [SerializeField] private SpriteRenderer _spriteRenderer;

    [Space]
    [SerializeField] private float _motionVelocityMagnitude = 2.5f;

    [Space]
    [SerializeField] private int _startHealth = 100;

    [Space]
    [SerializeField] private Piece[] _piecePrefabs;
    [SerializeField] private AudioClip[] _destroyAudioClips;

    [Space]
    [SerializeField] private int _minimumNumberOfScorePerKill = 150;
    [SerializeField] private int _maximumNumberOfScorePerKill = 500;

    private FunButton _funButton;
    private Rigidbody2D _rigidbody2D;
    private AudioSource _audioSource;

    private float _localScale;
    private int _health;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        if (_funButton == null)
        {
            _funButton = GameObject.FindObjectOfType<FunButton>();
        }

        float calculatedHealth = LocalScale * _startHealth;
        Health = (int)(calculatedHealth);

        this.OnHealthChanged += Bot_OnHealthChanged;
    }

    private void FixedUpdate()
    {
        Quaternion rotation = Quaternion.FromToRotation(Vector2.right, _funButton.transform.position - transform.position);
        _rigidbody2D.velocity = rotation * Vector2.right * _motionVelocityMagnitude;
    }

    public void ApplyDamage(int damage)
    {
        Health -= damage;
    }

    private void Bot_OnHealthChanged(int oldHealth, int newHealth)
    {
        gameObject.SetActive(newHealth > 0);
    }

    private void OnDisable()
    {
        for (int index = 0; index < _piecePrefabs.Length; index++)
        {
            Piece piece = Instantiate(_piecePrefabs[index], transform.position, transform.rotation);
            piece.LocalScale = LocalScale;
        }

        _audioSource.PlayOneShot(_destroyAudioClips[UnityRandom.Range(0, _destroyAudioClips.Length)]);

        GameObject.Destroy(gameObject);

        if(GameHandler.Linkage != null)
        {
            GameHandler.Score += UnityRandom.Range(_minimumNumberOfScorePerKill, _maximumNumberOfScorePerKill);
        }
    }

    private void OnDestroy()
    {
        this.OnHealthChanged -= Bot_OnHealthChanged;
    }
}
