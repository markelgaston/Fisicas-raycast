using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colisiones : MonoBehaviour
{
    //-----------------CENTRO----------(prueba, sin terminar)
    public RaycastHit2D centro;
    public float length;
    //--------------------------------

    public Vector2 movimiento;//servira para saber hacia donde se mueve y a que velocidad

    public struct ray
    { //los raycast que vamos a generar
        public RaycastHit2D top;
        public RaycastHit2D down;
        public RaycastHit2D left;
        public RaycastHit2D right;
    }
    public struct point //Almacenaremos aqui las esquinas del box (jugador)
    {
        public Vector2 top_left;
        public Vector2 top_right;
        public Vector2 down_left;
        public Vector2 down_right;
    }

    public struct estados //para saber si estamos chocando o no
    {
        public bool techo;
        public bool suelo;
        public bool pared_izquierda;
        public bool pared_derecha;

        //la variable salto se modificará desde fuera cuando se realice un salto y se deje de saltar
        public bool salto;
    }

    public estados estado;
    public ray rays;
    public point points;

    //------------Movimiento-----------------------

    //Gameobject jugador:
    public GameObject jugador;

    //teclado teclado
    public teclado teclado;

    public float vel_horizontal = 0.5f;

    //gravedad
    [SerializeField]
    private float vel_caida_max = 0.5f;
    public float peso = 1f;
    public float vel_caida_actual = 0f;

    //salto
    public float salto_max = 10;
    public float salto_fuerza = 5;
    public float salto_peso = 5;
    public float salto_vel;
    public int saltos_restantes = 2;
    //vvel 
    public float salto_tiempo = 2;

    //-----controla cuando llega a la altura maxima
    private float salto_altura_actual = 0;

    //INERCIA
    public Vector3 inercia;

    //layer con todos los gameobject con los que colisionara el raycast
    public LayerMask ground;
    //-----------------------------------
    // Use this for initialization
    void Start()
    {



        //Conseguimos la posicion local de los extremos del objeto

        points.top_left = new Vector2((-1f / 2f) + (1f / 20f), 1f / 2f);
        points.top_right = new Vector2(1f / 2f, (1f / 2f) - (1f / 20f));
        points.down_left = new Vector2(-1f / 2f, (-1f / 2f) + (1f / 20f));
        points.down_right = new Vector2((1f / 2f) - (1f / 20f), -1f / 2f);





        /*------copia de lo que hice-------
        points.top_left = new Vector2((-this.transform.lossyScale.x / 2) + (this.transform.lossyScale.x / 10), this.transform.lossyScale.y / 2);
        points.top_right = new Vector2(this.transform.lossyScale.x / 2, (this.transform.lossyScale.y / 2) - (this.transform.lossyScale.y / 10));
        points.down_left = new Vector2(-this.transform.lossyScale.x / 2, (-this.transform.lossyScale.y / 2) + (this.transform.lossyScale.y / 10));
        points.down_right = new Vector2((this.transform.lossyScale.x / 2) - (this.transform.lossyScale.x / 10), -this.transform.lossyScale.y / 2);
        */

        //variables
        salto_vel = salto_fuerza;

        //inicializamos los estados a null
        estado.techo = false;
        estado.suelo = false;
        estado.pared_izquierda = false;
        estado.pared_derecha = false;
        estado.salto = false;
    }

    // Update is called once per frame
    void Update()
    {
        inercia = new Vector3(0,0,0);
        //----------------Movimiento-------------------
        movimiento = new Vector2(0f, 0f);

        if (estado.suelo == false && estado.salto == false) //caida por gravedad
        {
            if (vel_caida_actual < vel_caida_max)
            {
                vel_caida_actual += peso * Time.deltaTime;
            }

            jugador.transform.position -= new Vector3(0, vel_caida_actual * Time.deltaTime, 0);
            movimiento.y -= vel_caida_actual * Time.deltaTime;
        }

        if (Input.GetKey(teclado.kc_derecha) && estado.pared_derecha == false) //movimiento derecha
        {
            jugador.transform.position -= new Vector3(-vel_horizontal * Time.deltaTime, 0, 0);
            movimiento.x += vel_horizontal * Time.deltaTime;
        }

        if (Input.GetKey(teclado.kc_izquierda) && estado.pared_izquierda == false) //movimiento izquierda
        {
            jugador.transform.position -= new Vector3(vel_horizontal * Time.deltaTime, 0, 0);
            movimiento.x += -vel_horizontal * Time.deltaTime;
        }

        if (Input.GetKeyUp(teclado.kc_salto))
        {
            estado.salto = false;
            salto_altura_actual = 0;
            salto_vel = salto_max / salto_tiempo;
        }

        if (Input.GetKey(teclado.kc_salto) && (estado.suelo == true || estado.salto == true)) //Saltar
        {
            vel_caida_actual = 0;
            estado.salto = true;
            
            salto_vel -= salto_vel / 2 * Time.deltaTime;


            jugador.transform.position += new Vector3(0, salto_vel, 0);
            movimiento.y += salto_vel;

            //------
            salto_altura_actual += (salto_vel);
            if (salto_altura_actual >= salto_max)
            {
                estado.salto = false;
                salto_altura_actual = 0;
                salto_vel = salto_max / salto_tiempo;
                estado.suelo = false;
            }


        }
        if (Input.GetKeyDown(teclado.kc_salto))
        {
            if (saltos_restantes > 0) 
            {
                salto_vel = salto_max / salto_tiempo;
                estado.salto = true;
                salto_altura_actual = 0;
                saltos_restantes--;
            }
            if (estado.suelo == false)
            {
                saltos_restantes = 0;
            }
        }
        //-------------------RAYCASTS----------------------
        //TOP
        rays.top = Physics2D.Raycast(transform.TransformPoint(points.top_left), this.transform.right, this.transform.lossyScale.x * 0.90f, ground);
        Debug.DrawRay(transform.TransformPoint(points.top_left), this.transform.right * (this.transform.lossyScale.x * 0.90f));
        //Debug.Log(rays.top.transform.tag);
        //DOWN
        rays.down = Physics2D.Raycast(transform.TransformPoint(points.down_right), -this.transform.right, this.transform.lossyScale.x * 0.90f,ground);
        Debug.DrawRay(transform.TransformPoint(points.down_right), -this.transform.right * (this.transform.lossyScale.x * 0.90f));
        //LEFT
        rays.left = Physics2D.Raycast(transform.TransformPoint(points.down_left), this.transform.up, this.transform.lossyScale.y * 0.90f,ground);
        Debug.DrawRay(transform.TransformPoint(points.down_left), this.transform.up * (this.transform.lossyScale.y * 0.90f));

        //RIGHT
        rays.right = Physics2D.Raycast(transform.TransformPoint(points.top_right), -this.transform.up, this.transform.lossyScale.y * 0.90f,ground);
        Debug.DrawRay(transform.TransformPoint(points.top_right), -this.transform.up * (this.transform.lossyScale.y * 0.90f));

        //------------Procesar raycast (¿que estados?)----------------------
        //top
        if (rays.top == true) {
            if (rays.top.transform.tag == "ground" || rays.top.transform.tag == "ground_move")
            {
                estado.techo = true;
                estado.salto = false;
                if (rays.top.transform.tag == "ground_move")
                {
                    actualizar_pos(rays.top, "", "col_abajo");
                }
            }
            else
            {
                estado.techo = false;
            }
        }

        //DOWN
        if (rays.down == true) {
            if (rays.down.transform.tag == "ground" || rays.down.transform.tag == "ground_move")
            {
                saltos_restantes=2;
                estado.suelo = true;
                vel_caida_actual = 0;
                if (rays.down.transform.tag == "ground_move" && rays.down.transform.name !="movible")
                {
                    inercia = rays.down.transform.GetComponent<plat_mov>().devolver_siguiente_movimiento();
                    actualizar_pos(rays.down, "y", "");
                }
            }
        }
        else
        {
            estado.suelo = false;
        }

        //LEFT
        if (rays.left == true && (rays.left.transform.tag == "ground" || rays.left.transform.tag == "ground_move"))
        {
            estado.pared_izquierda = true;
        }
        else
        {
            estado.pared_izquierda = false;
        }

        //RIGHT
        if (rays.right == true && (rays.right.transform.tag == "ground" || rays.right.transform.tag == "ground_move"))
        {
            estado.pared_derecha = true;
        }
        else
        {
            estado.pared_derecha = false;
        }


        //-------------------------Control de posicion-----------------
        //Debug.Log(movimiento);

        RaycastHit2D[] raycast = new RaycastHit2D[] { rays.top, rays.down, rays.left, rays.right };
        string[] eje = new string[] { "", "" };

        int control = 0;//con esta variable controlamos si debemos actualizar en 1 o 2 posiciones

        //almacenamos en la pos 2 y 3 los rays hacia los que se mueve
        if (movimiento.x < 0 || estado.pared_izquierda == true)
        {
            raycast[2] = rays.left;
            control++;
        }
        else if (movimiento.x > 0 || estado.pared_derecha == true)
        {
            raycast[2] = rays.right;
            control++;

        }
        if (movimiento.y < 0 || estado.suelo == true)
        {
            raycast[3] = rays.down;
            control++;

        }
        else if (movimiento.y > 0 || estado.techo == true)
        {
            raycast[3] = rays.top;
            control++;
        }

        //alacenamos en la pos 0 el ray hacia la direcion a la que mas rapido se mueve de lo que almacenamos antes, y el la pos 1 el otro ray
        if (movimiento.x != 0 || movimiento.y != 0)
        {
            if (Mathf.Sqrt(movimiento.x * movimiento.x) > Mathf.Sqrt(movimiento.y * movimiento.y))
            {
                raycast[0] = raycast[2];
                raycast[1] = raycast[3];
                eje[0] = "x";
                eje[1] = "y";
                //Debug.Log("x");
            }
            else if (Mathf.Sqrt(movimiento.x * movimiento.x) < Mathf.Sqrt(movimiento.y * movimiento.y))
            {
                raycast[0] = raycast[3];
                raycast[1] = raycast[2];
                eje[0] = "y";
                eje[1] = "x";
                //Debug.Log("y");
            }
            if (control == 1)
            {
                actualizar_pos(raycast[0], eje[0], ""); //actualizamos la posicion 
                //Debug.Log(control);
                control = 0;
            }
            else if (control == 2)
            {
                actualizar_pos(raycast[0], eje[0], "");
                actualizar_pos(raycast[1], eje[1], "");
                //Debug.Log(control+"ss");
                control = 0;

            }
        }
        //Debug.Log(movimiento.x);
        //-------------------------Raycast CENTRO--------------(Prueba, sin terminar)
        length = Mathf.Sqrt(((this.transform.lossyScale.x / 2f) * (this.transform.lossyScale.x / 2f)) + ((this.transform.lossyScale.y / 2f) * 0.9f) * ((this.transform.lossyScale.y / 2f) * 0.9f));

        //RIGHT_1
        centro = Physics2D.Raycast(transform.TransformPoint(new Vector2(0, 0)), new Vector2(this.transform.lossyScale.x / 2f, (this.transform.lossyScale.y / 2f) * 0.9f), length);
        Debug.DrawRay(transform.TransformPoint(new Vector2(0, 0)), new Vector2(this.transform.lossyScale.x / 2f, ((this.transform.lossyScale.y / 2f) * 0.9f)));
        //Debug.Log(centro.transform.tag);

        //RIGHT_2
        centro = Physics2D.Raycast(transform.TransformPoint(new Vector2(0, 0)), new Vector2(this.transform.lossyScale.x / 2f, -(this.transform.lossyScale.y / -2f) * 0.9f), length);
        Debug.DrawRay(transform.TransformPoint(new Vector2(0, 0)), new Vector2(this.transform.lossyScale.x / 2f, (this.transform.lossyScale.y / -2f) * 0.9f));

        //-------------------------------------------------------------------------

        //------------INERCIA-----------------
        if (inercia != new Vector3(0,0,0)) {
            jugador.transform.position += inercia;
        }
    }

    void actualizar_pos(RaycastHit2D ray, string eje, string direccion)//actualiza la posicion del jugador
    {



        if (ray == true && (ray.transform.tag == "ground" || ray.transform.tag == "ground_move"))
        {
            RaycastHit2D centro = Physics2D.Raycast(new Vector2(jugador.transform.position.x, jugador.transform.position.y), ray.point - new Vector2(jugador.transform.position.x, jugador.transform.position.y),ground);
            Debug.DrawRay(new Vector2(jugador.transform.position.x, jugador.transform.position.y), ray.point - new Vector2(jugador.transform.position.x, jugador.transform.position.y), Color.red, 1f);
            //Debug.Log(new Vector2(jugador.transform.position.x, jugador.transform.position.y));
            //Debug.Log(centro.point + "  poinr ");
            //Debug.Log("centro: "+centro.point + "ray: "+ray + "ray" + " - eje: "+eje+"length: "+ length + "distance: "+ Vector3.Distance(jugador.transform.position, centro.point));
            if (Vector3.Distance(jugador.transform.position, centro.point) <= length)
            {
                if (eje == "x")
                {
                    if (movimiento.x < 0 || estado.pared_izquierda == true)
                    {
                        jugador.transform.position = new Vector3(centro.point.x + this.transform.lossyScale.x / 2f, jugador.transform.position.y, jugador.transform.position.z);
                        //Debug.Log("1");
                    }
                    else if (movimiento.x > 0 || estado.pared_derecha == true)
                    {
                        jugador.transform.position = new Vector3(centro.point.x - this.transform.lossyScale.x / 2f, jugador.transform.position.y, jugador.transform.position.z);
                        //Debug.Log("2");
                    }

                }
                else if (eje == "y")
                {
                    if (direccion == "col_abajo" || movimiento.y < 0 ||estado.suelo == true)
                    {
                        jugador.transform.position = new Vector3(jugador.transform.position.x, centro.point.y + this.transform.lossyScale.y / 2f, jugador.transform.position.z);
                        //Debug.Log("3");
                    }
                    else if (direccion == "col_arriba" || movimiento.y > 0  || estado.techo == true)
                    {
                        jugador.transform.position = new Vector3(jugador.transform.position.x, centro.point.y - this.transform.lossyScale.y / 2f, jugador.transform.position.z);
                        //Debug.Log("4");
                    }
                }
            }
        }

    }

}
