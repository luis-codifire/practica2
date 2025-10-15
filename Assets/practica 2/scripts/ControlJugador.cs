using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlJugador : MonoBehaviour
{
    //movimiento
    public float velocidad = 5f; //velocidad del jugador
    public float gravedad = -9.8f; //controlar la velocidad o fuerza de gravedad del juego
    private CharacterController controller; //permite el movimiento en el juego
    private Vector3 velocidadVertical; //permite saber que tan rapido caemos

    //variable vista
    public Transform camara; //registra que camara funcionara como los ojos del jugador
    public float sensibilidadMouse = 200f; //que tan rapido girara el mouse para ver diferentes direcciones
    private float rotacionXVertical = 0f; //indica cuantos grados podra ver el jugador hacia arriba o hacia abajo



    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>(); //busca el componente charactercontroller

        //esta linea bloquea el puntero del mouse en los limite de la pantalla
        Cursor.lockState = CursorLockMode.Locked;
    }

    

    // Update is called once per frame
    void Update()
    {
        ManejadorMovimiento();
        ManejadorVista();
    }

    void ManejadorVista()
    {
        //1. leer input del mouse (Ctrl+D para duplicar linea)
        float mouseX= Input.GetAxis("Mouse X") * sensibilidadMouse *Time.deltaTime; //desplazamiento horizontal
        float mouseY= Input.GetAxis("Mouse Y") * sensibilidadMouse *Time.deltaTime; //desplazamiento vertical

        //2. construir rotacion horizontal
        transform.Rotate(Vector3.up * mouseX);

        //3. registro de la rotacion vertical
        rotacionXVertical -= mouseY;

        //4. limitar rotacion vertical
        Mathf.Clamp(rotacionXVertical, -90f, 90f);

        //5. aplicar rotacion
                                              // ejes:     x       y  z
        camara.localRotation = Quaternion.Euler(rotacionXVertical, 0, 0);
    }
    void ManejadorMovimiento()
    {
        //1. leer input de movimiento (wasd o flechas)
        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");

        //2. crear vector de movimiento
            //almacenar localmente el registro de direccion movimiento
        Vector3 direccion = transform.right*inputX + transform.forward*inputZ;

        //3. mover charactercontroller
        controller.Move(direccion*velocidad*Time.deltaTime);

        //4. aplicar gravedad
        //registro si esta en el piso para un futuro comportamiento de salto
        if (controller.isGrounded && velocidadVertical.y < 0)
        {
            velocidadVertical.y = 2f; //pequeña fuerza de gravedad para mantenerlo abajo en el piso
        }

        //aceleracion de la gravedad
        velocidadVertical.y += gravedad*Time.deltaTime;

        //mover el controlador hacia abajo
        controller.Move(velocidadVertical*Time.deltaTime);
    }
}