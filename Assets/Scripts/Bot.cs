using System;
using UnityEngine;
using UnityRandom = UnityEngine.Random;

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
    [SerializeField] private float _pieceVelocityMagnitude = 2.5f;

    [Space]
    [SerializeField] private int _minimumNumberOfScorePerKill = 150;
    [SerializeField] private int _maximumNumberOfScorePerKill = 500;

    [Space]
    [SerializeField] private ScoreParticle _scoreParticlePrefab;
    [SerializeField] private SphereSpawnArea _scoreSphereSpawnArea;

    private FunButton _funButton;
    private Rigidbody2D _rigidbody2D;

    private float _localScale;
    private int _health;

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

            if (piece.TryGetComponent(out Rigidbody2D pieceRigidbody2D))
            {
                float rangeX = UnityRandom.Range(-1.0f, 1.0f);
                float rangeY = UnityRandom.Range(-1.0f, 1.0f);

                Vector2 pieceVelocity = new Vector2(rangeX, rangeY).normalized * _pieceVelocityMagnitude;
                pieceRigidbody2D.AddForce(pieceVelocity * pieceRigidbody2D.mass, ForceMode2D.Impulse);
            }
        }

        if (GameHandler.Linkage != null)
        {
            int score = UnityRandom.Range(_minimumNumberOfScorePerKill, _maximumNumberOfScorePerKill);

            if (_scoreParticlePrefab != null)
            {
                ScoreParticle scoreParticle = Instantiate(_scoreParticlePrefab, _scoreSphereSpawnArea.GetSpawnPosition(), _scoreSphereSpawnArea.GetSpawnRotation());
                scoreParticle.ScoreTextMesh.text = $"+{score}";
            }

            GameHandler.Score += score;
        }

        GameObject.Destroy(gameObject);
    }

    private void OnDestroy()
    {
        this.OnHealthChanged -= Bot_OnHealthChanged;
    }
}
