using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MenuPausaScript : MonoBehaviour
{
    public AudioMixerGroup musica;
    public AudioMixerGroup sfx;    

    public GameObject menuInicial;
    public GameObject menuOpciones;
    public GameObject pausa;

    public float musicaG;
    public float sfxG;

    public bool Pausado = false;

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (Pausado)
            {
                Continuar();
            }
            else
            {
                Pausa();
            }
        }
    }

    private void Pausa()
    {
        AudioScript.instanciar.ReproducirEfectos("aceptar");
        pausa.SetActive(true);
        Time.timeScale = 0f;
        Pausado = true;
    }    

    public void Continuar()
    {
        AudioScript.instanciar.ReproducirEfectos("aceptar");
        pausa.SetActive(false);
        Time.timeScale = 1f;
        Pausado = false;
    }
    
    public void Opciones()
    {
        AudioScript.instanciar.ReproducirEfectos("aceptar");
        menuInicial.SetActive(false);
        menuOpciones.SetActive(true);
    }

    public void Salir()
    {
        AudioScript.instanciar.ReproducirEfectos("aceptar");
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }            

    public void ConfigurarVolumenMusica(float vMusica)
    {
        musica.audioMixer.SetFloat("volumenMusica", vMusica);
        musicaG = vMusica;
    }

    public void VolumenMusicaMute()
    {
        musica.audioMixer.SetFloat("volumenMusica", -80);
    }

    public void VolumenMusicaTope()
    {
        musica.audioMixer.SetFloat("volumenMusica", musicaG);
    }

    public void ConfigurarVolumenSFX(float vSFX)
    {
        sfx.audioMixer.SetFloat("volumenSFX", vSFX);
        sfxG = vSFX;
    }

    public void VolumenSFXaMute()
    {
        sfx.audioMixer.SetFloat("volumenSFX", -80);
    }

    public void VolumenSFXTope()
    {
        sfx.audioMixer.SetFloat("volumenSFX", sfxG);
    }

    public void ConfigurarPantallaCompleta(bool activar)
    {
        AudioScript.instanciar.ReproducirEfectos("aceptar");
        Screen.fullScreen = activar;
    }

    public void Volver()
    {
        AudioScript.instanciar.ReproducirEfectos("aceptar");
        menuInicial.SetActive(true);
        menuOpciones.SetActive(false);
    }
}
