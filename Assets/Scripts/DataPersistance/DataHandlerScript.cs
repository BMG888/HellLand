using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class DataHandlerScript
{
    private string direccionDatos = "";
    private string nombreArchivoDatos = "";
    private bool encriptar = false;
    private readonly string palabraEncriptar = "Venus";

    public DataHandlerScript(string dirDatos, string nomArchivosD, bool encriptar)
    {
        this.direccionDatos = dirDatos;
        this.nombreArchivoDatos = nomArchivosD;
        this.encriptar = encriptar;
    }

    public DatosJuego Cargar()
    {
        string fullPath = Path.Combine(direccionDatos, nombreArchivoDatos); // se utiliza Path.Combine para diferentes tipos de SO´s
        DatosJuego datosCargados = null; // variable a usar para cargar datos
        if (File.Exists(fullPath)) // revisar si el archivo existe
        {
            try
            {
                // cargar los datos serializados del archivo
                string datosACargar = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open)) // abre los datos para ser leidos
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        datosACargar = reader.ReadToEnd();
                    }
                }
                if (encriptar)
                {
                    datosACargar = EncriptarDesencriptar(datosACargar);
                }
                datosCargados = JsonUtility.FromJson<DatosJuego>(datosACargar); // deserializar los datos de Json a las variables de C#
            }
            catch(Exception e)
            {
                Debug.LogError("Ocurrio un error al tratar de cargar los datos en: " + fullPath + "\n" + e);
            }
        }
        return datosCargados;
    }

    public void Guardar(DatosJuego datos)
    {        
        string fullPath = Path.Combine(direccionDatos, nombreArchivoDatos); // se utiliza Path.Combine para diferentes tipos de SO´s
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath)); // crear la direccion donde se va a guardar el archivo en caso de no existir
            string datosAGuardar = JsonUtility.ToJson(datos, true); // serializar los datos de juego en C# a Json
            if (encriptar)
            {
                datosAGuardar = EncriptarDesencriptar(datosAGuardar);
            }
            using (FileStream stream = new FileStream(fullPath, FileMode.Create)) //escribir los datos serializados al archivo
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(datosAGuardar);
                }
            }
        }
        catch(Exception e)
        {
            Debug.LogError("Ocurrio un error al tratar de guardar los datos en: " + fullPath + "\n" + e);
        }
    }
    private string EncriptarDesencriptar(string datos)
    {
        string datosModificados = "";
        for(int i = 0; i < datos.Length; i++)
        {
            datosModificados += (char)(datos[i] ^ palabraEncriptar[i % palabraEncriptar.Length]);
        }
        return datosModificados;
    }
}
