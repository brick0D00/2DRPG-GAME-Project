using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{   
    public static AudioManager instance;

    [SerializeField] private float minDistanceToSound;//虽然正常应该给每个物体都装个组件的
    [SerializeField] private AudioSource[] sfx;
    [SerializeField] private AudioSource[] bgm;

    public bool isPlayingBGM;
    public int bgmIndex;
    public bool canPlayBGM;
    public bool canPlaySFX;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(instance.gameObject);
        }
        else
        {
            instance = this;
        }
        Invoke("AllowPlayBGM", 0.1f);
        Invoke("AllowPlaySFX", 0.1f);
    }
    private void Update()
    {
        if (!isPlayingBGM)
        {
            StopAllBGM();
        }
        else
        {
            if (!bgm[bgmIndex].isPlaying)
            {
                PlayBGM(bgmIndex);
            }
        }
    }
    public void PlayRandomBGM()
    {
        bgmIndex=Random.Range(0, bgm.Length);
        PlayBGM(bgmIndex);
    }
    public void PlaySFX(int _sfxIndex,Transform _source)
    {
        //TODO:播放特效音
        if (!canPlaySFX) { return; }
        //if (sfx[_sfxIndex].isPlaying) { return; }

        Transform playerTr=PlayerManager.instance.transform;

        //超出距离
        if (_source!=null && Vector2.Distance(_source.position, playerTr.position) > minDistanceToSound)
        {   
            return;
        }

        //让每次播放的音效没那么单调
        sfx[bgmIndex].pitch = Random.Range(.85f, 1.1f);
        sfx[_sfxIndex].Play();
    }
    public void StopSFX(int _sfxIndex)
    {   
        //TODO:停止特效音
        sfx[_sfxIndex].Stop();
    }

    public void StopSFXWithTime(int _index)
    {   
        //TODO:逐步减少音量
        StartCoroutine(IE_DecreaseVolume(_index));
    }
    private IEnumerator IE_DecreaseVolume(int _index)
    {
        float originVplume=sfx[_index].volume;

        while (sfx[_index].volume > 0.1f)
        {
            sfx[bgmIndex].volume*=0.8f;
            yield return new WaitForSeconds(.2f);

            if(sfx[_index].volume <= 0.1f)
            {
                StopSFX(_index);
                sfx[_index].volume = originVplume;
            }
        }
    }
    public void PlayBGM(int _bgmIndex)
    {
        //TODO:播放BGM
        if (!canPlayBGM)
        {
            return ;
        }

        bgmIndex=_bgmIndex;

        StopAllBGM();
        bgm[_bgmIndex].Play();
    }
    public void StopAllBGM()
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            bgm[i].Stop();
        }
    }

    public void AllowPlayBGM()
    {
        canPlayBGM = true;
    }
    public void AllowPlaySFX()
    {
        canPlaySFX = true;
    }
}
