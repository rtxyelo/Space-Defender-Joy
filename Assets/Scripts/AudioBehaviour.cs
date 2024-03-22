using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Manages the volume control for background music in the game.
/// </summary>
public class AudioBehaviour : MonoBehaviour
{
    [SerializeField] private AudioSource _music;
    [SerializeField] private AudioSource _btnSound;
    [SerializeField] private Slider _volumeSlider;

    [SerializeField]
    private AudioSource damageSound;    
    
    [SerializeField]
    private AudioSource shotSound; 
    
    [SerializeField]
    private AudioSource winSound;

    [SerializeField]
    private AudioSource loseSound;

    private readonly string _musicVolumeKey = "MusicVolumeKey";

    //private GameBehaviour gameBehaviour;

    /// <summary>
    /// Initializes the volume settings based on PlayerPrefs when the script is started.
    /// </summary>
    private void Start()
    {
        if (!PlayerPrefs.HasKey(_musicVolumeKey))
        {
            PlayerPrefs.SetFloat(_musicVolumeKey, 0.2f);
            if (_volumeSlider != null)
                _volumeSlider.value = PlayerPrefs.GetFloat(_musicVolumeKey);
        }
        else if (_volumeSlider != null)
        {
            _volumeSlider.value = PlayerPrefs.GetFloat(_musicVolumeKey);
        }

        if (_music != null)
            _music.volume = PlayerPrefs.GetFloat(_musicVolumeKey);

        if (winSound != null)
            winSound.volume = PlayerPrefs.GetFloat(_musicVolumeKey, 0.2f);
        
        if (loseSound != null)
            loseSound.volume = PlayerPrefs.GetFloat(_musicVolumeKey, 0.2f);

        //gameBehaviour = FindObjectOfType<GameBehaviour>();
        //if (gameBehaviour != null)
        //{
        //    gameBehaviour.GameWinEvent += GameWin;
        //    gameBehaviour.GameLoseEvent += GameLose;
        //}
    }

    /// <summary>
    /// Updates the volume settings based on the slider value.
    /// </summary>
    public void OnSliderChange()
    {
        PlayerPrefs.SetFloat(_musicVolumeKey, _volumeSlider.value);
        _music.volume = PlayerPrefs.GetFloat(_musicVolumeKey);
        if (_volumeSlider != null)
        {
            _volumeSlider.value = PlayerPrefs.GetFloat(_musicVolumeKey);
        }
    }

    public void PlayDamageSound()
    {
        damageSound.volume = PlayerPrefs.GetFloat(_musicVolumeKey);
        Debug.Log("Current volume " + damageSound.volume);
        damageSound.Play();
    }

    public void PlayShotSound()
    {
        shotSound.volume = PlayerPrefs.GetFloat(_musicVolumeKey);
        shotSound.Play();
    }

    public void PlayButtonSound()
    {
        _btnSound.volume = PlayerPrefs.GetFloat(_musicVolumeKey);
        _btnSound.Play();
    }

    private void GameWin()
    {
        _music.Stop();
        winSound.Play();
        _music.PlayDelayed(2f);
    }
    private void GameLose()
    {
        _music.Stop();
        loseSound.Play();
        _music.PlayDelayed(2.2f);
    }
}