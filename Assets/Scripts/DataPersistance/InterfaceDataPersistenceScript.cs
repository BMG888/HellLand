using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface InterfaceDataPersistenceScript
{
    void CargarDatos(DatosJuego datos); // toma los datos de juego
    void GuardarDatos(ref DatosJuego datos); // se referencia para que cuando los scripts modifiquen los datos, estos puedan ser guardados
}
