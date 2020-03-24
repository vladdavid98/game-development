using UnityEngine;
using System.Collections;

public class GameOptions : MonoBehaviour
{


	public GameObject musicPrefab;
	public BirdMovement _BirdMovement;
	public Score _Score;
	public PanelNotificationsScript _PanelNotifications;
	public ButtonAreYouSureScript _ButtonAreYouSureScript;
	public ButtonShareScript _ButtonShareScript;
	int type;

	void Start ()
	{
		type = PlayerPrefs.GetInt ("skyType");
		//Debug.Log(type);

	}

	public IEnumerator ResetScore ()
	{
		//_ButtonAreYouSureScript
		//yield return new WaitForSeconds(5f);
		_ButtonShareScript.MakeButtonInvisibleAndNoninteractable ();
		_ButtonAreYouSureScript.MakeButtonVisibleAndInteractable ();
		_PanelNotifications.SlideInPanelNotifications ("Are you absolutely sure you want to" + "\n" + "reset all your game data?", 4.0f);
		yield return new WaitForSeconds (7.45f);
		//while (_PanelNotifications.slidin) {
		//}
		_ButtonAreYouSureScript.MakeButtonInvisibleAndNoninteractable ();

		//PlayerPrefs.SetInt("highScore",0);
		//PlayerPrefs.Save ();
		//_Score.SetHighScoreToNull ();
		yield return null;
	}

	public void ResetScorez ()
	{
		if (!_PanelNotifications.slidin) {
			StartCoroutine ("ResetScore");
		}
	}

	public void SkyTypeContinuous ()
	{
		PlayerPrefs.SetInt ("skyTypeContinuous", 1);
		PlayerPrefs.SetInt ("skyTypeStatic", 0);
		//Debug.Log("Sky type set to continuous");
	}

	public void SkyTypeStatic ()
	{
		PlayerPrefs.SetInt ("skyTypeContinuous", 0);
		PlayerPrefs.SetInt ("skyTypeStatic", 1);
		//Debug.Log("Sky type set to static");
	}

	public void FlapTypeToTheRight ()
	{
		PlayerPrefs.SetInt ("flapTypeToTheRight", 1);
		PlayerPrefs.SetInt ("flapTypeToTheLeft", 0);
		//Debug.Log("Flap type set to the right");
	}

	public void FlapTypeToTheLeft ()
	{
		PlayerPrefs.SetInt ("flapTypeToTheRight", 0);
		PlayerPrefs.SetInt ("flapTypeToTheLeft", 1);
		//Debug.Log("Flap type set to the left");
	}

	public void SoundTypeEnabled ()
	{
		if (!_PanelNotifications.slidin) {
			_PanelNotifications.SlideInPanelNotifications ("Sound Enabled", 1.25f);
		}
		PlayerPrefs.SetInt ("soundTypeEnabled", 1);
		PlayerPrefs.SetInt ("soundTypeDisabled", 0);
		//Debug.Log("Sound type set to enabled");
		GameObject music_go = GameObject.FindGameObjectWithTag ("Music");
		if (music_go == null) {
			//Debug.Log("Couldn't find an object with tag music!");
			GameObject newActor = Instantiate (musicPrefab) as GameObject;
		}
		_BirdMovement.InitialiseSoundAndVibration ();
		PlayerPrefs.Save ();
	}

	public void SoundTypeDisabled ()
	{
		if (!_PanelNotifications.slidin) {
			_PanelNotifications.SlideInPanelNotifications ("Sound Disabled", 1.25f);
		}
		PlayerPrefs.SetInt ("soundTypeEnabled", 0);
		PlayerPrefs.SetInt ("soundTypeDisabled", 1);
		//Debug.Log("Sound type set to disabled");
		GameObject music = GameObject.FindGameObjectWithTag ("Music");
		Destroy (music);
		_BirdMovement.InitialiseSoundAndVibration ();
		PlayerPrefs.Save ();

	}

	public void VibrationTypeEnabled ()
	{
		if (!_PanelNotifications.slidin) {
			_PanelNotifications.SlideInPanelNotifications ("Vibration Enabled", 1.25f);
		}
		PlayerPrefs.SetInt ("vibrationTypeEnabled", 1);
		PlayerPrefs.SetInt ("vibrationTypeDisabled", 0);
		//Debug.Log("Vibration type set to enabled");
		PlayerPrefs.Save ();
	}

	public void VibrationTypeDisabled ()
	{
		if (!_PanelNotifications.slidin) {
			_PanelNotifications.SlideInPanelNotifications ("Vibration Disabled", 1.25f);
		}
		PlayerPrefs.SetInt ("vibrationTypeEnabled", 0);
		PlayerPrefs.SetInt ("vibrationTypeDisabled", 1);
		//Debug.Log("Vibration type set to disabled");
		PlayerPrefs.Save ();
	}

	public void GameTypeOne ()
	{
		PlayerPrefs.SetInt ("gameType", 1);
		//Debug.Log ("set gametype to 1");
	}

	public void GameTypeTwo ()
	{
		PlayerPrefs.SetInt ("gameType", 2);
		//Debug.Log ("set gametype to 2");
	}
	//gametype 1 = normal start
	//gametype 2 = go directly to almostplay, no Start button











}
