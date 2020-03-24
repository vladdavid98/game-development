using UnityEngine;
using System.Collections;

public class BGSpawner : MonoBehaviour
{
	
	int numBGPanels = 6;
	int numPipes = 6;
	int toggle;

	public float pipeMin = 0.25f;
	public float pipeMax = 25f;
	public GameObject skyPrefab;
	public GameObject pipePrefab;
	public GameObject groundPrefab;
	GameObject camera_go;
	GameObject bglooper_go;
	Transform cameraTransform;

	Vector3 pos;
	Vector3 skyPos;
	Vector3 changeSkyPos;
	Vector3 pipePos;
	Vector3 changePipePos;
	Vector3 groundPos;
	Vector3 changeGroundPos;
	Quaternion rotation;



	void Start ()
	{
		if (PlayerPrefs.GetInt ("flapTypeToTheRight") == 1) {
			toggle = 1;
			rotation.Set (0, 0, 0, 0);
		} else if (PlayerPrefs.GetInt ("flapTypeToTheLeft") == 1) {
			toggle = -1;
			rotation.Set (0, 180, 0, 0);
		}
		camera_go = GameObject.FindGameObjectWithTag ("MainCamera");
		bglooper_go = GameObject.FindGameObjectWithTag ("Looper");
		pos = bglooper_go.transform.localPosition;
		pos.x *= toggle;
		bglooper_go.transform.localPosition = pos;
		cameraTransform = camera_go.transform;
		InitialiseSky ();
		InitialiseGround ();
		//InitialisePipes();

		//Debug.Log(PlayerPrefs.GetInt ("skyTypeContinuous"));
		//Debug.Log(PlayerPrefs.GetInt ("skyTypeStatic"));
		//Debug.Log(PlayerPrefs.GetInt ("flapTypeToTheRight"));
		//Debug.Log(PlayerPrefs.GetInt ("flapTypeToTheLeft"));
	}

	void FixedUpdate ()
	{
		if (BirdMovement.gameStarted == true) {
			MoveSky ();
		}
	}

	protected void InitialiseSky ()
	{
		if (PlayerPrefs.GetInt ("skyTypeContinuous") == 1) {
			skyPos.x = -1.5f;
			skyPos.y = 2.56f;
			changeSkyPos.x = 2.88f * toggle;

			for (int i = 1; i <= numBGPanels; i++) {
				GameObject newActor = Instantiate (skyPrefab) as GameObject;
				newActor.transform.localPosition = skyPos;
				newActor.transform.localRotation = rotation;
				skyPos.x += changeSkyPos.x;
			}
		} else if (PlayerPrefs.GetInt ("skyTypeStatic") == 1) {
			GameObject newActor = Instantiate (skyPrefab) as GameObject;
			newActor.transform.SetParent (cameraTransform);
			Vector3 pos = newActor.transform.localPosition;
			pos.x = 0;
			pos.y = 0;
			newActor.transform.localPosition = pos;
			newActor.transform.localRotation = rotation;
			Vector3 scale = newActor.transform.localScale;
			scale.x = 1.03f;
			scale.y = 1.03f;
			newActor.transform.localScale = scale;
		}
	}

	public void InitialisePipes ()
	{
		
		changePipePos.x = 2f * toggle;
		pipePos.z = -1f;
		pipePos.x = this.transform.position.x + 5 * toggle;
		for (int i = 1; i <= numPipes; i++) {
			GameObject newActor = Instantiate (pipePrefab) as GameObject;
			newActor.transform.localPosition = pipePos;
			pipePos.x += changePipePos.x;
			//Debug.Log(pipePos);

		}
		GameObject[] pipes = GameObject.FindGameObjectsWithTag ("Pipe");
		foreach (GameObject pipe in pipes) {
			Vector3 pos = pipe.transform.position;
			pos.y = Random.Range (pipeMin, pipeMax);
			pipe.transform.position = pos;
		}
	}

	void InitialiseGround ()
	{
		changeGroundPos.x = 3.36f * toggle;
		groundPos.y = -0.56f;
		if (toggle == -1)
			groundPos.x = 1f;
		else
			groundPos.x = -1f;
		groundPos.z = -2f;
		for (int i = 1; i <= numBGPanels; i++) {
			GameObject newActor = Instantiate (groundPrefab) as GameObject;
			newActor.transform.localPosition = groundPos;
			newActor.transform.localRotation = rotation;
			groundPos.x += changeGroundPos.x;
		}
	}

	void MoveSky ()
	{
		if (PlayerPrefs.GetInt ("skyTypeContinuous") == 1) {
			GameObject[] sky = GameObject.FindGameObjectsWithTag ("Sky");
			{
				foreach (GameObject Sky in sky) {
					Vector3 pos = Sky.transform.localPosition;
					Sky.transform.localPosition = pos;
				}
			}
		} else if (PlayerPrefs.GetInt ("skyTypeStatic") == 1) {
			GameObject[] sky = GameObject.FindGameObjectsWithTag ("Sky");
			foreach (GameObject Sky in sky) {
				Vector3 pos;
				pos.z = 10;
				pos.x = 0;
				pos.y = 0;
				Sky.transform.localPosition = pos;
			}
		}
	}


}