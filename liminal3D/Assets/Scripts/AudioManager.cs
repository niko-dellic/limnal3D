using UnityEngine.Audio;
using System;
using UnityEngine;

public class CustomAudioManager : MonoBehaviour
{

    public Sound[] sounds;

    public static CustomAudioManager instance;

    // public float shortDelay = 10f;
    public float longDelay = 20f;

    [HideInInspector]
    public float soundTimeDelay = 0;

    private bool singlePlay = true;

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
    }


    public void PlayLate()
    {
        Play("INTRO_SONG");
        Play("ROBOT");
    }

    public void playIntro()
    {
        if(Input.GetKeyDown(KeyCode.T) && singlePlay == true)
        {
            singlePlay = false; //disable the script from playing twice
            soundTimeDelay = Time.time; //get time at which i press the button

            Play("WOMBOCOMBO");
            Invoke("PlayLate", longDelay);
        }


    }

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

    void Update()
    {
        playIntro();
    }
}
