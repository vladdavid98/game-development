using UnityEngine;
using System.Collections;

public class BGLooper : MonoBehaviour
{
    private int numBGPanels = 6;
    private float pipeMax = 2.5f;
    private float pipeMin = 0.25f;

    private void Start()
    {
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        float widthOfBgObject = collider.bounds.size.x;
        Vector3 pos = collider.transform.position;

        pos.x += widthOfBgObject * numBGPanels;

        if (collider.tag == "Pipe")
        {
            pos.y = Random.Range(pipeMin, pipeMax);
        }

        collider.transform.position = pos;
    }
}