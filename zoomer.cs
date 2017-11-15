using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zoomer : MonoBehaviour {

    public Transform cuerpo;

    public RaycastHit2D centro;
    public RaycastHit2D alt;

    public float length;
	public LayerMask ground;

    public float vel = 0.5f;
    public Vector3 pos;

    public float rotation = 20;

    public int control = 0;
    public float altura;
    // Use this for initialization
    void Start () {
        altura = cuerpo.lossyScale.y / 2;
        length = this.transform.lossyScale.y / 2f;
		centro = Physics2D.Raycast(new Vector2(this.transform.position.x, this.transform.position.y), -this.transform.up, Mathf.Infinity,ground);
        Debug.DrawRay(new Vector2(this.transform.position.x, this.transform.position.y), -this.transform.up, Color.red, 1f);

        if (centro==true)
        {
			//Debug.Log ("asdasdasd");
            this.transform.position= new Vector3(this.transform.position.x, centro.point.y, this.transform.position.z);
            pos = this.transform.position;
            control = 1;
        }
    }
	// Update is called once per frame
	void FixedUpdate() {
        /*
        centro = Physics2D.Raycast(new Vector2(this.transform.position.x, this.transform.position.y), -this.transform.up, length, ground);
        Debug.DrawRay(new Vector2(this.transform.position.x, this.transform.position.y), -this.transform.up*length, Color.red, 1f);

        if (centro==true)
        {
            Debug.Log("1");
            this.transform.position += (transform.right*vel)*Time.deltaTime;
            pos = this.transform.position;
        }
        else
        {
            Debug.Log("2");
            this.transform.Rotate(-new Vector3(0,0,1)* rotation);
            this.transform.position= pos + (new Vector3(pos.x-this.transform.position.x, pos.y- this.transform.position.y, 0));
        }
        */
        centro = Physics2D.Raycast(new Vector2(this.transform.position.x, this.transform.position.y), -this.transform.up, length, ground);
        Debug.DrawRay(new Vector2(this.transform.position.x, this.transform.position.y), -this.transform.up * length, Color.red, 1f);

        alt = Physics2D.Raycast(new Vector2(this.transform.position.x, this.transform.position.y) + new Vector2(0,transform.up.y/2), this.transform.right, length, ground);
        Debug.DrawRay(new Vector2(this.transform.position.x, this.transform.position.y) + new Vector2(transform.up.x / 2, transform.up.y / 2), this.transform.right * length, Color.red, 1f);

        if (centro == true || control == 2)
        {
            //Debug.Log("1");
            this.transform.position += (transform.right * vel) * Time.deltaTime;
            if (control == 2 && centro == true) {
                control = 1;
            }
        }
        else if (centro == false && control == 1)
        {
           // Debug.Log("2");
            this.transform.Rotate(-new Vector3(0, 0, 1) * 90);

            control = 2;

        }
    }
}
