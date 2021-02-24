using UnityEngine.Audio;
using System;
using UnityEngine;

public class CustomAudioManager : MonoBehaviour
{

    public Sound[] sounds;

    public static CustomAudioManager instance;

    public float shortDelay = 10f;
    public float longDelay = 30f;

    // Start is called before the first frame update
    void Awake() 
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            // s.source.AudioMixer = s.AudioMixer;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch; 
            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = s.AudioMixer;
        }
    }

    void Start ()
    {
        Play("SoundScape");
        // Play("INTRO_SONG");
        // Play("ROBOT");

        Invoke("PlayDelayed", shortDelay);
        Invoke("PlayLate", longDelay);

    }

    public void PlayDelayed()
    {
        Play("WOMBOCOMBO");
    }
    public void PlayLate()
    {
        Play("INTRO_SONG");
        Play("ROBOT");
    }

    // Update is called once per frame
    public void Play (string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Play();
    }
}
