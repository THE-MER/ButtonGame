using UnityEngine;

public sealed class ScoreParticle : MonoBehaviour
{
    public TextMesh ScoreTextMesh => _scoreTextMesh;

    [SerializeField] private float _lifeTime = 2.0f;

    [Space]
    [SerializeField] private TextMesh _scoreTextMesh;

    private void Start()
    {
        GameObject.Destroy(gameObject, _lifeTime);
    }
}
