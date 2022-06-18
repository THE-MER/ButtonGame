using UnityEngine;

public sealed class Bullet : MonoBehaviour
{
    public int Damage
    {
        get => _damageInternalValue;
        set => _damageInternalValue = value;
    }

    [SerializeField] private float _lifeTime = 20;

    private int _damageInternalValue;

    private void Start()
    {
        GameObject.Destroy(gameObject, _lifeTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.TryGetComponent(out IDamageable damageable))
        {
            damageable.ApplyDamage(Damage);
            GameObject.Destroy(gameObject);
        }
    }
}
