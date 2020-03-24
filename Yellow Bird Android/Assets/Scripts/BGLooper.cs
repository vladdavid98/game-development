using UnityEngine;
using System.Collections;

public class BGLooper : MonoBehaviour
{


	int numBGPanels = 6;
	float pipeMax = 2.5f;
	float pipeMin = 0.25f;
	int toggle;

	void Start ()
	{
		toggle = 1;
		if (PlayerPrefs.GetInt ("flapTypeToTheRight") == 1 && Application.loadedLevel == 2) {
			toggle = 1;
		} else if (PlayerPrefs.GetInt ("flapTypeToTheLeft") == 1 && Application.loadedLevel == 2) {
			toggle = -1;
		}
	}


	void OnTriggerEnter2D (Collider2D collider)
	{

		float widthOfBGObject = ((BoxCollider2D)collider).size.x;

		Vector3 pos = collider.transform.position;
		if (toggle == 1) {
			pos.x += widthOfBGObject * numBGPanels * toggle;
		} else if (toggle == -1) {
			pos.x += widthOfBGObject * numBGPanels * toggle;
		}

		if (collider.tag == "Pipe") {
			pos.y = Random.Range (pipeMin, pipeMax);
		}


		collider.transform.position = pos;
	}
}
