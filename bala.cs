using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bala : MonoBehaviour
{

    public float velocidad = 2;
    public int daño = 40;

    public float tiempo_desctrucccion = 10;
    private float tiempo_transcurrido = 0;


    public Vector2 direccion_vel;


    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        tiempo_transcurrido += Time.deltaTime;

        direccion_vel = this.transform.position;

        this.transform.position += transform.right * Time.deltaTime * velocidad;

        if (tiempo_transcurrido >=tiempo_desctrucccion)
        {
            Destroy(this.gameObject);
        }
        direccion_vel = new Vector2(direccion_vel.x - this.transform.position.x, direccion_vel.y - this.transform.position.y);

    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.tag == "ground" || collision.tag == "ground_move")
        {
            if (collision.name =="movible" )
            {
                Debug.Log(collision.name);
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(-direccion_vel*10);

            }
            Destroy(this.gameObject);
        }
    }
}
