using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; // busca los objetos de InterfaceDataPersistence

public class DataPersistenceManagerScript : MonoBehaviour
{
    [Header("Configurar almacenamiento de datos")]
    [SerializeField] private string nombreArchivo;
    [SerializeField] private bool encriptar;

    private DatosJuego datosJuego; // se obtiene la clase
    private List<InterfaceDataPersistenceScript> objetosDataPersistence; // lista de datos a manipular
    private DataHandlerScript dataHandler; // se obtiene la clase

    public static DataPersistenceManagerScript instanciar { get; private set; } // publicamente se pueden obtener los datos, pero modificarlos solo de forma privada

    private void Awake()
    {
        if (instanciar != null) // si ya existe un DataManager
        {
            Debug.LogError("Se encontro mas de un gestor de datos en esta escena.");
        }
        instanciar = this; // si no se instancia
    }

    public void Start()
    {
        this.dataHandler = new DataHandlerScript(Application.persistentDataPath, nombreArchivo, encriptar); // se encarga de la permanencia de datos en Unity
        this.objetosDataPersistence = FindAllDataPersistenceObjects();
        CargarJuego();
    }

    public void NuevoJuego()
    {
        this.datosJuego = new DatosJuego(); // se crean datos nuevos
    }

    public void CargarJuego()
    {
        // se carga cualquier dato guardado de algun archivo utilizando el manipulador de datos (DataHandler)
        this.datosJuego = dataHandler.Cargar();
        // si no hay ningun dato se inicia un juego nuevo
        if (this.datosJuego == null)
        {
            Debug.Log("No se encontraron datos, se inicia nuevo juego.");
            NuevoJuego();
        }
        //poner los datos de juego donde se requiera
        foreach(InterfaceDataPersistenceScript objDataPersistence in objetosDataPersistence)
        {
            objDataPersistence.CargarDatos(datosJuego);
        }
    }

    public void GuardarJuego()
    {
        // pasar los datos a los demas scripts para que refresquen datos
        foreach (InterfaceDataPersistenceScript objDataPersistence in objetosDataPersistence)
        {
            objDataPersistence.GuardarDatos(ref datosJuego);
        }
        //guardar los datos a un archivo utilizando el manipulador de datos (DataHandler)
        dataHandler.Guardar(datosJuego);
    }

    private List<InterfaceDataPersistenceScript> FindAllDataPersistenceObjects()
    {
        // por usar Linq se puede encontrar "InterfaceDataScript" en cada script con Monobehaviour (es decir, que sea un componente del juego)
        IEnumerable<InterfaceDataPersistenceScript> objetosDataPersistence = FindObjectsOfType<MonoBehaviour>().OfType<InterfaceDataPersistenceScript>();
        return new List<InterfaceDataPersistenceScript>(objetosDataPersistence);
    }
}
