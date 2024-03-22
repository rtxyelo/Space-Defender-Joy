using UnityEngine;

public class AudioSourceGroup : MonoBehaviour
{
    public AudioSource[] typingSources;
    private int nextTypeSource = 0;
    private readonly string _musicVolumeKey = "MusicVolumeKey";

    public void PlayFromNextSource(AudioClip clip) {
        AudioSource nextSource = typingSources[nextTypeSource];

        nextSource.clip = clip;

        if (PlayerPrefs.GetFloat(_musicVolumeKey, 0.2f) * 2f > 1f)
            nextSource.volume = 1f;
        else
            nextSource.volume = PlayerPrefs.GetFloat(_musicVolumeKey, 0.2f);

        nextSource.Play();

        nextTypeSource = (nextTypeSource + 1) % typingSources.Length;
    }
}
