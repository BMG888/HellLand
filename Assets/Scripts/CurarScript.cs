using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurarScript : MonoBehaviour
{
    public ParticleSystem sanar;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (FindObjectOfType<BarraVidaScript>().vidaRestante <= 0 && FindObjectOfType<SistemaInventarioScript>().contadorTotem > 0) // si la vida restante es menor o igual a 0 y aun hay totems en el inventario
        {
            AudioScript.instanciar.ReproducirEfectos("curar");
            ParticulaSanar();            
            gameObject.GetComponent<SistemaInventarioScript>().contadorTotem--; // decrese la cantidad de vidas 
            gameObject.GetComponent<BarraVidaScript>().vidaRestante = 100; // sube toda la vida
            gameObject.GetComponent<BarraVidaScript>().barraVida.fillAmount = 1; // la barra de vida sube al maximo
        }

        if(gameObject.GetComponent<JugadorScript>().RestringirMovimiento() == false)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            UsarPP();
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            UsarPG();
        }
    }

    public void UsarPP()
    {
        if(gameObject.GetComponent<SistemaInventarioScript>().contadorPP <= 0 || gameObject.GetComponent<BarraVidaScript>().vidaRestante == 100) // si no hay pociones de vida restante o la barra de vida es igual a 100
        {
            return; // retorna y no procesa lo que esta debajo de estas lineas
        }
        AudioScript.instanciar.ReproducirEfectos("curar");
        ParticulaSanar();
        gameObject.GetComponent<SistemaInventarioScript>().contadorPP--; // el contador de pociones pequeñas baja
        gameObject.GetComponent<BarraVidaScript>().GanarVida(20); // la vida aumenta en 20
    }

    public void UsarPG()
    {
        if(gameObject.GetComponent<SistemaInventarioScript>().contadorPG <= 0 || gameObject.GetComponent<BarraVidaScript>().vidaRestante == 100) // si no hay pociones de vida restante o la barra de vida es igual a 100
        {
            return; // retorna y no procesa lo que esta debajo de estas lineas
        }
        AudioScript.instanciar.ReproducirEfectos("curar");
        ParticulaSanar();
        gameObject.GetComponent<SistemaInventarioScript>().contadorPG--; // el contador de pociones grandes baja
        gameObject.GetComponent<BarraVidaScript>().GanarVida(50); // la vida aumenta en 50
    }

    public void ParticulaSanar()
    {
        sanar.Play();
    }
}
