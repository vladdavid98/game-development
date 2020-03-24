using UnityEngine;
using System.Collections;

public class ScorePoint : MonoBehaviour {

	AudioSource audioS;
	bool canPlaySound;
	public bool isActive;
	//public AudioClip scoreClip;
	public int i;
	void Start()
	{
		isActive = true;
		i = 0;
		if (PlayerPrefs.GetInt ("soundTypeEnabled") == 1) {
			canPlaySound = true;
		}else canPlaySound=false;
		audioS = GetComponent<AudioSource>();
		//audioS.clip=scoreClip;
	}
	void Update()
	{
		if (PlayerPrefs.GetInt ("soundTypeEnabled") == 1) {
			canPlaySound = true;
		}else canPlaySound=false;
		if (isActive == false) {
			i++;
			//Debug.Log ("coie");
			if (i >= 200) {
				//gameObject.SetActive (true);
				isActive = true;
				//Debug.Log ("pl");
				i=0;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.tag == "Player" && isActive) {
			Score.Increment ();
			//gameObject.SetActive (false);
			isActive = false;
			//Debug.Log ("is false");
			if(canPlaySound)audioS.Play();
		}
		//if(canPlaySound)audioS.Play();
	}
}
