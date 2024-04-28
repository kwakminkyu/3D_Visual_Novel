using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] private Sound[] effectSounds;
    [SerializeField] private AudioSource[] effectPlayer;

    [SerializeField] private Sound[] bgmSounds;
    [SerializeField] private AudioSource bgmPlayer;

    [SerializeField] private AudioSource voicePlayer;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void PlayBGM(string name)
    {
        for (int i = 0; i < bgmSounds.Length; i++)
        {
            if(name == bgmSounds[i].name)
            {
                bgmPlayer.clip = bgmSounds[i].clip;
                bgmPlayer.Play();
                return;
            }
        }
        Debug.Log(name + "  BGM ������ �����ϴ�.");
    }
    
    private void StopBGM()
    {
        bgmPlayer.Stop();
    }

    private void PauseBGM()
    {
        bgmPlayer.Pause();
    }

    private void UnPauseBGM()
    {
        bgmPlayer.UnPause();
    }

    private void PlayEffectSound(string name)
    {
        for (int i = 0; i < effectSounds.Length; i++)
        {
            if (name == effectSounds[i].name)
            {
                for (int j = 0; j < effectPlayer.Length; j++)
                {
                    if (!effectPlayer[j].isPlaying)
                    {
                        effectPlayer[j].clip = effectSounds[i].clip;
                        effectPlayer[j].Play();
                        return;
                    }
                }
                Debug.Log("��� ������ҽ��� ����� �Դϴ�.");
            }
        }
        Debug.Log(name + " ȿ���� ������ �����ϴ�.");
    }

    private void StopAllEffectSound()
    {
        for (int i = 0; i < effectPlayer.Length; i++)
        {
            effectPlayer[i].Stop();
        }
    }

    private void PlayVoiceSound(string name)
    {
        AudioClip _clip = Resources.Load<AudioClip>("Sounds/Voice/" + name);
        if (_clip != null)
        {
            voicePlayer.clip = _clip;
            voicePlayer.Play();
        }
        else
        {
            Debug.Log(name + " ���̽� ������ �����ϴ�.");
        }
    }

    public void PlaySound(string name, int type)
    {
        if (type == 0)
            PlayBGM(name);
        else if (type == 1)
            PlayEffectSound(name);
        else 
            PlayVoiceSound(name);
    }
}
