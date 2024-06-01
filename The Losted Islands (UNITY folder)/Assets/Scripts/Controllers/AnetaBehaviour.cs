using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AnetaBehaviour : MonoBehaviour
{
    public Animator animator;
    public bool isCluttering = false;
    public AudioSource anetaVoice;
    void Start()
    {
        StartCoroutine(Clatter(Random.Range(30, 60)));
    }

    // Update is called once per frame
    IEnumerator Clatter(float time)
    {
        yield return new WaitForSeconds(time);
        if (!isCluttering)
        {
            animator.SetFloat("toClatter", 1);
            anetaVoice.mute = false;
            anetaVoice.Play();
            isCluttering = true;
            StartCoroutine(Clatter(2.4f));
        }
        else
        {
            animator.SetFloat("toClatter", 0);
            isCluttering = false;
            StartCoroutine(Clatter(Random.Range(30, 60)));
        }
    }

}
