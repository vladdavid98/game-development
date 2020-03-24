using UnityEngine;
using System.Collections;

public class GroundMover : MonoBehaviour {

	
	Rigidbody2D player;
	public BirdMovement _Player;
	public Vector3 playVel;
	public Vector3 menuAndAlmostPlayVel;

	void Start ()
	{
		//BirdMovement _Player = GameObject.FindGameObjectWithTag ("Player");
		GameObject player_go = GameObject.FindGameObjectWithTag ("Player");
		if (player_go == null)
			//Debug.Log ("NULL!");
		//if (player_go == null) {
		//	return;
		//}

		//player=player_go.GetComponent<Rigidbody2D>();

		playVel.x = _Player.flapSpeed-0.01f;
		menuAndAlmostPlayVel = _Player.menuGametypeFlapSpeed;
		menuAndAlmostPlayVel.x -= 0.01f;
	}




	/*void FixedUpdate () {
		float vel = player.velocity.x*0.9f;

		transform.position=transform.position+Vector3.right*vel*Time.deltaTime;
	}*/
	void Update()
	{
		if (_Player.gameIsInPlayMode) {
			transform.position += playVel;
			//Debug.Log ("1");
		} else if (_Player.gameIsInMenuMode || _Player.gameIsInAlmostPlayMode) {
			transform.position += menuAndAlmostPlayVel;
			//Debug.Log ("2");
		}


	}
}
