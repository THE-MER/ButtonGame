using UnityEngine;

public sealed class Bullet : MonoBehaviour
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

    [SerializeField] private SpriteRenderer _spriteRenderer;

    [Space]
    [SerializeField] private float _lifeTime = 20;

    [Space]
    [SerializeField] private int _damage = 100;

    [Space]
    [SerializeField] private BulletBlast _bulletBlast;

    private float _localScale;

    private void Start()
    {
        GameObject.Destroy(gameObject, _lifeTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out IDamageable damageable))
        {
            float calculatedDamage = LocalScale * _damage;
            damageable.ApplyDamage((int)calculatedDamage);
        }

        GameObject.Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if(_bulletBlast != null)
        {
            BulletBlast bulletBlast = Instantiate(_bulletBlast, transform.position, transform.rotation);
            bulletBlast.LocalScale = LocalScale;
        }
    }
}
