using UnityEngine;
using UnityEngine.UI;
using UnityRandom = UnityEngine.Random;

public class GameHandler : MonoBehaviour
{
    public const int NUMBER_OF_SCORE_IN_ONE_LEVEL = 1000;

    public static GameHandler Linkage { get; private set; }

    public static int Score
    {
        get => Linkage._score;
        set
        {
            Linkage._score = value;
            Linkage._scoreImage.fillAmount = ((float)value % NUMBER_OF_SCORE_IN_ONE_LEVEL) / NUMBER_OF_SCORE_IN_ONE_LEVEL;
            Linkage._levelText.text = $"Level {(int)(value / NUMBER_OF_SCORE_IN_ONE_LEVEL)}";
        }
    }

    [SerializeField] private BoxSpawnArea _spawnArea;

    [Space]
    [SerializeField] private Bot _botPrefab;
    [SerializeField] private uint _botsCount = 300;

    [Space]
    [SerializeField] private float _botMinimumLocalScale = 0.5f;
    [SerializeField] private float _botMaximumLocalScale = 2.5f;

    [Space]
    [SerializeField] private Image _scoreImage;
    [SerializeField] private Text _levelText;

    private int _score;

    private void Awake()
    {
        Linkage = this;
    }

    private void Start()
    {
        Score = 0;

        for (int index = 0; index < _botsCount; index++)
        {
            Bot botInstance = Instantiate(_botPrefab, _spawnArea.GetSpawnPosition(), _spawnArea.GetSpawnRotation());
            float bodyLocalScale = UnityRandom.Range(_botMinimumLocalScale, _botMaximumLocalScale);
            botInstance.LocalScale = bodyLocalScale;
        }
    }
}
