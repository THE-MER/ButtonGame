using UnityEngine;

public class GameHandler : MonoBehaviour
{
    [SerializeField] private SpawnArea _spawnArea;

    [Space]
    [SerializeField] private Bot _botPrefab;
    [SerializeField] private uint _botsCount;

    private void Start()
    {
        for (int index = 0; index < _botsCount; index++)
        {
            Bot botInstance = Instantiate(_botPrefab, _spawnArea.GetSpawnPosition(), _spawnArea.GetSpawnRotation());
        }
    }
}
