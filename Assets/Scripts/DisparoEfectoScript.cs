using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DisparoEfectoScript : MonoBehaviour
{
    public UnityEvent Disparando;
    
    public void Efecto()
    {
        Disparando?.Invoke();
    }
}
