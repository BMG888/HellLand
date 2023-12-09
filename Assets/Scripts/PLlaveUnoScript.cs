using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))] // se agrega un box collider al gameObject
public class PLlaveUnoScript : MonoBehaviour
{
    public Transform puntoMovimiento; // punto al cual se movera la puerta
    private bool abierto = false; // bool para verificar si esta abierto
    public int linkPLlaveUnoId; // id para linkear la puerta con el trigger

    // Start is called before the first frame update
    void Start()
    {
        EventoManagerScript.instanciar.abrirPuertaLlaveUnoEvento += Abrir;
    }

    // Update is called once per frame
    void Update()
    {
        if (abierto == true) // si la puerta esta abierta
        {
            transform.position = Vector2.MoveTowards(transform.position, puntoMovimiento.position, 0.6f * Time.deltaTime); // la puerta se mueve desde su punto inicial, hasta el punto de apertura a cierta velocidad
        }
    }

    private void Abrir(int linkPortonId) // la funcion recibirá una variabe
    {
        if (linkPortonId == linkPLlaveUnoId) // si el link del trigger es igual al link de la puerta
        {
            abierto = true; // se abre
            AudioScript.instanciar.ReproducirEfectos("porton");
        }
    }

    private void OnDisable() // en caso de que la puerta se deshabilite
    {
        EventoManagerScript.instanciar.abrirPuertaLlaveUnoEvento -= Abrir; // de desuscribe al evento
    }
}
