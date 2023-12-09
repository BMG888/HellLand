using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleBehaviour : StateMachineBehaviour
{
    private Transform posicionJugador; // transform del jugador 

    private float distanciaSeguir; // distancia minima para que el enemigo siga al jugador
    private float inicioTiempoEspera; // tiempo en que se ejecuta la animacion
    private float tiempoEspera; // tiempo que ira decreciendo en el transcuros del tiempo
    private Vector3 direccion;  // direccion en que el enemigo tendra la vision  

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        posicionJugador = GameObject.FindGameObjectWithTag("Jugador").transform; // el transform se obtiene por medio del tag                
        distanciaSeguir = animator.gameObject.GetComponent<EnemigoIAScript>().distanciaSeguir; // la distancia se obtiene el script del enemigo
        inicioTiempoEspera = animator.gameObject.GetComponent<EnemigoIAScript>().inicioTiempoEspera; // igual que el tiempo de espera inicial
        direccion = animator.GetBehaviour<PatrullarBehaviour>().direccion; // la direccion se obtiene del behaviour de patrullaje
        tiempoEspera = inicioTiempoEspera;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (tiempoEspera <= 0) // si el tiempo de espera es menor o igual a 0
        {            
            animator.SetBool("Patrullar", true);
            animator.SetBool("Idle", false);
            tiempoEspera = inicioTiempoEspera;
        }
        else
        {            
            tiempoEspera -= Time.deltaTime; // si no va decreciendo con el transcurso del tiempo
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

        if (Vector2.Distance(animator.transform.position, posicionJugador.position) < distanciaSeguir) // si la posicion del enemigo es menor a la distancia de seguir 
        {
            animator.SetBool("Seguir", true);
            animator.SetBool("Idle", false);
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
