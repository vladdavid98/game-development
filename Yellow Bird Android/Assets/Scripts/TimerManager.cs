using UnityEngine;
using System.Collections;

public class TimerManager : MonoBehaviour {

	public static int timeToIgnoreInput;
	// Use this for initialization
	void Start () {
		timeToIgnoreInput = 30;
	}
	
	// Update is called once per frame
	void Update () {
		if (timeToIgnoreInput > 0)
			timeToIgnoreInput--;
	}
}
