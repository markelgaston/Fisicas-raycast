using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class seguir : MonoBehaviour {

    public Transform objetivo;

    public float vel = 0.5f;
    public float z;

	// Use this for initialization
	void Start () {
        z = this.transform.position.z;
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(objetivo.position.x, objetivo.position.y, z), vel * Time.deltaTime);
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, objetivo.rotation, vel * Time.deltaTime);
    }
}
