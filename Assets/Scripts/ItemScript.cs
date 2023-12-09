using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))] //por defecto, cada GameObject que tenga este Script, se le otorgará un box collider
public class ItemScript : MonoBehaviour
{
    public enum TipoInteraccion {Ninguno, Recoger, Examinar, Agregar, Conversar, Balas, Palanca, Switch, Llave1, Llave2, IDCard} // lista para escoger tipo de interaccion
    public enum TipoItem {Ninguno, Lore, Consumible, Mecanismo} // lista para elegir el tipo de objeto
    public TipoInteraccion tipoInteraccion;
    public TipoItem tipoItem;

    public UnityEvent consumir;        

    public string nombre;
    [TextArea(3, 5)]
    public string descripcion;

    public ConversacionScript conversacion; // se llama el script que contiene las oraciones    

    private void Reset()
    {
        GetComponent<Collider2D>().isTrigger = true; // en el collider activa el componente trigger
        gameObject.layer = 7; // se le asigna el tipo de layer que tendrá
        GetComponent<SpriteRenderer>().sortingOrder = -1;
    }    

    public void Interaccion()
    {
        switch (tipoInteraccion)
        {
            case TipoInteraccion.Recoger:
                if (!FindObjectOfType<SistemaInventarioScript>().InventarioLleno()) //si el inventario esta lleno, no se puede recoger
                {
                    return;
                }
                FindObjectOfType<SistemaInventarioScript>().Recoger(gameObject); // se agrega el objeto en la lista del script mencionado
                gameObject.SetActive(false);
                break;

            case TipoInteraccion.Examinar:
                FindObjectOfType<SistemaInteraccionScript>().ExaminarObjetos(this); // llamar el objeto de interaccion al SistemaInteraccion
                break;

            case TipoInteraccion.Agregar:
                FindObjectOfType<SistemaInventarioScript>().Agregar(gameObject); // se agrega el objeto al contador en el inventario
                gameObject.SetActive(false);
                break;

            case TipoInteraccion.Conversar:
                FindObjectOfType<ConversacionManagerScript>().EmpezarConversacion(conversacion); // se inicializa la conversacion
                break;

            case TipoInteraccion.Balas:
                if(FindObjectOfType<DispararScript>().municionDisponible >= FindObjectOfType<DispararScript>().maxMunicionDisponible) // si la municion disponible es amyor igual al maximo
                {
                    return; // no recoge las balas
                }
                FindObjectOfType<DispararScript>().Municion(gameObject); // la municion se agrega a la municion disponible
                gameObject.SetActive(false);
                break;

            case TipoInteraccion.Palanca:
                gameObject.GetComponent<PuertaTriggerScript>().AbrirPorton(gameObject);
                break;

            case TipoInteraccion.Switch:
                gameObject.GetComponent<PuertaTriggerScript>().AbrirPElectrica(gameObject);
                break;

            case TipoInteraccion.Llave1:
                if(FindObjectOfType<SistemaInventarioScript>().llave1 == false) // si aun no tiene la primera llave
                {
                    FindObjectOfType<SistemaInteraccionScript>().ExaminarObjetos(this); // se examina la cerradura
                }
                else if(FindObjectOfType<SistemaInventarioScript>().llave1 == true) // si ya la tiene 
                {
                    gameObject.GetComponent<PuertaTriggerScript>().AbrirPLlaveUno(gameObject); // inicia el evento
                }                
                break;

            case TipoInteraccion.Llave2:
                if(FindObjectOfType<SistemaInventarioScript>().llave2 == false) // si aun no tiene la segunda llave 
                {
                    FindObjectOfType<SistemaInteraccionScript>().ExaminarObjetos(this); // se examina la cerradura
                }
                else if(FindObjectOfType<SistemaInventarioScript>().llave2 == true) // si ya la tiene 
                {
                    gameObject.GetComponent<PuertaTriggerScript>().AbrirPLlaveDos(gameObject); // inicia el evento
                }
                break;

            case TipoInteraccion.IDCard:
                if(FindObjectOfType<SistemaInventarioScript>().idCard == false) // si aun no tiene la tarjeta de identificacion
                {
                    FindObjectOfType<SistemaInteraccionScript>().ExaminarObjetos(this); // se examina el escaner
                }
                else if(FindObjectOfType<SistemaInventarioScript>().idCard == true) // si la tiene 
                {
                    gameObject.GetComponent<PuertaTriggerScript>().AbrirPEIDCard(gameObject); // inicia el evento
                }
                break;

            default:
                break;
        }
    }
}
