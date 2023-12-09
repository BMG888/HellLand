using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SistemaInteraccionScript : MonoBehaviour
{
    public Transform puntoDeteccion; //gameObject en el jugador para la deteccion    
    public LayerMask layerDeteccion; // layer que se asignara para ser detectado
    public GameObject objetoDetectado; // gameObject para detectar objeto
    public GameObject ventanaExaminar; // referencia al gameObject de la ventana
    public GameObject interactuar; // referencia al gameobject para el simbolo de interaccion
    public Image ImagenExaminar; // referencia al gameObject de la imagen
    public Text TextoExaminar; // referencia al gameObject del texto    

    private const float radioDeteccion = 0.5f; // radio que tendra ese gameObject

    public bool examinando = false; 

    // Update is called once per frame
    void Update()
    {
        if (RestringirInteraccion() == true)
        {
            return;
        }

        if (DetectarObjeto())
        {
            if (Interaccion())
            {
                objetoDetectado.GetComponent<ItemScript>().Interaccion(); //se obtiene la funcion del script mencionado
            }
        }
    }

    private bool Interaccion()
    {
        return Input.GetKeyDown(KeyCode.E); // tecla para interactuar
    }

    private bool DetectarObjeto()
    {
        Collider2D objeto =  Physics2D.OverlapCircle(puntoDeteccion.position, radioDeteccion, layerDeteccion); // se asigna la condicion del objeto
        if (objeto == null) // si lo que detecta es null
        {
            objetoDetectado = null;
            interactuar.SetActive(false);
            return false;
        }
        else 
        {
            objetoDetectado = objeto.gameObject; // pero si detecta un objeto
            interactuar.SetActive(true);
            return true;
        }
    }    

    public void ExaminarObjetos(ItemScript item)
    {
        if (examinando == true)
        {
            Time.timeScale = 1f;
            ventanaExaminar.SetActive(false); // ocultar ventana            
            examinando = false;
        }
        else
        {
            Time.timeScale = 0f;
            ImagenExaminar.sprite = item.GetComponent<SpriteRenderer>().sprite; // obtiene la imagen del componente SpriteRenderer del item
            TextoExaminar.text = item.descripcion; // obtiene la descripcion
            ventanaExaminar.SetActive(true); // mostrar ventana
            examinando = true;
        }        
    }

    public bool RestringirInteraccion()
    {
        bool restriccion = false;        
        if (FindObjectOfType<SistemaInventarioScript>().abrirInventario == true)
        {
            restriccion = true;
        }
        if (FindObjectOfType<MenuPausaScript>().Pausado == true)
        {
            restriccion = true;
        }
        return restriccion;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(puntoDeteccion.position, radioDeteccion);
    }
}
