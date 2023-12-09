using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SignalPNScript : MonoBehaviour
{
    public int numeroSiguienteEscenario;

    public void SiguienteNivel()
    {
        SceneManager.LoadScene(numeroSiguienteEscenario);
    }
}
