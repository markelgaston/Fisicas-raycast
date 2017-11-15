using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disparo : MonoBehaviour {

    //teclado
    public teclado teclado;

    //arma y lugar donde se dispara la bala
    public GameObject arma;
    public GameObject agujero;

    //Bala
    public GameObject bala;

    private Vector3 pos_inicial;


    // Use this for initialization
    void Start () {
        pos_inicial = arma.gameObject.transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {

        //APUNTADOS
        if (Input.GetKey(teclado.kc_derecha)) //derecha
        {
            arma.transform.position = transform.TransformPoint(pos_inicial);
            arma.transform.localEulerAngles = new Vector3(0, 0, 0);
        }

        if (Input.GetKey(teclado.kc_izquierda)) //izquierda
        {
            arma.transform.position = transform.TransformPoint(-pos_inicial);
            arma.transform.localEulerAngles = new Vector3(0, 0, -180);
        }
        if (Input.GetKey(teclado.kc_arriba)) //arriba
        {
            arma.transform.position = transform.TransformPoint(new Vector3(pos_inicial.y,pos_inicial.x,0));
            arma.transform.localEulerAngles = new Vector3(0, 0, 90);
        }

        if (Input.GetKey(teclado.kc_abajo)) //abajo
        {
            arma.transform.position = transform.TransformPoint(new Vector3(-pos_inicial.y, -pos_inicial.x, 0));
            arma.transform.localEulerAngles = new Vector3(0, 0, -90);
        }

        //disparo
        if (Input.GetKeyDown(teclado.kc_disparo)) //abajo
        {
            Instantiate(bala, agujero.transform.position, agujero.transform.rotation);
        }

    }
}
