using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarraVidaScript : MonoBehaviour, InterfaceDataPersistenceScript
{
    public Image barraVida; // referencia a la barra de vida
    public float vidaRestante; // vida que se irá restando o aumentando
        
    public void PerderVida(int vida)
    {
        if (vidaRestante <= 0)
        {
            return;
        }
        vidaRestante -= vida; // se reduce la vida
        barraVida.fillAmount = vidaRestante / 100; // se reduce la vida en la barra del UI        
    }

    public void GanarVida(int vida)
    {        
        vidaRestante += vida; // se suma la vida       
        barraVida.fillAmount = vidaRestante / 100; // se aumenta la vida
        if (vidaRestante > 100) // si la vida al curarse es mayor a 100
        {
            vidaRestante = 100; // se iguala a 100
        }
    }

    public void CargarDatos(DatosJuego datos)
    {
        this.vidaRestante = datos.barraVida;
    }

    public void GuardarDatos(ref DatosJuego datos)
    {
        datos.barraVida = this.vidaRestante;
    }
}