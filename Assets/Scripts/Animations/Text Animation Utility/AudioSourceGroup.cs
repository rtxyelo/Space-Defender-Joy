using UnityEngine;

public class AudioSourceGroup : MonoBehaviour
{
    public AudioSource[] typingSources;
    private int nextTypeSource = 0;
    //private readonly string _musicVolumeKey = "MusicVolumeKey";
    private readonly string _soundKey = "SoundKey";

    public void PlayFromNextSource(AudioClip clip) {
        AudioSource nextSource = typingSources[nextTypeSource];

        nextSource.clip = clip;

        if (PlayerPrefs.GetInt(_soundKey, 0) == 1)
            nextSource.volume = 1f;
        else
            nextSource.volume = 0f;

        nextSource.Play();

        nextTypeSource = (nextTypeSource + 1) % typingSources.Length;
    }
}
