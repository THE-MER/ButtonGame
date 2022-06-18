using System;
using UnityEngine;

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

    private Rigidbody2D _rigidbody2D;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
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
    }

    private void OnDestroy()
    {
        this.OnButtonDown -= _funButton_OnButtonDown;
        this.OnButtonPressed -= _funButton_OnButtonPressed;
        this.OnButtonUp -= _funButton_OnButtonUp;
    }
}
