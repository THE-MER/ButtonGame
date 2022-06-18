using UnityEngine;
using UnityRandom = UnityEngine.Random;

public class GameHandler : MonoBehaviour
{
    [SerializeField] private SpawnArea _spawnArea;

    [Space]
    [SerializeField] private Bot _botPrefab;
    [SerializeField] private uint _botsCount = 300;

    [Space]
    [SerializeField] private float _botMinimumLocalScale = 0.5f;
    [SerializeField] private float _botMaximumLocalScale = 2.5f;


    private void Start()
    {
        for (int index = 0; index < _botsCount; index++)
        {
            Bot botInstance = Instantiate(_botPrefab, _spawnArea.GetSpawnPosition(), _spawnArea.GetSpawnRotation());
            float bodyLocalScale = UnityRandom.Range(_botMinimumLocalScale, _botMaximumLocalScale);
            botInstance.LocalScale = bodyLocalScale;
        }
    }
}
