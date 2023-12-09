using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetirarBehaviour : StateMachineBehaviour
{

    private Transform posicionJugador; // transform del jugador

    private float velocidad; // velocidad de movimiento
    private float distanciaSeguir; // distancia minima para que el enemigo siga al jugador
    private float distanciaRetirar; // distancia minima para que el enemigo se retire
    private float distanciaParar; // distancia minima para que el enemigo pare

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        posicionJugador = GameObject.FindGameObjectWithTag("Jugador").transform; // el transform se obtiene por medio del tag
        velocidad = animator.gameObject.GetComponent<EnemigoIAScript>().velocidad; // las variables se obtienen por medio del script del enemigo
        distanciaRetirar = animator.gameObject.GetComponent<EnemigoIAScript>().distanciaRetirar;
        distanciaParar = animator.gameObject.GetComponent<EnemigoIAScript>().distanciaParar;
        distanciaSeguir = animator.gameObject.GetComponent<EnemigoIAScript>().distanciaSeguir;        
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector3 direccion = posicionJugador.transform.position - animator.transform.position; // vector para establecer la direccion del enemigo en direccion al jugador
        if (Vector2.Distance(animator.transform.position, posicionJugador.position) < distanciaRetirar) // si la distancia del enemigo al jugador es menor a la distancia de retiro
        {
            animator.transform.position = Vector2.MoveTowards(animator.transform.position, posicionJugador.position, -velocidad * Time.deltaTime); // el enemigo se movera hacia atras con respecto a la posicion del jugador
            if (direccion.x >= 0.0f) // si la direccion en x es mayor o igual a 0
            {
                animator.transform.localScale = new Vector3(1f, 1f, 1f); // ve a la derecha
            }
            else 
            if (direccion.x <= 0.0f) // si es menor
            {
                animator.transform.localScale = new Vector3(-1f, 1f, 1f); // ve a la izquierda
            }
        }
        else
        if (Vector2.Distance(animator.transform.position, posicionJugador.position) < distanciaParar && Vector2.Distance(animator.transform.position, posicionJugador.position) > distanciaRetirar) // si la distancia entre el enemigo y el jugador es menor a la posicion de parar y mayor a la distancia de retiro
        {
            animator.SetBool("Disparar", true);
            animator.SetBool("Retirar", false);
        }
        else 
        if (Vector2.Distance(animator.transform.position, posicionJugador.position) < distanciaSeguir) // si la distancia entre el enemigo y el jugador es menor a la distancia de seguir
        {
            animator.SetBool("Seguir", true);
            animator.SetBool("Retirar", false);
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
