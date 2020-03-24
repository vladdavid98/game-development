using UnityEngine;
using System.Collections;

public class ButtonCreditsScript : MonoBehaviour
{

	public PanelNotificationsScript _PanelNotif;

	public void ShowCredits ()
	{
		_PanelNotif.SlideInPanelNotifications ("Programmer: David Vlad Alexandru" + "\n" + "E-mail: vladdavidapps@gmail.com", 3.0f);






	}
}
