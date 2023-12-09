using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemigoIAScript : MonoBehaviour
{
    public Transform puntoDisparo;
    public GameObject balae;
    public GameObject muerte;
    public Transform patrullaje;
    public Transform jugador;

    public int vida;    
    private int golpe = 1;
    public float velocidad;
    public float velocidadPatrulla;
    public float distanciaRetirar;
    public float distanciaParar;
    public float distanciaSeguir;
    public float inicioTiempoEspera;
    public float xMinimo;
    public float xMaximo;
    public float y;
    public float distanciaReproduccion;

    // Start is called before the first frame update
    void Start()
    {
        jugador = GameObject.FindGameObjectWithTag("Jugador").transform;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PerderVida(int daño)
    {
        vida -= daño;
        if (vida <= 0)
        {
            Morir();
            this.gameObject.SetActive(false);
        }        
    }

    private void Morir()
    {
        if(muerte != null)
        {
            GameObject efectoMorir = Instantiate(muerte, transform.position, Quaternion.identity);
            AudioScript.instanciar.ReproducirEfectos("srmuerte");
            Destroy(efectoMorir, 1f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if(collision.gameObject.tag == "Jugador") // si el jugador entra en collision con el enemigo
        {
            FindObjectOfType<BarraVidaScript>().PerderVida(golpe); // el jugador pierde vida
            FindObjectOfType<JugadorScript>().Golpe(true);
        }
    }

    public void DispararEnemigo()
    {
        GameObject bala = Instantiate(balae, puntoDisparo); // se obtiene el prefab de la bala, la cual sale del punto de disparo
        bala.transform.parent = null;
    }   

    public void SRP1()
    {
        if(Vector2.Distance(transform.position, jugador.position) < distanciaReproduccion)
        {
            AudioScript.instanciar.ReproducirEfectos("srp1");
        }
    }

    public void SRP2()
    {
        if (Vector2.Distance(transform.position, jugador.position) < distanciaReproduccion)
        {
            AudioScript.instanciar.ReproducirEfectos("srp2");
        }
    }    
}
