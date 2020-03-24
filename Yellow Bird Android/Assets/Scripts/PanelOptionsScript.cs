using UnityEngine;
using System.Collections;

public class PanelOptionsScript : MonoBehaviour
{

	public GameObject panelOptions;
	public BirdMovement _BirdMovement;
	public FadeOutScript _FadeOptions;
	public FadeOutScript _FadePlay;
	public FadeOutScript _FadeOutScriptTapTapImage;
	public PanelNotificationsScript _NotificationsPanel;
	public bool PanelOptionsIsDown = false;
	private Animator anim;
	private bool isPaused = false;

	void Start ()
	{
		anim = panelOptions.GetComponent<Animator> ();
		anim.enabled = false;
	}

	public void SlideInPanelOptions ()
	{
		if (_BirdMovement.timeFromStartOfTheGame >= 27) {
			//_BirdMovement.gameIsReadyToPlayAfterPauseIsOver = false;
			if (_NotificationsPanel.slidin) {
				_NotificationsPanel.SlideOutPanelNotifications ();
				_NotificationsPanel.slidin = false;
				_NotificationsPanel.timeToSlideOut = 0;
			}
			//_BirdMovement.hideAlmostPlayObject ();
			anim.enabled = true;
			anim.Play ("PanelOptionsSlideIn");
			//isPaused = true;
			_BirdMovement.MakeGameBeInPausedMode ();
			//Time.timeScale = 0;
			_FadeOptions.FadeOutObject (0.0f, 1.0f);
			_FadePlay.FadeOutObject (0.0f, 1.0f);
			_FadeOutScriptTapTapImage.FadeOutObject (0.0f, 0.6f);
			PanelOptionsIsDown = true;
		}
	}

	public void SlideOutPanelOptions ()
	{
		
		//isPaused = false;
		//_BirdMovement.gameCanRestart=true;
		anim.Play ("PanelOptionsSlideOut");
		if (!_BirdMovement.gameWasInAlmostPlayModeBeforePausing && !_BirdMovement.gameWasInPlayModeBeforePausing && !_BirdMovement.gameWasInDeathModeBeforePausing)
			_BirdMovement.MakeGameBeInMenuMode ();
		else if ((_BirdMovement.gameWasInPlayModeBeforePausing || _BirdMovement.gameWasInAlmostPlayModeBeforePausing) && !_BirdMovement.gameWasInDeathModeBeforePausing) {
			
			if (!_BirdMovement.gameWasInPlayModeBeforePausing) {
				_FadeOutScriptTapTapImage.FadeInObject (1.0f, 0.6f);
			}
			_BirdMovement.MakeGameBeInAlmostPlayMode ();

		} else if (_BirdMovement.gameWasInDeathModeBeforePausing) {
			_BirdMovement.MakeGameBeInDeathMode ();
		}
		_FadeOptions.FadeInObject (_BirdMovement.alphaOptionsBeforeFade, 1.0f);
		_FadePlay.FadeInObject (_BirdMovement.alphaPlayBeforeFade, 1.0f);
		PanelOptionsIsDown = false;
	}
}
