using UnityEngine;
using UnityRandom = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
public sealed class FunButtonAudio : MonoBehaviour
{
    [SerializeField] private AudioClip[] _pressedAudioClips;

    private AudioSource _audioSource;
    private FunButton _funButton;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        if (_funButton == null)
        {
            _funButton = GameObject.FindObjectOfType<FunButton>();
        }

        _funButton.OnButtonUp += _funButton_OnButtonUp;
    }

    private void _funButton_OnButtonUp(FunButton funButton)
    {
        _audioSource.PlayOneShot(_pressedAudioClips[UnityRandom.Range(0, _pressedAudioClips.Length)]);
    }

    private void OnDestroy()
    {
        if (_funButton != null)
        {
            _funButton.OnButtonUp -= _funButton_OnButtonUp;
        }
    }
}
