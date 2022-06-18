using UnityEngine;

public sealed class HandleButton : MonoBehaviour
{
    public SpriteRenderer SpriteRenderer => _spriteRenderer;

    [SerializeField] private float _acceleration = 1000f;
    [SerializeField] private float _deceleration = 35f;
    [SerializeField] private float _rotationVelocityMagnitude = 15f;

    [Space]
    [SerializeField] private SpriteRenderer _spriteRenderer;

    [Space]
    [SerializeField] private Sprite _buttonDownSprite;
    [SerializeField] private Sprite _buttonPressedSprite;
    [SerializeField] private Sprite _buttonUpSprite;

    [Space]
    [SerializeField] private Bullet _bulletPrefab;

    [Space]
    [SerializeField] private float _bulletInitialVelocityMagnitude = 2.5f;

    [Space]
    [SerializeField] private float _shootCameraShakeDuration = 1.0f;
    [SerializeField] private float _shootCameraShakePower = 1.0f;

    [Space]
    [SerializeField] private float _bulletMinimumLocalScale = 0.5f;
    [SerializeField] private float _bulletMaximumLocalScale = 2.0f;

    [Space]
    [SerializeField] private float _funButtonRecoilVelocityMagnitude = 7.5f;

    private Rigidbody2D _rigidbody2D;
    private FunButton _funButton;

    private float _buttonDownTime;
    private float _buttonUpTime;

    public float KeyHoldingTime => _buttonUpTime - _buttonDownTime;

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

        _funButton.OnButtonDown += _funButton_OnButtonDown;
        _funButton.OnButtonPressed += _funButton_OnButtonPressed;
        _funButton.OnButtonUp += _funButton_OnButtonUp;
    }

    private void _funButton_OnButtonDown(FunButton funButton)
    {
        _buttonDownTime = Time.time;
        _spriteRenderer.sprite = _buttonDownSprite;
    }

    private void _funButton_OnButtonPressed(FunButton funButton)
    {
        _spriteRenderer.sprite = _buttonPressedSprite;
    }

    private void _funButton_OnButtonUp(FunButton funButton)
    {
        _buttonUpTime = Time.time;
        _spriteRenderer.sprite = _buttonUpSprite;

        Shoot();
    }

    private void FixedUpdate()
    {
        Quaternion rotation = Quaternion.FromToRotation(Vector2.right, _funButton.transform.position - transform.position);

        if (Input.GetKey(_funButton.ButtonKeyCode))
        {
            _rigidbody2D.velocity = Vector2.MoveTowards(_rigidbody2D.velocity, Vector2.zero, _deceleration * Time.fixedDeltaTime);
        }
        else
        {
            Vector3 rotationVelocity = rotation * Vector2.down * _rotationVelocityMagnitude;
            _rigidbody2D.velocity = Vector2.MoveTowards(_rigidbody2D.velocity, rotationVelocity, _acceleration * Time.fixedDeltaTime);
        }
    }

    private void Shoot()
    {
        Quaternion rotation = Quaternion.FromToRotation(Vector2.right, _funButton.transform.position - transform.position);
        Bullet bulletInstance = Instantiate(_bulletPrefab, transform.position, rotation);

        float bulletLocalScale = Mathf.Min(Mathf.Max(_bulletMinimumLocalScale, KeyHoldingTime), _bulletMaximumLocalScale);
        bulletInstance.LocalScale = bulletLocalScale;

        if (bulletInstance.TryGetComponent(out Rigidbody2D bulletRigidbody2D))
        {
            float bulletInitialImpulseMagnitude = _bulletInitialVelocityMagnitude * bulletRigidbody2D.mass;
            Vector2 bulletInitialImpulse = rotation * Vector2.left * bulletInitialImpulseMagnitude;
            bulletRigidbody2D.AddForce(bulletInitialImpulse, ForceMode2D.Impulse);
        }

        CameraShake.Shake(_shootCameraShakeDuration * bulletLocalScale, _shootCameraShakePower * bulletLocalScale, CameraShake.ShakeMode.XY);

        if (_funButton.TryGetComponent(out Rigidbody2D funButtonRigidbody2D))
        {
            float funButtonRecoilImpulseMagnitude = _funButtonRecoilVelocityMagnitude * funButtonRigidbody2D.mass * KeyHoldingTime;
            Vector2 funButtonRecoilImpulse = Vector3.Normalize(_funButton.transform.position - transform.position) * funButtonRecoilImpulseMagnitude;
            funButtonRigidbody2D.AddForce(funButtonRecoilImpulse, ForceMode2D.Impulse);
        }
    }

    private void OnDestroy()
    {
        if (_funButton != null)
        {
            _funButton.OnButtonDown -= _funButton_OnButtonDown;
            _funButton.OnButtonPressed -= _funButton_OnButtonPressed;
            _funButton.OnButtonUp -= _funButton_OnButtonUp;
        }
    }
}
