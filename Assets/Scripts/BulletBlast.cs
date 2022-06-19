using UnityEngine;

public sealed class BulletBlast : MonoBehaviour
{
    public float LocalScale
    {
        get => _localScale;
        set
        {
            _localScale = value;
            transform.localScale = Vector3.one * _localScale;
        }
    }

    [SerializeField] private float _lifeTime = 1.0f;

    private float _localScale;

    private void Start()
    {
        GameObject.Destroy(gameObject, _lifeTime);
    }
}
