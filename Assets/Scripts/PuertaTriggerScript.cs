using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuertaTriggerScript : MonoBehaviour
{
    public int linkPortonId; // id para linkear el trigger con la puerta
    public int linkPortonL1;
    public int linkPortonL2;
    public int linkElectricaId;
    public int linkPEIDCard;
    public Sprite desactivado;
    public Sprite activado;    

    private void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = desactivado;
    }

    public void AbrirPorton(GameObject item) // la funcion recibe un parametro de tipo gameObject
    {
        Activar();
        EventoManagerScript.instanciar.EmpezarEventoPuerta(linkPortonId); // inicia el evento
    }

    public void AbrirPLlaveUno(GameObject item)
    {
        Activar();
        EventoManagerScript.instanciar.EmpezarEventoLlaveUno(linkPortonL1);
    }

    public void AbrirPLlaveDos(GameObject item)
    {
        Activar();
        EventoManagerScript.instanciar.EmpezarEventoLlaveDos(linkPortonL2);
    }

    public void AbrirPElectrica(GameObject item)
    {
        AudioScript.instanciar.ReproducirEfectos("interruptor");
        Activar();
        EventoManagerScript.instanciar.EmpezarEventoPuertaElectrica(linkElectricaId);
    }

    public void AbrirPEIDCard(GameObject item)
    {
        AudioScript.instanciar.ReproducirEfectos("interruptor");
        Activar();
        EventoManagerScript.instanciar.EmpezarEventoPEIDCard(linkPEIDCard);
    }

    public void Activar()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = activado; // se cambia el sprite
        gameObject.GetComponent<BoxCollider2D>().enabled = false; // se desactiva el box collider para no interactuar mas con el
    }
}
