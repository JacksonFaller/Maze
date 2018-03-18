using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField]
    private AudioMixer _mainAudioMixer;

    [SerializeField]
    private Dropdown _resolutionDropdown;

    private int _currentResolutionIndex;

	void Start ()
	{
        _resolutionDropdown.ClearOptions();
	    Resolution[] resolutions = Screen.resolutions;
	    List<string> resolutionStrings = new List<string>(resolutions.Length);
	    for (int i = 0; i < resolutions.Length; i++)
	    {
	        resolutionStrings.Add($"{resolutions[i].width} x {resolutions[i].height}");
	        if (resolutions[i].width == Screen.currentResolution.width &&
	            resolutions[i].height == Screen.currentResolution.height)
	            _currentResolutionIndex = i;
	    }
        _resolutionDropdown.AddOptions(resolutionStrings);
	    _resolutionDropdown.value = _currentResolutionIndex;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetVolume(float volume)
    {
        _mainAudioMixer.SetFloat("Volume", volume);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFulscreen(bool isFulscreen)
    {
        Screen.fullScreen = isFulscreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = Screen.resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
