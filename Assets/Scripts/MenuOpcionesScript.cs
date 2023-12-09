using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MenuOpcionesScript : MonoBehaviour
{
    public AudioMixerGroup audioM; //obtiene la referencia del controlador de audio
    public GameObject menuPrincipal;
    public GameObject mPOpciones;
    public float volumenMusica;

    //Resolution[] resoluciones;

    //public Dropdown resolucionDropdown;

    //private void Start()
    //{
    //    resoluciones = Screen.resolutions; // se obtienen las resoluciones

    //    resolucionDropdown.ClearOptions(); // se limpia las lista

    //    List<string> opciones = new List<string>(); // se crea la lista

    //    for (int i = 0; i < resoluciones.Length; i++) //se inicia la lista
    //    {
    //        string opcion = resoluciones[i].width + " x " + resoluciones[i].height; //se obtienen las resoluciones
    //        opciones.Add(opcion); // se agrega la resolucion
    //    }
    //    resolucionDropdown.AddOptions(opciones); //se agregan las resoluciones a la lista del menu
    //}

    public void configurarVolumen(float volumen)
    {
        audioM.audioMixer.SetFloat("volumenMusica", volumen); //aqui se configura el float para ajustar el "Audio Mixer"        
        volumenMusica = volumen;
    }

    public void VolumenMute()
    {
        audioM.audioMixer.SetFloat("volumenMusica", -80);
    }

    public void VolumenTope()
    {
        audioM.audioMixer.SetFloat("volumenMusica", volumenMusica);
    }

    //public void configurarGraficos(int calidad)
    //{
    //    QualitySettings.SetQualityLevel(calidad); //se referencia el controlador de calidad y luego se configura de acuerdo a la opcion elegida del 0 al 2
    //}

    public void configuracionPantallaCompleta(bool completa)
    {
        AudioScript.instanciar.ReproducirEfectos("aceptar");
        Screen.fullScreen = completa; // la pantalla esta completa por default, para no tenerla asi, se debe dar clock en la opcion
    }

    public void Volver()
    {
        AudioScript.instanciar.ReproducirEfectos("aceptar");
        menuPrincipal.SetActive(true);
        mPOpciones.SetActive(false);
    }
}
