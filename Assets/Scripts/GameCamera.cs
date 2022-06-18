using UnityEngine;

public sealed class GameCamera : MonoBehaviour
{
    public static GameCamera Linkage { get; private set; }

    public static FunButton FunButton { get; private set; }

    private void Awake()
    {
        Linkage = this;
    }

    private void Start()
    {
        if (FunButton == null)
        {
            FunButton = GameObject.FindObjectOfType<FunButton>();
        }
    }

    private void LateUpdate()
    {
        transform.position = new Vector3(FunButton.transform.position.x, FunButton.transform.position.y, transform.position.z);
    }
}
