using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class VibrateAndPlaySoundButton : MonoBehaviour
{
    [SerializeField] private AudioClip _audioClip;
    private AudioSource _audioSource;
    private Button _button;

    private void Awake() 
    {
        _audioSource = GetComponent<AudioSource>();
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick() 
    {
        PlaySound();
        Vibrate();
    }

    private void PlaySound()
    {
        if (_audioClip != null)
        {
            _audioSource.PlayOneShot(_audioClip);
        }
        else
        {
            Debug.LogWarning("AudioClip не установлен на " + gameObject.name);
        }
    }

    private void Vibrate()
    {
        if (VibrationController.Instance != null)
        {
            VibrationController.Instance.Vibrate(VibrationController.VibrationType.light);
        }
        else
        {
            Debug.LogWarning("VibrationController не настроен.");
        }
    }
}
