using UnityEngine;
using UnityRandom = UnityEngine.Random;

public sealed class SpawnArea : MonoBehaviour
{
    public Vector3 GetSpawnPosition()
    {
        Vector3 point = new Vector3(UnityRandom.Range(-0.5f, 0.5f), UnityRandom.Range(-0.5f, 0.5f), UnityRandom.Range(-0.5f, 0.5f));
        return transform.TransformPoint(point);
    }

    public Quaternion GetSpawnRotation()
    {
        return transform.rotation;
    }
}
