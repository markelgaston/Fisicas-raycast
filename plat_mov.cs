using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plat_mov : MonoBehaviour {

    public Vector3 pos_1;
    public Vector3 pos_2;

    public Transform tran_pos_1;
    public Transform tran_pos_2;

    public float vel = 5;
    
    private int control = 1;

    public Vector3 velocidad = new Vector3(0, 0, 0);
    // Use this for initialization
    void Start () {
        pos_1 = tran_pos_1.position;
        pos_2 = tran_pos_2.position;

        velocidad = new Vector3((pos_1.x - pos_2.x) / vel, (pos_1.y - pos_2.y) / vel, 0);
        this.transform.position = pos_2;
    }
	
	// Update is called once per frame
	void Update () {

        if (control == 1) {
            //this.transform.position = Vector3.Lerp(this.transform.position, pos_1, vel * Time.deltaTime);
            this.transform.position += velocidad;
            if (this.transform.position == pos_1) { control = 2; velocidad = -velocidad; }
        }
        if (control == 2)
        {
            this.transform.position += velocidad;
            //this.transform.position = Vector3.Lerp(this.transform.position, pos_2, vel * Time.deltaTime);
            if (this.transform.position == pos_2) { control = 1; velocidad = -velocidad; }
        }

    }
    public Vector3 devolver_siguiente_movimiento()
    {
        Vector3 next_move = new Vector3((pos_1.x - pos_2.x) / vel, (pos_1.y - pos_2.y) / vel, 0);

        if (control == 1)
        {
            if (this.transform.position+velocidad == pos_1) { return (-next_move); }
            else
            {
                return next_move;
            }
        }
        if (control == 2)
        {
            if (this.transform.position+velocidad == pos_2) { return (next_move); }
            else
            {
                return -next_move;
            }
        }

        return next_move;
    }
}
