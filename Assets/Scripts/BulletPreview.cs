using UnityEngine;

public sealed class BulletPreview : MonoBehaviour
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

    private float _localScale;
}
