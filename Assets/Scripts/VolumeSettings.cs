using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider musicSlider;

    private void start()
    {
        if(PlayerPrefs.HasKey("musicVolume"))
        {
            loadVolume();
        }
        else
        {
            setMusicVol();
        }
        
    }

    public void setMusicVol() 
    {
        float volume = musicSlider.value;
        myMixer.SetFloat("Music", Mathf.Log10(volume)*20);
    }

    private void loadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        setMusicVol();
    }

}
