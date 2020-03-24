using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

	int ignoreInput;


	public void LoadLevel (string name)
	{
		ignoreInput = TimerManager.timeToIgnoreInput;
		if(ignoreInput==0)
			Application.LoadLevel (name);
	}
	public void QuitLevel ()
	{
		Application.Quit();
	}



}
