using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraScript : MonoBehaviour
{
    public GameObject Jugador; // se hace referencia al jugador

    public string nombreCancion;

    private void Awake()
    {
        AudioScript.instanciar.ReproducirMusica(nombreCancion);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Jugador != null) //si el jugador aun existe
        {
            Vector3 posicion = transform.position; // se coge la posición de la camara
            posicion.x = Jugador.transform.position.x; // se posiciona de acuerdo al eje x
            posicion.y = Jugador.transform.position.y; // se posiciona de acuerdo al eje y
            transform.position = posicion; // se guarda en el vector posicion
        }        
    }
}
