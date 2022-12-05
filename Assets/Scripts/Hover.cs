using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : MonoBehaviour
{
    //adjust this to change speed
    public float speed = 5f;
    //adjust this to change how high it goes
    public float height = 0.5f;

    private float phase;

    void Start() {
        phase = Random.Range(-2, 2);
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + phase, transform.localPosition.z);
    }

    void Update() {
        //get the objects current position and put it in a variable so we can access it later with less code
        Vector3 pos = transform.localPosition;
        //calculate what the new Y position will be
        float newY = pos.y + Mathf.Sin((phase + Time.time) * speed) * height;
        //set the object's Y to the new calculated Y
        transform.localPosition = new Vector3(pos.x, newY, pos.z);
    }
}
