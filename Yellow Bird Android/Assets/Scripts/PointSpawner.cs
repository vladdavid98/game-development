using UnityEngine;
using System.Collections;

public class PointSpawner : MonoBehaviour 
{
	public BirdMovement nextElementToSpawn;
	Vector3 pos;
	void OnTriggerEnter2D (Collider2D collider)
	{
		Destroy(collider.gameObject);
		pos.x = 0f;
		pos.y = 2f;
		Spawn(pos);
	}


	#region Protected - These methods can be overwritten in extending classes for more specific spawners
	protected void Spawn (Vector3 pos)
	{   
		
		BirdMovement newActor = Instantiate(nextElementToSpawn) as BirdMovement;
		newActor.transform.SetParent(transform);
		newActor.transform.localPosition = pos;
	}

	#endregion
}