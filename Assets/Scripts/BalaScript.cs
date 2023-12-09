using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalaScript : MonoBehaviour, InterfaceDataPersistenceScript
{
    public float velocidadBala; // velocidad que tendrá la bala
    public int daño;
    public GameObject impacto;

    // Update is called once per frame
    void Update()
    {       
        transform.Translate(transform.right * transform.localScale.x * velocidadBala * Time.deltaTime); // la bala va en la misma direccion en que esta la mira del objeto a escala del objeto en x en la velocidad especificada        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Jugador") // si colisiona con el jugador
        {
            return; // no hace nada
        }

        if (collision.GetComponent<DisparoEfectoScript>())
        {
            collision.GetComponent<DisparoEfectoScript>().Efecto();
            DestruirBala();
        }

        if (collision.tag == "Enemigo") // si el tag es enemigo
        {
            Impacto();
            collision.GetComponent<EnemigoIAScript>().PerderVida(daño); // pierde vida
            DestruirBala();
        }

        if (collision.gameObject.layer == 6 || collision.gameObject.layer == 9 || collision.tag == "Puerta") // si el layer es suelo o pared o techo o tag puerta
        {
            Impacto();            
            DestruirBala();
        }        
    }

    public void DestruirBala()
    {       
        Destroy(gameObject); //destruye el objeto
    }

    public void Sonido()
    {
        AudioScript.instanciar.ReproducirEfectos("disparo");
    }

    private void Impacto()
    {
        if(impacto != null) // si el objeto existe
        {
            GameObject balaimpactar = Instantiate(impacto, transform.position, Quaternion.identity); // el prefab en la posicion actual y sin rotacion es igual a bala impactar           
            Destroy(balaimpactar, 0.25f);
        }
    }

    public void CargarDatos(DatosJuego datos)
    {
        this.daño = datos.dañoBala;
    }

    public void GuardarDatos(ref DatosJuego datos)
    {
        datos.dañoBala = this.daño;
    }
}
