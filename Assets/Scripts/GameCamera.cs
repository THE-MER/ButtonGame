using UnityEngine;

public sealed class GameCamera : MonoBehaviour
{
    public static GameCamera Linkage { get; private set; }

    public static Camera Camera => Linkage._camera;

    public static FunButton FunButton { get; private set; }

    [SerializeField] private Camera _camera;

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
        transform.position = FunButton.transform.position;
    }
}
