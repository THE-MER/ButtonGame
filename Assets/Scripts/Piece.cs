using UnityEngine;

public class Piece : MonoBehaviour
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
    [SerializeField] private float _lifeTime = 10f;

    [Space]
    [SerializeField] private Color _endColor = Color.white;

    private float _localScale;

    private void Start()
    {
        GameObject.Destroy(gameObject, _lifeTime);
    }

    private void Update()
    {
        if (SpriteRenderer.color != _endColor)
        {
            SpriteRenderer.color = Color.Lerp(SpriteRenderer.color, _endColor, Time.deltaTime * 1 / _lifeTime);
        }
    }
}
