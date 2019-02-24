using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource efxSource;
    public AudioSource musicSource;

    public static SoundManager instance = null;

    public float lowPitchEange = 0.95f;
    public float HighPitchEange = 1.05f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySingle(AudioClip clip)
    {
        efxSource.clip = clip;
        efxSource.Play();
    }

    public void RandomizeSfx(params AudioClip[] audioClips)
    {
        int randomIndex = Random.Range(0, audioClips.Length);
        float RandomPitch = Random.Range(lowPitchEange, HighPitchEange);

        efxSource.pitch = RandomPitch;
        efxSource.clip = audioClips[randomIndex];
        efxSource.Play();
    }
}
