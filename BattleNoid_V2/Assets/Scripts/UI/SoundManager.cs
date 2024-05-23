using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public AudioSource Sound;
    // Start is called before the first frame update
    

    // Update is called once per frame
    public void SetSoundMusic(float Volume)
    {
        Sound.volume = Volume;
    }
}
