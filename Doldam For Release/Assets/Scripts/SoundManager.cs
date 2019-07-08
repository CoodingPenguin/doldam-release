using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public static SoundManager instance = null;

    public AudioSource efxSource;   // 효과음 AudioSource
    public AudioSource btnSource;   // 버튼음 AudioSource
    public AudioSource bgmSource;   // 배경음 AudioSource

    public float lowPitchRange = 0.95f;
    public float highPitchRange = 1.05f;


    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// 효과음을 한 번 재생
    /// </summary>
    /// <param name="clip"></param>
    public void PlaySingleForEfx(AudioClip clip)
    {
        efxSource.PlayOneShot(clip);
    }

    /// <summary>
    /// 버튼음을 한 번 재생
    /// </summary>
    /// <param name="clip"></param>
    public void PlaySingleForBtn(AudioClip clip)
    {
        btnSource.PlayOneShot(clip);
    }

    /// <summary>
    /// 효과음의 피치를 랜덤으로 하여 매번 소리를 다르게 재생
    /// </summary>
    /// <param name="clips"></param>
    public void RandomizeSfx(params AudioClip[] clips)
    {
        int randomIndex = Random.Range(0, clips.Length);
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);
        efxSource.pitch = randomPitch;

        efxSource.PlayOneShot(clips[randomIndex]);
    }

    /// <summary>
    /// 배경음 재생
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="loop"></param>
    public void PlayBgm(AudioClip clip, bool loop = true)
    {
        bgmSource.clip = clip;
        bgmSource.loop = loop;
        bgmSource.Play();
    }
}
