using System;
using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
	public Slider sensivityXSlider;
	public Slider sensivityYSlider;
	public Slider soundSlider;
	private float _senX;
	private float _senY;
	private float _sound;

	private void Start()
	{
		if (Math.Abs(PlayerPrefs.GetFloat(Constants.PlayPrefsSensivityX) - sensivityXSlider.value) >= 0)
			sensivityXSlider.value = PlayerPrefs.GetFloat(Constants.PlayPrefsSensivityX);
		else
			sensivityXSlider.value = 2;

		if (Math.Abs(PlayerPrefs.GetFloat(Constants.PlayPrefsSensivityY) - sensivityYSlider.value) >= 0)
			sensivityYSlider.value = PlayerPrefs.GetFloat(Constants.PlayPrefsSensivityY);
		else
			sensivityYSlider.value = 2;

		if (Math.Abs(PlayerPrefs.GetFloat(Constants.PlayPrefsSoundVolume) - soundSlider.value) >= 0)
			soundSlider.value = PlayerPrefs.GetFloat(Constants.PlayPrefsSoundVolume);
		else
			soundSlider.value = 1;
	}

	public void OnSensivityXListener()
	{
		_senX = sensivityXSlider.value;
		PlayerPrefs.SetFloat(Constants.PlayPrefsSensivityX, _senX);
	}

	public void OnSensivityYListener()
	{
		_senY = sensivityYSlider.value;
		PlayerPrefs.SetFloat(Constants.PlayPrefsSensivityY, _senY);
	}

	public void OnSoundListener()
	{
		_sound = soundSlider.value;
		PlayerPrefs.SetFloat(Constants.PlayPrefsSoundVolume, _sound);
	}
}