using UnityEngine;
using System.Collections;

public class MusicPlayer : MonoBehaviour {

	static MusicPlayer instance=null;
	public AudioClip ambientClip;
	AudioSource audioS;
	bool canPlaySound;
	//GameObject music;

	void Start ()
	{
		//music=this;
		audioS = GetComponent<AudioSource>();
		audioS.clip=ambientClip;
		if (PlayerPrefs.GetInt ("soundTypeEnabled") == 1) {
			canPlaySound = true;
		}else canPlaySound=false;

		if (instance != null)
			Destroy (gameObject);
		else {
			instance=this;
			GameObject.DontDestroyOnLoad (gameObject);
			//audioS.clip=ambientClip;
			if(canPlaySound)audioS.Play();
			if(canPlaySound==false)Destroy(gameObject);
		}




	}
	

}
