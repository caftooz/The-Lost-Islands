using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/*https://www.youtube.com/watch?v=hvVb9L59X-k&t=427s
     */

public class SoundManager : MonoBehaviour
{
    public AudioClip[] sounds; 
    public SoundArrays[] randSound;

    public AudioSource bgAudioSrc;
    public AudioSource efAudioSrc;

    public Slider bgVolumeSlider;
    public Slider effectsVolumeSlider;

    public float effectsVolume;
    public float bgMusicVolume;

    public int index;
    public int newIndex;

    public void Starting()
    {
        efAudioSrc.volume = effectsVolumeSlider.value;
        bgAudioSrc.volume = bgVolumeSlider.value;
        index = Random.Range(0, randSound[0].soundArray.Length);
        bgAudioSrc.clip = randSound[0].soundArray[index];
        bgAudioSrc.volume = bgVolumeSlider.value;
        bgAudioSrc.Play();
        StartCoroutine(BgSound(index));
    }
    public void PlaySound(int ind)
    {
        efAudioSrc.PlayOneShot(sounds[ind], effectsVolume);
    }

    public void PlayRandomSound(int ind)
    {
        efAudioSrc.PlayOneShot(randSound[ind].soundArray[Random.Range(0, randSound[ind].soundArray.Count())], effectsVolume);
    }
    IEnumerator BgSound(int _index)
    {
        yield return new WaitForSeconds(randSound[0].soundArray[index].length);
        newIndex = Random.Range(0, randSound[0].soundArray.Length);
        while(newIndex == index)
        {
            newIndex = Random.Range(0, randSound[0].soundArray.Length);
        }
        index = newIndex;
        bgAudioSrc.clip = randSound[0].soundArray[index];
        bgAudioSrc.Play();
        StartCoroutine(BgSound(index));
    }

    public void ChangeVolume()
        {
            bgAudioSrc.volume = bgVolumeSlider.value;
            effectsVolume = effectsVolumeSlider.value;
        }
    }

[System.Serializable]

public class SoundArrays
{
        public AudioClip[] soundArray;
}