using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipalScript : MonoBehaviour
{
    public GameObject menuPrincipal;
    public GameObject mPOpciones;    

    public void NuevaPartida()
    {
        AudioScript.instanciar.ReproducirEfectos("aceptar");
        DataPersistenceManagerScript.instanciar.NuevoJuego();
        SceneManager.LoadScene(1); // al dar click, el boton cargará la siguiente escena
    }

    public void Continuar()
    {
        AudioScript.instanciar.ReproducirEfectos("aceptar");
        DataPersistenceManagerScript.instanciar.CargarJuego();

    }

    public void Opciones()
    {
        AudioScript.instanciar.ReproducirEfectos("aceptar");
        menuPrincipal.SetActive(false);
        mPOpciones.SetActive(true);
    }

    public void Salir()
    {
        Application.Quit(); // cierra la aplicación
    }
}
