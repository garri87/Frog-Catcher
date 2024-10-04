using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    public float oscRate = 5; //Oscillation speed
    public float yAmp = 1; //max and min Range in Y axis
    private float yPos; // current Y position


    private void OnEnable()
    {
        yPos = transform.position.y;

    }
    private void Update()
    {
        transform.position = new Vector3(transform.position.x, Mathf.Sin(Time.time * oscRate / 2) * yAmp + yPos, transform.position.z);
    }
}
