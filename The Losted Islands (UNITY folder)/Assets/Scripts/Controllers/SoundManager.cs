using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

/*https://www.youtube.com/watch?v=hvVb9L59X-k&t=427s
     */

public class SoundManager : MonoBehaviour
{
    public AudioClip[] sounds; 
    public SoundArrays[] randSound;
    public AudioSource audioSrc => GetComponent<AudioSource>();

    public Slider bgVolumeSlider;
    public Slider effectsVolumeSlider;

    public float effectsVolume;
    public float bgMusicVolume;

    public int index;
    public int newIndex;

    public void Starting()
    {
        effectsVolume = effectsVolumeSlider.value;
        bgMusicVolume = bgVolumeSlider.value;
        index = Random.Range(0, randSound[0].soundArray.Length);
        audioSrc.clip = randSound[0].soundArray[index];
        audioSrc.volume = bgVolumeSlider.value;
        audioSrc.Play();
        StartCoroutine(BgSound(index));
    }
    public void PlaySound(int i = 0, bool random = false, float volume = 0.0001f, float p1 = 0.9f, float p2 = 1.1f)
    {
        AudioClip clip = random ? randSound[i].soundArray[Random.Range(0, randSound[i].soundArray.Length)] : sounds[i];
        audioSrc.pitch = Random.Range(p1, p2);
        volume = volume == 0.0001f ? effectsVolume : volume;
        audioSrc.PlayOneShot(clip, volume);
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
        audioSrc.clip = randSound[0].soundArray[index];
        audioSrc.Play();
        StartCoroutine(BgSound(index));
    }

    [System.Serializable]

    public class SoundArrays
    {
        public AudioClip[] soundArray;
    }

    public void ChangeVolume()
    {
        audioSrc.volume = bgVolumeSlider.value;
        effectsVolume = effectsVolumeSlider.value;
    }
}
