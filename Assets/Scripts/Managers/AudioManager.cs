using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip bgmClip;             // BGM Ŭ��
    [SerializeField] private AudioClip[] sfxClips;          // SFX Ŭ��

    private AudioSource bgmSource;                          
    private List<AudioSource> sfxSource = new();            
    private int sfxPoolSize = 5;

    // ȿ���� ����
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

    // ȿ���� ����
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

    // ���� ��� �� AudioSource ����
    private IEnumerator CleanupClipAfterPlay(AudioSource source)
    {
        yield return new WaitWhile(() => source.isPlaying);

        source.clip = null;
    }

}
