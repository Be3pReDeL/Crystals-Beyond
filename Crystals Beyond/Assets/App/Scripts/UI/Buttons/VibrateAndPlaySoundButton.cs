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
        _audioSource.PlayOneShot(_audioClip);

        VibrationController.Instance.Vibrate(VibrationController.VibrationType.light);
    }
}
