using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField]
    private AudioSource _bgmPlayer;
    [SerializeField]
    private AudioSource _sePlayer;

    private void Awake()
    {
        if (Instance == null)
        {
            //�V�[���J�ڂ��Ă��j������Ȃ��悤�ɂ���
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            //��d�ŋN������Ȃ��悤�ɂ���
            Destroy(gameObject);
        }
    }

    public void PlayBGM(AudioClip clip)
    {
        _bgmPlayer.clip = clip;
        _bgmPlayer.Play();
    }

    public void PlaySE(AudioClip clip, bool isLoop)
    {
        _sePlayer.clip = clip;
        _sePlayer.loop = isLoop;
        _sePlayer.Play();
    }

    public void StopBGM()
    {
        _bgmPlayer?.Stop();
    }

    public void StopSE()
    {
        _sePlayer?.Stop();
    }
}
