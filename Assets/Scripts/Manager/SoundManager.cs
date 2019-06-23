using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager manager = null;
    public AudioSource Audio {
        get {
            return audioSource;
        }
    }
    AudioSource audioSource;


    private void Awake() {
        if(manager == null) {
            manager = this;
            DontDestroyOnLoad(gameObject);
        } else if(manager != this) {
            Destroy(gameObject);
        }
    }

    public bool IsPlaying {
        get {
            return audioSource.isPlaying;
        }
    }

    public float PassedTime {
        get {
            return audioSource.time;
        }
    }

    private void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayMusic(AudioClip clip) {
        audioSource.clip = clip;
        audioSource.Play();
    }

    public void PlayMusicByPath(string path) {
        AudioClip clip = Resources.Load<AudioClip>("Audio/" + path);
        audioSource.clip = clip;
        audioSource.Play();
    }

    public void PauseMusic() {
        audioSource.Pause();
    }

    public void StopMusic() {
        audioSource.Stop();
    }
}
