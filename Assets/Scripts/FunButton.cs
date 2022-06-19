using System;
using UnityEngine;
using UnityRandom = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
public sealed class FunButton : MonoBehaviour
{
    public KeyCode ButtonKeyCode => _buttonKeyCode;
    public SpriteRenderer SpriteRenderer => _spriteRenderer;

    public event Action<FunButton> OnButtonDown;
    public event Action<FunButton> OnButtonPressed;
    public event Action<FunButton> OnButtonUp;

    [SerializeField] private KeyCode _buttonKeyCode;

    [Space]
    [SerializeField] private SpriteRenderer _spriteRenderer;

    [Space]
    [SerializeField] private Sprite _buttonDownSprite;
    [SerializeField] private Sprite _buttonPressedSprite;
    [SerializeField] private Sprite _buttonUpSprite;

    [Space]
    [SerializeField] private int _minimumNumberOfScorePerPress = 10;
    [SerializeField] private int _maximumNumberOfScorePerPress = 50;

    [Space]
    [SerializeField] private AudioClip[] _pressedAudioClips;

    [Space]
    [SerializeField] private ScoreParticle _scoreParticlePrefab;
    [SerializeField] private SphereSpawnArea _scoreSphereSpawnArea;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        this.OnButtonDown += _funButton_OnButtonDown;
        this.OnButtonPressed += _funButton_OnButtonPressed;
        this.OnButtonUp += _funButton_OnButtonUp;
    }

    private void Update()
    {
        if (Input.GetKeyDown(_buttonKeyCode))
        {
            OnButtonDown?.Invoke(this);
        }

        if (Input.GetKey(_buttonKeyCode))
        {
            OnButtonPressed?.Invoke(this);
        }

        if (Input.GetKeyUp(_buttonKeyCode))
        {
            OnButtonUp?.Invoke(this);
        }
    }

    private void _funButton_OnButtonDown(FunButton funButton)
    {
        _spriteRenderer.sprite = _buttonDownSprite;
    }

    private void _funButton_OnButtonPressed(FunButton funButton)
    {
        _spriteRenderer.sprite = _buttonPressedSprite;
    }

    private void _funButton_OnButtonUp(FunButton funButton)
    {
        _spriteRenderer.sprite = _buttonUpSprite;

        _audioSource.PlayOneShot(_pressedAudioClips[UnityRandom.Range(0, _pressedAudioClips.Length)]);

        if (GameHandler.Linkage != null)
        {
            int score = UnityRandom.Range(_minimumNumberOfScorePerPress, _maximumNumberOfScorePerPress);

            if(_scoreParticlePrefab != null)
            {
                ScoreParticle scoreParticle = Instantiate(_scoreParticlePrefab, _scoreSphereSpawnArea.GetSpawnPosition(), _scoreSphereSpawnArea.GetSpawnRotation());
                scoreParticle.ScoreTextMesh.text = $"+{score}";
            }

            GameHandler.Score += score;
        }
    }

    private void OnDestroy()
    {
        this.OnButtonDown -= _funButton_OnButtonDown;
        this.OnButtonPressed -= _funButton_OnButtonPressed;
        this.OnButtonUp -= _funButton_OnButtonUp;
    }
}
