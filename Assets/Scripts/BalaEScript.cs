using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalaEScript : MonoBehaviour
{
    public float velocidadBala;
    public int daño;
    public GameObject impactoE;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.right * transform.localScale.x * velocidadBala * Time.deltaTime); // la bala va en la misma direccion en que esta la mira del objeto a escala del objeto en x en la velocidad especificada
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemigo") // si colisiona con el enemigo
        {
            return; // no hace nada
        }  
        
        if (collision.tag == "Jugador") // si el tag es igual a jugador
        {
            Impacto();
            collision.GetComponent<BarraVidaScript>().PerderVida(daño); //pierde vida
            FindObjectOfType<JugadorScript>().Golpe(true); // se ejecuta la animacion de golpe
            DestruirBala();
        }

        if (collision.gameObject.layer == 6) // si el layer es suelo
        {
            Impacto();
            DestruirBala();
        }

        if (collision.gameObject.layer == 9) // si el layer es pared
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
        if (impactoE != null) // si el objeto existe
        {
            GameObject balaimpactar = Instantiate(impactoE, transform.position, Quaternion.identity); // el prefab, en la posicion actual, sin rotacion es igual a bala impactar
            Destroy(balaimpactar, 0.25f);
        }
    }
}
