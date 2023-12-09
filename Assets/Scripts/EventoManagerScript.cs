using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventoManagerScript : MonoBehaviour
{
    public static EventoManagerScript instanciar; // para poder ser utilizado en cualquier script
    public event Action<int> abrirPuertaEvento; // se crea un evento el cual recibe una variable
    public event Action<int> abrirPuertaLlaveUnoEvento;
    public event Action<int> abrirPuertaLlaveDosEvento;
    public event Action<int> abrirPuertaElectricaEvento;
    public event Action<int> abrirPEIDCardEvento;
    
    void Awake()
    {
        instanciar = this; // al cargar el script se instancia
    }

    public void EmpezarEventoPuerta(int id) // la funcion recibe un id
    {
        abrirPuertaEvento?.Invoke(id); // si existe el evento (mo es nulo), se llama el evento recibiendo el id
    }

    public void EmpezarEventoLlaveUno(int id)
    {
        abrirPuertaLlaveUnoEvento?.Invoke(id);
    }

    public void EmpezarEventoLlaveDos(int id)
    {
        abrirPuertaLlaveDosEvento?.Invoke(id);
    }

    public void EmpezarEventoPuertaElectrica(int id)
    {
        abrirPuertaElectricaEvento?.Invoke(id);
    }

    public void EmpezarEventoPEIDCard(int id)
    {
        abrirPEIDCardEvento?.Invoke(id);
    }
}
