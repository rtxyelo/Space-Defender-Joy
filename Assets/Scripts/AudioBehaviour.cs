using System;
using System.Collections;
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
    private AudioSource lazerSound;

    [SerializeField]
    private AudioSource enemyDeathSound; 
    
    [SerializeField]
    private AudioSource enemyShotSound;

    [SerializeField]
    private AudioSource enemyShootingSound;

    [SerializeField]
    private AudioSource asteroidDestroyedSound;

    [SerializeField]
    private AudioSource shotSound;

    [SerializeField]
    private AudioSource winSound;

    [SerializeField]
    private AudioSource loseSound;

    private bool sound = true;
    private bool music = true;

    private readonly string _musicVolumeKey = "MusicVolumeKey";
    private readonly string _soundKey = "SoundKey";
    private readonly string _musicKey = "MusicKey";

    /// <summary>
    /// Initializes the volume settings based on PlayerPrefs when the script is started.
    /// </summary>
    private void Start()
    {
        if (!PlayerPrefs.HasKey(_soundKey))
            PlayerPrefs.SetInt(_soundKey, Convert.ToInt32(true));

        if (!PlayerPrefs.HasKey(_musicKey))
            PlayerPrefs.SetInt(_musicKey, Convert.ToInt32(true));

        sound = Convert.ToBoolean(PlayerPrefs.GetInt(_soundKey, Convert.ToInt32(true)));
        music = Convert.ToBoolean(PlayerPrefs.GetInt(_musicKey, Convert.ToInt32(true)));

        if (!PlayerPrefs.HasKey(_musicVolumeKey))
        {
            PlayerPrefs.SetFloat(_musicVolumeKey, 1f);
            if (_volumeSlider != null)
                _volumeSlider.value = PlayerPrefs.GetFloat(_musicVolumeKey);
        }
        else if (_volumeSlider != null)
        {
            _volumeSlider.value = PlayerPrefs.GetFloat(_musicVolumeKey);
        }

        if (_music != null && music)
        {
            _music.volume = PlayerPrefs.GetFloat(_musicVolumeKey);
            _music.Play();
        }

        if (winSound != null && music)
            winSound.volume = PlayerPrefs.GetFloat(_musicVolumeKey, 1f);

        if (loseSound != null && music)
            loseSound.volume = PlayerPrefs.GetFloat(_musicVolumeKey, 1f);
    }

    public void MusicOn()
    {
        Debug.Log("Music ON");

        music = true;
        PlayerPrefs.SetInt(_musicKey, Convert.ToInt32(music));

        if (_music != null && !_music.isPlaying)
            _music.Play();
    }

    public void MusicOff()
    {
        Debug.Log("Music OFF");

        music = false;
        PlayerPrefs.SetInt(_musicKey, Convert.ToInt32(music));

        if (_music != null && _music.isPlaying)
            _music.Stop();
    }

    public void SoundOn()
    {
        Debug.Log("Sound ON");

        sound = true;
        PlayerPrefs.SetInt(_soundKey, Convert.ToInt32(sound));
    }

    public void SoundOff()
    {
        Debug.Log("Sound OFF");

        sound = false;
        PlayerPrefs.SetInt(_soundKey, Convert.ToInt32(sound));

        if (_btnSound != null && _btnSound.isPlaying)
            _btnSound.Stop();
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
        if (damageSound != null && sound)
        {
            damageSound.volume = PlayerPrefs.GetFloat(_musicVolumeKey);
            damageSound.Play();
        }
    }

    public void PlayShotSound()
    {
        if (shotSound != null && sound)
        {
            shotSound.volume = PlayerPrefs.GetFloat(_musicVolumeKey);
            shotSound.Play();
        }
    }

    public void PlayLazerSound()
    {
        if (lazerSound != null && sound)
        {
            lazerSound.volume = PlayerPrefs.GetFloat(_musicVolumeKey);
            lazerSound.Play();
        }
    }

    public void PlayEnemyDeathSound()
    {
        if (enemyDeathSound != null && sound)
        {
            enemyDeathSound.volume = PlayerPrefs.GetFloat(_musicVolumeKey);
            enemyDeathSound.Play();
        }
    }

    public void PlayEnemyShotSound()
    {
        if (enemyShotSound != null && sound)
        {
            enemyShotSound.volume = PlayerPrefs.GetFloat(_musicVolumeKey);
            enemyShotSound.Play();
        }
    }

    public void PlayEnemyShootingSound()
    {
        if (enemyShootingSound != null && sound)
        {
            enemyShootingSound.volume = PlayerPrefs.GetFloat(_musicVolumeKey);
            enemyShootingSound.Play();
        }
    }

    public void PlayeAsteroidDestroyedSound()
    {
        if (asteroidDestroyedSound != null && sound)
        {
            asteroidDestroyedSound.volume = PlayerPrefs.GetFloat(_musicVolumeKey);
            asteroidDestroyedSound.Play();
        }
    }

    public void PlayButtonSound()
    {
        if (sound && _btnSound != null)
        {
            _btnSound.volume = PlayerPrefs.GetFloat(_musicVolumeKey);
            _btnSound.Play();
        }

    }

    public void GameWin()
    {
        if (music)
        {
            _music.Stop();
            winSound.Play();
            //_music.PlayDelayed(2f);
        }
    }
    public void GameLose()
    {
        if (music)
        {
            _music.Stop();
            loseSound.Play();
            //_music.PlayDelayed(2.2f);
        }
    }

    public static class FadeAudioSource
    {
        public static IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
        {
            float currentTime = 0;
            float start = audioSource.volume;
            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
                yield return null;
            }
            yield break;
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
}