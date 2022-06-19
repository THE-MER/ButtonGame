using UnityEngine;
using UnityRandom = UnityEngine.Random;

public sealed class SphereSpawnArea : MonoBehaviour
{
    public Vector3 GetSpawnPosition()
    {
        Vector3 point = UnityRandom.insideUnitSphere;
        return transform.TransformPoint(point);
    }

    public Quaternion GetSpawnRotation()
    {
        return transform.rotation;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireSphere(Vector3.zero, 1.0f);
    }
}
