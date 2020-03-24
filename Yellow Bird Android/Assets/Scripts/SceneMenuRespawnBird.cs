using UnityEngine;
using System.Collections;

public class SceneMenuRespawnBird : MonoBehaviour {



	void OnTriggerEnter2D (Collider2D collider)
	{
		Vector3 pos = collider.transform.localPosition;
		pos.x = 0f;
		pos.y = 2f;
		collider.transform.localPosition = pos;
	}
}









