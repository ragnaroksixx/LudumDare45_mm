using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class ModuleAuduiSystem : MonoBehaviour
{
    public List<AudioClip> clips;
    public List<AudioSource> sources;
    public static ModuleAuduiSystem instance;

    private void Awake()
    {
        sources = new List<AudioSource>();
        foreach (AudioClip clip in clips)
        {
            AudioSource a = gameObject.AddComponent<AudioSource>();
            sources.Add(a);
            a.playOnAwake = false;
            a.clip = clip;
            a.volume = 0;
            a.loop = true;
            a.Play();
        }
        instance = this;

    }
    // Use this for initialization
    void Start()
    {

    }

    public void UpdateAudioTracks(int count)
    {
        if (count <= 0)
            count = 1;
        int i = 1;
        foreach (AudioSource source in sources)
        {
            if(i<=count)
            {
                source.DOFade(1, 1);
            }else
            {
                source.DOFade(0, 1);
            }
            i++;
        }
    }
}
