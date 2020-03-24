using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PanelNotificationsScript : MonoBehaviour {

	public GameObject panelNotifications;
	private Animator anim;
	public bool slidin;
	private Text text;
	string textToTypeInBox;
	float seconds;
	public int instance;
	public float timeToSlideOut;

	public bool panelIsClosed;
	public bool panelIsOpening;
	public bool panelIsOpened;
	public bool panelIsClosing;


	void Update()
	{
		if (timeToSlideOut > 0) {
			timeToSlideOut--;
		} else if(panelIsOpened) {
			SlideOutPanelNotifications ();
			slidin = false;
		}


	}




	void Start () {
		panelIsClosed = true;
		instance = 0;
		slidin = false;
		anim = panelNotifications.GetComponent<Animator>();
		anim.enabled = false;
		text = panelNotifications.GetComponentInChildren<Text> ();
	}

	public IEnumerator SlideInPanelNotifications(){

		yield return new WaitForSeconds (0.25f);
		text.text = "" + textToTypeInBox;
		anim.enabled = true;
		panelIsOpening = true;
		panelIsClosed = false;
		anim.Play("PanelNotificationsSlideIn");
		//panelIsOpened = true;
		//Debug.Log ("1");
		//yield return new WaitForSeconds(1f);
		slidin = true;
		yield return new WaitForSeconds (1.0f);
		panelIsOpened = true;
		panelIsOpening = false;
		/*yield return new WaitForSeconds (seconds);

		panelIsClosing = true;
		panelIsOpened = false;

		SlideOutPanelNotifications();

		slidin = false;
		yield return new WaitForSeconds(1f);
		panelIsClosed = true;
		panelIsClosing = false;*/

		//Debug.Log ("2");
		yield return null;



	}

	public void SlideInPanelNotifications(string x,float y)
	{
		if (!slidin) {
			textToTypeInBox = x;
			seconds = y;
			timeToSlideOut = y * 100f;
			StartCoroutine ("SlideInPanelNotifications");
		}
	}
	public void SlideInPanelNotificationsForcefully(string x,float y)
	{
		textToTypeInBox = x;
		seconds = y;
		timeToSlideOut = y * 100f;
		StartCoroutine ("SlideInPanelNotifications");
	}

	public IEnumerator slideOutPanelNotifications(){
		anim.enabled = true;
		anim.Play("PanelNotificationsSlideOut");
		panelIsClosing = true;
		panelIsOpened = false;
		yield return new WaitForSeconds(1f);
		panelIsClosed = true;
		panelIsClosing = false;

	}

	public void SlideOutPanelNotifications()
	{
		StartCoroutine ("slideOutPanelNotifications");
	}


	


}