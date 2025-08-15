using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip bgmClip;             // BGM 클립
    [SerializeField] private AudioClip[] sfxClips;          // SFX 클립

    private AudioSource bgmSource;                          
    private List<AudioSource> sfxSource = new();            
    private int sfxPoolSize = 5;

    // 효과음 종류
    public enum Sfx { Button, GameOver, Pop };

    private static AudioManager instance;
    public static AudioManager Instance => instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        InitAudioSources();
    }
    
    private void Start()
    {
        PlayBGM();
    }

    private void InitAudioSources()
    {
        // BGM
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmSource = bgmObject.AddComponent<AudioSource>();

        // SFX
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;

        for(int i = 0;i<sfxPoolSize;i++)
        {
            AudioSource source = sfxObject.AddComponent<AudioSource>();
            sfxSource.Add(source);
        }
    }

    private void PlayBGM()
    {
        bgmSource.clip = bgmClip;
        bgmSource.volume = 0.7f;
        bgmSource.Play();
    }

    // 효과음 실행
    public void PlaySFX(Sfx type)
    {
        switch(type)
        {
            case Sfx.Button:
                FindEmptyAudioSourceAndPlaySfx(0);
                break;
            case Sfx.GameOver:
                FindEmptyAudioSourceAndPlaySfx(1);
                break;
            case Sfx.Pop:
                FindEmptyAudioSourceAndPlaySfx(2);
                break;
        }

        void FindEmptyAudioSourceAndPlaySfx(int clipNumber)
        {
            for (int index = 0; index < sfxPoolSize; index++)
            {
                if (sfxSource[index].isPlaying)
                    continue;

                sfxSource[index].clip = sfxClips[clipNumber];
                sfxSource[index].volume = 0.7f;
                sfxSource[index].Play();

                StartCoroutine(CleanupClipAfterPlay(sfxSource[index]));
                break;
            }
        }
        
    }

    // 사운드 출력 후 AudioSource 정리
    private IEnumerator CleanupClipAfterPlay(AudioSource source)
    {
        yield return new WaitWhile(() => source.isPlaying);

        source.clip = null;
    }

}
