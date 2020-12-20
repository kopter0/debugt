using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{

    public AudioClip[] audios;
    private AudioSource source;
    private int cur_idx;
    private void Start()
    {
        source = GetComponent<AudioSource>();
        cur_idx = -1;
        Random.InitState((int)System.DateTime.Now.Ticks);
    }

    // Update is called once per frame
    void Update()
    {
        if (!source.isPlaying)
        {
            int next = cur_idx;
            while (next == cur_idx)
                next = Random.Range(0, audios.Length);
            cur_idx = next;
            source.clip = audios[cur_idx];
            source.Play();

        }
    }

}
