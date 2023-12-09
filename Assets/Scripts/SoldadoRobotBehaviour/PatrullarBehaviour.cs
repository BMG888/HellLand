using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrullarBehaviour : StateMachineBehaviour
{
    private Transform posicionJugador; // transform del jugador
    private Transform patrullaje; // transform del punto de patrullaje
    
    private float velocidad; // velocidad de movimiento
    private float distanciaSeguir; // distancia minima para que el enemigo siga al jugador  
    private float xMinimo; // punto en x minimo en que se movera el punto de patrullaje
    private float xMaximo; // punto en x maximo en que se movera el punto de patrullaje
    private float y; // punto en y en que se movera el punto de patrullaje
    private float inicioTiempoEspera; // tiempo en que se ejecutara la animacion de idle
    private float tiempoEspera; // tiempo que ira decreciendo en el transcuros del tiempo
    public Vector3 direccion; // direccion en que el enemigo tendra la vision 

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        posicionJugador = GameObject.FindGameObjectWithTag("Jugador").transform; // el transform se obtiene por medio del tag
        patrullaje = animator.gameObject.GetComponent<EnemigoIAScript>().patrullaje; // las variables y demas transform se obtienen por medio del script del enemigo
        velocidad = animator.gameObject.GetComponent<EnemigoIAScript>().velocidadPatrulla;                
        distanciaSeguir = animator.gameObject.GetComponent<EnemigoIAScript>().distanciaSeguir;
        xMaximo = animator.gameObject.GetComponent<EnemigoIAScript>().xMaximo;
        xMinimo = animator.gameObject.GetComponent<EnemigoIAScript>().xMinimo;
        y = animator.gameObject.GetComponent<EnemigoIAScript>().y;
        inicioTiempoEspera = animator.gameObject.GetComponent<EnemigoIAScript>().inicioTiempoEspera;        
        tiempoEspera = inicioTiempoEspera;

        patrullaje.position = new Vector2(Random.Range(xMinimo, xMaximo), y); // la pocision del punto de patrullaje tendra un rango random de acuerdo a lo especificado
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        direccion = patrullaje.transform.position - animator.transform.position; // direccion del enemigo en direccion al jugador
        animator.transform.position = Vector2.MoveTowards(animator.transform.position, patrullaje.transform.position, velocidad * Time.deltaTime); // la pocision del enemigo sera igual a moverse desde su punto inicial al punto de patrullaje a la velocidad establecida
        if (Vector2.Distance(animator.transform.position, patrullaje.position) < 0.2f) // si la distancia del enemigo hacia el punto de patrullaje es menor a 0.2
        {
            if(tiempoEspera <= 0) // si el tiempo de espera es menor o igual a 0
            {
                patrullaje.position = new Vector2(Random.Range(xMinimo, xMaximo), y); // se cambia la posicion del punto de patrullaje a un rango random
                tiempoEspera = inicioTiempoEspera; // se reinicia el tiempo de espera               
            }
            else // si no
            {
                animator.SetBool("Idle", true);
                tiempoEspera -= Time.deltaTime; // decrece el tiempo de espera
            }
        }
        if (direccion.x >= 0.0f) // si la direccion en x es mayor o igual a 0
        {
            animator.transform.localScale = new Vector3(1f, 1f, 1f); // ve a la derecha
        }
        else
            if (direccion.x <= 0.0f) // si es menor
        {
            animator.transform.localScale = new Vector3(-1f, 1f, 1f); // ve a la izquierda
        }

        if (Vector2.Distance(animator.transform.position, posicionJugador.position) <= distanciaSeguir) // si la distancia entre el enemigo y el jugador es menor o igual a la distancia de seguir
        {
            animator.SetBool("Seguir", true);
            animator.SetBool("Patrullar", false);
        }            
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
