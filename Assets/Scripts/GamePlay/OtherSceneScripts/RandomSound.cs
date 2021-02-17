using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSound : MonoBehaviour
{
    public AudioSource metalSound;
    private int clipIndex;
    void Start()
    {
        metalSound = GetComponent<AudioSource>();
        StartCoroutine(PlaySound());
    }

    IEnumerator PlaySound()
    {
        yield return new WaitForSeconds(Random.Range(0f, 12f));


        metalSound.Play();
        StartCoroutine(PlaySound());
    }
}
