using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConversacionManagerScript : MonoBehaviour
{
    public Text conversacion; // referencia al texto
    public GameObject ventanaConversacion; // referencia a la caja de conversacion
    public bool conversando = false; //bool para activar o desactivar el movimiento

    private Queue<string> oraciones; //lista que funciona como un fifo

    // Start is called before the first frame update
    void Start()
    {
        oraciones = new Queue<string>(); // se inicia
    }

    public void EmpezarConversacion(ConversacionScript conversacion)
    {
        conversando = true;
        oraciones.Clear(); // se limpia la cadena de string
        ventanaConversacion.SetActive(true);
        foreach(string oracion in conversacion.oraciones) // por cada oracion en las oraciones creadas
        {
            oraciones.Enqueue(oracion); // se muestra la primera oracion
        }
        ContinuarConversacion();
    }

    public void ContinuarConversacion()
    {
        if(oraciones.Count == 0) // se verifica si hay mas oraciones, en caso de que sea 0
        {
            TerminarConversacion();
            return;
        }
        string oracion = oraciones.Dequeue(); // se muestra la siguiente oracion
        conversacion.text = oracion; // se muestra en la ui
    }

    public void TerminarConversacion()
    {
        conversando = false;
        ventanaConversacion.SetActive(false);
    }
}
