using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepatrullarBehaviour : StateMachineBehaviour
{
    private Transform posicionJugador; // transform del jugador
    private Transform patrullaje; // transform del punto de patrullaje
    
    private float velocidad; // velocidad de movimiento
    private float distanciaSeguir; // distancia minima para que el enemigo siga al jugador

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        posicionJugador = GameObject.FindGameObjectWithTag("Jugador").transform; // el transform se obtiene por medio del tag
        patrullaje = animator.gameObject.GetComponent<EnemigoIAScript>().patrullaje; // las variables y demas transform se obtienen por medio del script del enemigo
        velocidad = animator.gameObject.GetComponent<EnemigoIAScript>().velocidadPatrulla;
        distanciaSeguir = animator.gameObject.GetComponent<EnemigoIAScript>().distanciaSeguir;        
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Vector2.Distance(animator.transform.position, posicionJugador.position) > distanciaSeguir) // si la distancia enre el enemigo y el jugador es mayor a la de seguir
        {
            animator.transform.position = Vector2.MoveTowards(animator.transform.position, patrullaje.transform.position, velocidad * Time.deltaTime); // se mueve directo al punto de patrullaje
            Vector3 direccion = patrullaje.transform.position - animator.transform.position; // vector para establecer la direccion del enemigo en direccion al jugador
            if (Vector2.Distance(animator.transform.position, patrullaje.position) < 0.2f) // si la distancia entre el enemigo y el punto de patrullaje es menor a 0.2
            {
                animator.SetBool("Patrullar", true);
                animator.SetBool("Repatrullar", false);
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
        }else 
        if(Vector2.Distance(animator.transform.position, posicionJugador.position) < distanciaSeguir) // si la distancia entre el enemigo y el jugador es menor a la distancia de seguir
        {
            animator.SetBool("Seguir", true);
            animator.SetBool("Repatrullar", false);
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
