using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonShareScript : MonoBehaviour
{

	public CanvasGroup _CanvasGroupOfButton;
	public Text _TextOfPanelNotifications;
	public bool playerIsSure;
	public Score _Score;
	public PanelNotificationsScript _PanelNotifScript;



	void Start ()
	{
		_CanvasGroupOfButton.blocksRaycasts = false;
		_CanvasGroupOfButton.alpha = 0;
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
}
