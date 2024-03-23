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



public class MusicEssence : MonoBehaviour
{
    public static MusicEssence Instance;

    private AudioSource _musicSource;

    private string _volumekey = "Volumekey";
    public float VolumeValue { get; private set; }

    private void Awake()
    {
        _musicSource = GetComponent<AudioSource>();

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            return;
        }


        Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        PlayerPrefs.SetFloat(_volumekey, VolumeValue);
    }

    public void Initialize()
    {
        if (!PlayerPrefs.HasKey(_volumekey))
            PlayerPrefs.SetFloat(_volumekey, 0);

        VolumeValue = PlayerPrefs.GetFloat(_volumekey);
        SetVolume(VolumeValue);
    }

    public void SetVolume(float volumeValue)
    {
        VolumeValue = volumeValue;
        _musicSource.volume = volumeValue;
    }
}

public class MusicDisplay : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    public void Initialize()
    {
        _slider.value = MusicEssence.Instance.VolumeValue;
        _slider.onValueChanged.AddListener(MusicEssence.Instance.SetVolume);
    }
    private void OnDestroy()
    {
        _slider.onValueChanged.RemoveAllListeners();
    }
}