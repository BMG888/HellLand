using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CargarNivelScript : MonoBehaviour, InterfaceDataPersistenceScript
{
    public int numeroSiguienteEscenario;

    public void GuardarDatos(ref DatosJuego datos)
    {
        datos.nivelJuego = this.numeroSiguienteEscenario;
    }

    public void CargarDatos(DatosJuego datos)
    {
        return;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject triggerGameObject = collision.gameObject;
        SceneManager.LoadScene(numeroSiguienteEscenario);
        DataPersistenceManagerScript.instanciar.GuardarJuego();
    }
}
