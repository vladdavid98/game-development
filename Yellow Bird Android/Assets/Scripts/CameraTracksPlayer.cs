using UnityEngine;
using System.Collections;

public class CameraTracksPlayer : MonoBehaviour
{
    private Transform playerTransform;
    private float offsetX;
    public BirdMovement _Player1;
    public Player2 _Player2;
    private bool dead;

    private GameObject player_go;
    private GameObject player_go_2;

    // Use this for initialization
    private void Start()
    {
        player_go = GameObject.FindGameObjectWithTag("Player");
        player_go_2 = GameObject.FindGameObjectWithTag("Player2");
        dead = false;

        if (player_go == null)
        {
            return;
        }
        if (PlayerPrefs.GetInt("flapTypeToTheLeft") == 1 && Application.loadedLevel == 2)
        {
            Vector3 pos = transform.position;
            pos.x = -0.2f;
            transform.position = pos;
        }

        playerTransform = player_go.transform;

        offsetX = transform.position.x - playerTransform.position.x;
    }

    // Update is called once per frame
    private void Update()
    {
        if (_Player1.dead && !_Player2.dead)
        {
            playerTransform = player_go_2.transform;
        }

        if (_Player1.gameIsInDeathMode)
            dead = true;
        if (playerTransform != null && !dead)
        {
            Vector3 pos = transform.position;
            pos.x = playerTransform.position.x + offsetX;
            transform.position = pos;
        }
    }

    public void MoveCameraInPlayPosition()
    {
        offsetX++;
    }
}