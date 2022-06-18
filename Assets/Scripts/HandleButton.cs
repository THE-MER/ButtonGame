using UnityEngine;

public sealed class HandleButton : MonoBehaviour
{
    [SerializeField] private float _acceleration = 1000f;
    [SerializeField] private float _deceleration = 35f;
    [SerializeField] private float _rotationVelocityMagnitude = 15f;

    [Space]
    [SerializeField] private Sprite _buttonDownSprite;
    [SerializeField] private Sprite _buttonPressedSprite;
    [SerializeField] private Sprite _buttonUpSprite;

    [Space]
    [SerializeField] private Bullet _bulletPrefab;

    [Space]
    [SerializeField] private float _bulletInitialVelocityMagnitude = 2.5f;

    [Space]
    [SerializeField] private float _bulletMinimumLocalScale = 0.25f;
    [SerializeField] private float _bulletMaximumLocalScale = 1f;

    [Space]
    [SerializeField] private int _bulletInitialDamage = 100;

    [Space]
    [SerializeField] private float _funButtonRecoilVelocityMagnitude = 5f;

    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody2D;
    private FunButton _funButton;

    private float _buttonDownTime = 0.0f;
    private float _buttonUpTime = 0.0f;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        if(_funButton == null)
        {
            _funButton = GameObject.FindObjectOfType<FunButton>();
        }

        _funButton.OnButtonDown += _funButton_OnButtonDown;
        _funButton.OnButtonPressed += _funButton_OnButtonPressed;
        _funButton.OnButtonUp += _funButton_OnButtonUp;
    }

    private void _funButton_OnButtonDown(FunButton funButton)
    {
        _spriteRenderer.sprite = _buttonDownSprite;
        _buttonDownTime = Time.time;
    }

    private void _funButton_OnButtonPressed(FunButton funButton)
    {
        _spriteRenderer.sprite = _buttonPressedSprite;
    }

    private void _funButton_OnButtonUp(FunButton funButton)
    {
        _spriteRenderer.sprite = _buttonUpSprite;
        _buttonUpTime = Time.time;

        Shoot();
    }

    private void Update()
    {
        transform.rotation = Quaternion.FromToRotation(Vector2.right, _funButton.transform.position - transform.position);
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(_funButton.ButtonKeyCode))
        {
            _rigidbody2D.velocity = Vector2.MoveTowards(_rigidbody2D.velocity, Vector2.zero, _deceleration * Time.fixedDeltaTime);
        }
        else
        {
            Vector3 rotationVelocity = transform.rotation * Vector2.down * _rotationVelocityMagnitude;
            _rigidbody2D.velocity = Vector2.MoveTowards(_rigidbody2D.velocity, rotationVelocity, _acceleration * Time.fixedDeltaTime);
        }
    }

    private void Shoot()
    {
        Bullet bulletInstance = Instantiate(_bulletPrefab, transform.position, transform.rotation);

        Rigidbody2D bulletRigidbody2D = bulletInstance.GetComponent<Rigidbody2D>();
        float bulletInitialImpulseMagnitude = _bulletInitialVelocityMagnitude * bulletRigidbody2D.mass;
        Vector2 bulletInitialImpulse = transform.rotation * Vector2.left * bulletInitialImpulseMagnitude;
        bulletRigidbody2D.AddForce(bulletInitialImpulse, ForceMode2D.Impulse);

        float bulletLocalScale = Mathf.Min(Mathf.Max(_bulletMinimumLocalScale, _buttonUpTime - _buttonDownTime), _bulletMaximumLocalScale);
        bulletInstance.transform.localScale = Vector3.one * bulletLocalScale;

        bulletInstance.Damage = (int)(_bulletInitialDamage * bulletLocalScale);

        Rigidbody2D funButtonRigidbody2D = _funButton.GetComponent<Rigidbody2D>();
        float funButtonRecoilImpulseMagnitude = _funButtonRecoilVelocityMagnitude * funButtonRigidbody2D.mass * (_buttonUpTime - _buttonDownTime);
        Vector2 funButtonRecoilImpulse = Vector3.Normalize(_funButton.transform.position - transform.position) * funButtonRecoilImpulseMagnitude;
        funButtonRigidbody2D.AddForce(funButtonRecoilImpulse, ForceMode2D.Impulse);
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
