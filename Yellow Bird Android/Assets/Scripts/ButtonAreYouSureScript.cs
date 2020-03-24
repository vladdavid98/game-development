using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonAreYouSureScript : MonoBehaviour
{

	public CanvasGroup _CanvasGroupOfButton;
	public Text _TextOfPanelNotifications;
	public bool playerIsSure;
	public Score _Score;
	public PanelNotificationsScript _PanelNotifScript;

	void Start ()
	{
		playerIsSure = false;
		_CanvasGroupOfButton.blocksRaycasts = false;
		_CanvasGroupOfButton.alpha = 0;
		//_TextOfPanelNotifications.
	}

	public void MakePlayerBeSure ()
	{
		playerIsSure = true;
		PlayerPrefs.SetInt ("highScore", 0);
		PlayerPrefs.Save ();
		_Score.SetHighScoreToNull ();

		StartCoroutine ("MockPlayer");



	}

	public void MakePlayerBeNotSure ()
	{
		playerIsSure = false;
	}

	public void MakeButtonVisibleAndInteractable ()
	{
		_CanvasGroupOfButton.blocksRaycasts = true;
		_CanvasGroupOfButton.alpha = 1;
	}

	public void MakeButtonInvisibleAndNoninteractable ()
	{
		_CanvasGroupOfButton.blocksRaycasts = false;
		_CanvasGroupOfButton.alpha = 0;
	}

	public IEnumerator MockPlayer ()
	{
		
		//yield return new WaitForSeconds(5f);
		_PanelNotifScript.SlideOutPanelNotifications ();
		_PanelNotifScript.timeToSlideOut = 0;
		_PanelNotifScript.slidin = false;
		//MakeButtonInvisibleAndNoninteractable ();
		yield return new WaitForSeconds (1.0f);
		MakeButtonInvisibleAndNoninteractable ();
		_PanelNotifScript.SlideInPanelNotifications ("You asked for it. Scores wiped.", 1.5f);
	}


}
