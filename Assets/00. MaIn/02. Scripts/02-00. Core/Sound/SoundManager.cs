using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SoundManager : Singleton<SoundManager>
{
    public enum EAudioType { BGM, SFX }

    [Header("#BGM")]
    public AudioClip[] BgmClips;                        // BGM 클립 여러개
    public float BgmVolume;
    private AudioSource _bgmPlayer;                     // BGM 플레이어는 단일
    private int _currentSceneIndex = 0;

    [Header("#SFX")]
    public AudioClip[] SfxClips;
    public float SfxVolume;
    public int Channels;                                // SFX 사운드 채널
    private AudioSource[] _sfxPlayers;                  // SFX는 동시에 여러개가 실행됨
    private int _channelIndex;

    public float BGMVolume
    {
        get => GetVolume(EAudioType.BGM);
        set => OnVolumeChanged(EAudioType.BGM, value);
    }

    public float SFXVolume
    {
        get => GetVolume(EAudioType.SFX);
        set => OnVolumeChanged(EAudioType.SFX, value);
    }

    protected override void Awake()
    {
        base.Awake();
        Init();
    }

    private void Start()
    {
        SceneTransitionManager.Instance.OnChangeScene += ChangeBgm;
    }

    private void Init()
    {
        // 배경음 플레이어 초기화
        GameObject bgmObject = new GameObject("BGMPlayer");
        bgmObject.transform.parent = transform;
        _bgmPlayer = bgmObject.AddComponent<AudioSource>();
        _bgmPlayer.playOnAwake = false;                          // 게임 시작 시 재생 끄기
        _bgmPlayer.loop = true;
        _bgmPlayer.volume = BgmVolume;

        // 용량 최적화
        _bgmPlayer.dopplerLevel = 0.0f;
        _bgmPlayer.reverbZoneMix = 0.0f;
        //bgmPlayer.clip = bgmClips;

        // 효과음 플레이어 초기화
        GameObject sfxParentObject = new GameObject("SFXPlayers");
        sfxParentObject.transform.parent = transform;
        _sfxPlayers = new AudioSource[Channels];

        for (int idx = 0; idx < _sfxPlayers.Length; idx++)
        {
            GameObject sfxObject = new GameObject("SFXPlayer");
            sfxObject.transform.parent = sfxParentObject.transform;
            _sfxPlayers[idx] = sfxObject.AddComponent<AudioSource>();
            _sfxPlayers[idx].playOnAwake = false;
            _sfxPlayers[idx].volume = SfxVolume;
            _sfxPlayers[idx].dopplerLevel = 0.0f;
            _sfxPlayers[idx].reverbZoneMix = 0.0f;
        }

        BgmVolume = 1.0f - PlayerPrefs.GetFloat("BGM_Volume");           // default 값이 0이기 때문에 1.0f - value로 저장
        SfxVolume = 1.0f - PlayerPrefs.GetFloat("Effect_Volume");
    }

    private void ChangeBgm()
    {
        if (_currentSceneIndex >= (int)EBgmType.Count) return;

        StopBgm();
        StopAllSfx();
        PlayBgm((EBgmType)_currentSceneIndex);
    }

    // BGM 사용을 위한 함수
    public void PlayBgm(EBgmType bgm)
    {
        if (_bgmPlayer == null) return;
        _bgmPlayer.clip = BgmClips[(int)bgm];
        _bgmPlayer.Play();
        _currentSceneIndex++;
    }

    public void StopBgm()
    {
        if (_bgmPlayer != null) _bgmPlayer.Stop();
    }

    // 효과음 사용을 위한 함수
    public void PlaySfx(ESfxType sfx)
    {
        // 쉬고 있는 하나의 sfxPlayer에게 clip을 할당하고 실행
        for (int idx = 0; idx < _sfxPlayers.Length; idx++)
        {
            int loopIndex = (idx + _channelIndex) % _sfxPlayers.Length;    // 채널 개수만큼 순회하도록 채널인덱스 변수 활용

            if (_sfxPlayers[loopIndex].isPlaying) continue;               // 진행 중인 sfxPlayer는 쭉 진행

            _channelIndex = loopIndex;
            _sfxPlayers[loopIndex].clip = SfxClips[(int)sfx];
            _sfxPlayers[loopIndex].spatialBlend = 0f;
            _sfxPlayers[loopIndex].dopplerLevel = 0f;
            _sfxPlayers[loopIndex].Play();
            break;
        }
    }

    public void PlaySfx(ESfxType sfx, Vector3 position)
    {
        // 쉬고 있는 하나의 sfxPlayer에게 clip을 할당하고 실행
        for (int idx = 0; idx < _sfxPlayers.Length; idx++)
        {
            int loopIndex = (idx + _channelIndex) % _sfxPlayers.Length;    // 채널 개수만큼 순회하도록 채널인덱스 변수 활용

            if (_sfxPlayers[loopIndex].isPlaying) continue;               // 진행 중인 sfxPlayer는 쭉 진행

            _channelIndex = loopIndex;
            _sfxPlayers[loopIndex].clip = SfxClips[(int)sfx];
            _sfxPlayers[loopIndex].gameObject.transform.position = position;
            _sfxPlayers[loopIndex].spatialBlend = 1f;
            _sfxPlayers[loopIndex].dopplerLevel = 1f;
            _sfxPlayers[loopIndex].Play();
            break;
        }
    }

    private void StopAllSfx()
    {
        for (int idx = 0; idx < _sfxPlayers.Length; idx++)
        {
            int loopIndex = (idx + _channelIndex) % _sfxPlayers.Length;    // 채널 개수만큼 순회하도록 채널인덱스 변수 활용

            if (_sfxPlayers[loopIndex].isPlaying) continue;               // 진행 중인 sfxPlayer는 쭉 진행

            _channelIndex = loopIndex;
            _sfxPlayers[loopIndex].Stop();
            _sfxPlayers[loopIndex].clip = null;
            break;
        }
    }

    public void OnChangedBGMVolume(float value)
    {
        BGMVolume = value;
        _bgmPlayer.volume = BGMVolume;
    }

    public float GetVolume(EAudioType type)
    {
        return type == EAudioType.BGM ? _bgmPlayer.volume : _sfxPlayers[0].volume;
    }

    public void OnVolumeChanged(EAudioType type, float value)
    {
        PlayerPrefs.SetFloat(type == EAudioType.BGM ? "BGM_Volume" : "SFX_Volume", 1.0f - value);

        if (type == EAudioType.BGM)
        {
            _bgmPlayer.volume = value;
        }
        else
        {
            foreach (var player in _sfxPlayers)
            {
                player.volume = value;
            }
        }
    }
}