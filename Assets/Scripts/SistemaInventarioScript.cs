using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SistemaInventarioScript : MonoBehaviour, InterfaceDataPersistenceScript
{
    #region Componentes y UI
    public List<GameObject> items = new List<GameObject>(); // lista de objetos.
    public GameObject ventanaUI; // referencia a la UI
    public Image[] imagenesItems; // lista para guardar las imagenes
    public GameObject ventanaDescripcionUI; // referencia a la venatana de descripcion    
    public Image imagenDescripcion;
    public Image llave1Acticar;
    public Image llave2Acticar;
    public Image idCardActicar;
    public Image barraRecarga;
    public Image barraCartucho;
    public Image barraMunicion;
    public Image barraDaño;
    public Image barraCaja;
    public Sprite desactivado;
    public Sprite activado;
    public Text titulo;
    public Text descripcion;
    public Text Pgrande; // referencia al txt de la UI
    public Text Ppequeña;
    public Text metal;
    public Text totemInventario;
    public Text totemUIJugador;
    public Text cartuchoUIJugador;
    public Text municionDisponibleUIJugador;
    public Text municionDisponibleInventario;
    public Text textoTiempoRecarga;
    public Text textoCantidadXCartucho;
    public Text textoPorcentajeDaño;
    public Text textoCantidadMunicion;
    public Text textoCantidadCaja;
    public Text cantidadMBRecarga; // cantidad de metal(M) en el boton (B)
    public Text cantidadMBCartucho;
    public Text cantidadMBDaño;
    public Text cantidadMBMunicion;
    public Text cantidadMBCaja;
    public Button btnrecarga;
    public Button btncartucho;
    public Button btndaño;
    public Button btnmunicion;
    public Button btncaja;    
    #endregion

    #region Enteros y Floats
    public int contadorPG; // contadores de los objetos
    public int contadorPP;
    public int contadorTotem;
    public int contadorMetal;
    public int contadorLlaves;
    public int nivelRecarga = 0;
    public int nivelCartucho = 0;
    public int nivelDaño = 0;
    public int nivelMunicion = 0;
    public int nivelCaja = 0;
    public float tiempoRecargaMejora;
    public int cartuchoMejora;
    public int dañoMejora;
    public int municionMejora;
    public int cajaMejora;
    public int metalCartucho;
    public int metalRecarga;
    public int metalDaño;
    public int metalMunicion;
    public int metalCaja;    
    #endregion

    #region Booleanos
    public bool abrirInventario = false;
    public bool llave1 = false;
    public bool llave2 = false;
    public bool idCard = false;
    public bool mRecarga = false;
    public bool mCartucho = false;
    public bool mDaño = false;
    public bool mMunicion = false;
    public bool mCaja = false;
    #endregion

    public GameObject Bala;

    // Start is called before the first frame update
    private void Start()
    {
        llave1Acticar.sprite = desactivado;
        llave2Acticar.sprite = desactivado;
        idCardActicar.sprite = desactivado;        
        tiempoRecargaMejora = GetComponent<DispararScript>().tiempoRecarga;
        cartuchoMejora = GetComponent<DispararScript>().maxCartucho;
        dañoMejora = Bala.GetComponent<BalaScript>().daño;
        municionMejora = GetComponent<DispararScript>().maxMunicionDisponible;
        cajaMejora = GetComponent<DispararScript>().cantidadMunicionXCaja;
    }
    // Update is called once per frame
    private void Update()
    {        
        ActualizarTxt(); // se actualizan los textis de los objetos agregados
        ActualizarLLaveIDCard(); // se actualiza el estado de la llaveo targeta
        InfoMRecarga();
        InfoMCartucho();
        InfoMDaño(); // se actualiza la informacion de cada mejora
        InfoMMunicion();
        InfoMCaja();
        CompararContadores(); //se compara si hay suficiente metal para realizar las mejoras
        GetComponent<DispararScript>().tiempoRecarga = tiempoRecargaMejora;
        GetComponent<DispararScript>().maxCartucho = cartuchoMejora;
        Bala.GetComponent<BalaScript>().daño = dañoMejora; //con cada mejora que se haga, se actualizara la respectiva variable en el script indicado
        GetComponent<DispararScript>().maxMunicionDisponible = municionMejora;
        GetComponent<DispararScript>().cantidadMunicionXCaja = cajaMejora;

        if (RestringirInventario() == true)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            ActivarInventario();
        }
    }

    private void ActivarInventario()
    {
        abrirInventario = !abrirInventario; // si es true, se pone false. si es false, se hace true
        ventanaUI.SetActive(abrirInventario); // se activa o se desactiva al contrario de su estado
        ActualizarUI();
    }

    #region Objetos consumibles
    public void Recoger(GameObject item)
    {        
        AudioScript.instanciar.ReproducirEfectos("objetoLore");                
        items.Add(item); // se agrega el item a la lista items
        ActualizarUI();
    }

    public bool InventarioLleno()
    {
        if(items.Count >= imagenesItems.Length) //si el total de items recogidos es mayor o igual al total de espacio en el inventario (imagenes)
        {
            return false; // retornar falso
        }
        else
        {
            return true; // si no verdadero
        }
    }

    private void ActualizarUI()
    {
        OcultarImagenes();

        for(int i = 0; i < items.Count; i++) // por cada item en la lista
        {
            imagenesItems[i].sprite = items[i].GetComponent<SpriteRenderer>().sprite; // busca el sprite en cada campo de la lista
            imagenesItems[i].gameObject.SetActive(true); // activa esa imagen
        }
    }

    private void OcultarImagenes()
    {
        foreach (var i in imagenesItems)
        {
            i.gameObject.SetActive(false); //esconde todas las imagenes en la lista
            OcultarInformacion();
        }
    }

    public void MostrarInformacion(int id)
    {        
        titulo.text = items[id].GetComponent<ItemScript>().nombre; // se obtiene el titulo
        imagenDescripcion.sprite = imagenesItems[id].sprite; // se obtiene la imagen
        descripcion.text = items[id].GetComponent<ItemScript>().descripcion; // se obtiene la descripcion
        titulo.gameObject.SetActive(true);
        imagenDescripcion.gameObject.SetActive(true); // se activan los gameObjects
        descripcion.gameObject.SetActive(true);

    }

    public void OcultarInformacion()
    {        
        titulo.gameObject.SetActive(false);
        imagenDescripcion.gameObject.SetActive(false); // se desactivan los gameObjects
        descripcion.gameObject.SetActive(false);
    }

    public void Consumir(int id)
    {
        if(items[id].GetComponent<ItemScript>().tipoItem == ItemScript.TipoItem.Consumible) // si el item elegido es igual al tipo consumible
        {
            AudioScript.instanciar.ReproducirEfectos("consumir");
            items[id].GetComponent<ItemScript>().consumir.Invoke(); // se activa el evento puesto en el item
            Destroy(items[id], 0.9f); // se destruye el objeto con un pequeño delay para dar tiempo de eliminarlo de la lista
            items.RemoveAt(id); // se elimina de la lista            
            ActualizarUI();
        }
    }
    #endregion

    #region Objetos Agregados
    public void Agregar(GameObject item)
    {
        if(item.tag == "PocionG") // si el tag del item es PocionG
        {
            AudioScript.instanciar.ReproducirEfectos("pocion");
            contadorPG ++; // el contador de la pocion grande aumenta            
        }
        if(item.tag == "PocionP") // si el tag del item es PocionP
        {
            AudioScript.instanciar.ReproducirEfectos("pocion");
            contadorPP ++; // el contador de la pocion pequeña aumenta
        }
        if(item.tag == "Totem") // si el tag del item es totem
        {
            AudioScript.instanciar.ReproducirEfectos("totem");
            contadorTotem ++; // el contador del totem aumenta
        }
        if(item.tag == "MetalP")
        {
            AudioScript.instanciar.ReproducirEfectos("metal");
            contadorMetal ++; // el contador del metal aumenta
        }
        if (item.tag == "MetalM")
        {
            AudioScript.instanciar.ReproducirEfectos("metal");
            contadorMetal += 5; // el contador del metal aumenta en 5
        }
        if (item.tag == "MetalG")
        {
            AudioScript.instanciar.ReproducirEfectos("metal");
            contadorMetal += 10; // el contador del metal aumenta en 10
        }
        if (item.tag == "MetalD")
        {
            AudioScript.instanciar.ReproducirEfectos("metal");
            contadorMetal += 50; // el contador del metal aumenta en 50
        }
        if (item.tag == "Llave")
        {
            AudioScript.instanciar.ReproducirEfectos("llave");
            contadorLlaves ++;
        }
        if(item.tag == "IDCard")
        {
            AudioScript.instanciar.ReproducirEfectos("targeta");
            idCard = true;            
        }
    }

    private void ActualizarTxt()
    {
        string infoPG = contadorPG.ToString(); // se pasa el int del contador a string
        Pgrande.text = infoPG; // esta informacion se para al txt de la ui

        string infoPP = contadorPP.ToString(); // se pasa el int del contador a string
        Ppequeña.text = infoPP; // esta informacion se para al txt de la ui

        string infoTotem = contadorTotem.ToString(); // se pasa el int del contador a string
        totemInventario.text = infoTotem; // esta informacion se para al txt de la ui
        totemUIJugador.text = infoTotem;

        string infoMetal = contadorMetal.ToString(); // se pasa el int del contador a string
        metal.text = infoMetal; // esta informacion se para al txt de la ui

        string infoMDI = GetComponent<DispararScript>().totalBalas.ToString(); // se pasa el int del contador a string
        municionDisponibleInventario.text = infoMDI; // esta informacion se para al txt de la ui

        string infoCartucho = GetComponent<DispararScript>().cartucho.ToString(); // se pasa el int del contador a string
        cartuchoUIJugador.text = infoCartucho; // esta informacion se para al txt de la ui

        string infoMDUIJ = GetComponent<DispararScript>().municionDisponible.ToString(); // se pasa el int del contador a string
        municionDisponibleUIJugador.text = infoMDUIJ; // esta informacion se para al txt de la ui        
    }    

    public void ActualizarLLaveIDCard()
    {
        if(contadorLlaves >= 1)
        {
            llave1 = true;
            llave1Acticar.sprite = activado;
        }
        if(contadorLlaves >= 2)
        {
            llave2 = true;
            llave2Acticar.sprite = activado;
        }
        if(idCard == true)
        {
            idCardActicar.sprite = activado;
        }
    }
    #endregion

    #region Mejora Recarga
    public void InfoMRecarga()
    {
        if(nivelRecarga == 0)
        {            
            barraRecarga.fillAmount = 0;
            metalRecarga = 30;
            cantidadMBRecarga.text = metalRecarga.ToString();
            textoTiempoRecarga.text = tiempoRecargaMejora.ToString() + "s";
        }
        if (nivelRecarga == 1)
        {            
            metalRecarga = 90;
            cantidadMBRecarga.text = metalRecarga.ToString();
            textoTiempoRecarga.text = tiempoRecargaMejora.ToString() + "s";
        }
        if (nivelRecarga == 2)
        {
            metalRecarga = 200;
            cantidadMBRecarga.text = metalRecarga.ToString();
            textoTiempoRecarga.text = tiempoRecargaMejora.ToString() + "s";
        }
        if (nivelRecarga == 3)
        {
            cantidadMBRecarga.text = "Max";
            textoTiempoRecarga.text = tiempoRecargaMejora.ToString() + "s";
        }
    }    

    public void MejoraRecarga()
    {
        if (nivelRecarga == 0)
        {
            if (mRecarga == true)
            {
                AudioScript.instanciar.ReproducirEfectos("mejora");
                nivelRecarga = 1;
                barraRecarga.fillAmount = 0.33f;
                contadorMetal -= metalRecarga;
                tiempoRecargaMejora = 1.5f;
            }
            else
            {
                AudioScript.instanciar.ReproducirEfectos("denegar");
                StartCoroutine(Nometal());
            }
        }
        else if (nivelRecarga == 1)
        {
            if (mRecarga == true)
            {
                AudioScript.instanciar.ReproducirEfectos("mejora");
                nivelRecarga = 2;
                barraRecarga.fillAmount = 0.66f;
                contadorMetal -= metalRecarga;
                tiempoRecargaMejora = 1f;
            }
            else
            {
                AudioScript.instanciar.ReproducirEfectos("denegar");
                StartCoroutine(Nometal());
            }
        }
        else if (nivelRecarga == 2)
        {
            if (mRecarga == true)
            {
                AudioScript.instanciar.ReproducirEfectos("mejora");
                nivelRecarga = 3;
                barraRecarga.fillAmount = 1f;
                contadorMetal -= metalRecarga;
                tiempoRecargaMejora = 0.7f;
            }
            else
            {
                AudioScript.instanciar.ReproducirEfectos("denegar");
                StartCoroutine(Nometal());
            }
        }
        else if (nivelRecarga == 3)
            btnrecarga.enabled = false;
    }
    #endregion

    #region Mejora Cartucho
    public void InfoMCartucho()
    {
        if (nivelCartucho == 0)
        {            
            barraCartucho.fillAmount = 0;
            metalCartucho = 10;
            CambiarTxtCartucho();
        }
        if (nivelCartucho == 1)
        {
            metalCartucho = 20;
            CambiarTxtCartucho();
        }
        if (nivelCartucho == 2)
        {
            metalCartucho = 40;
            CambiarTxtCartucho();
        }
        if (nivelCartucho == 3)
        {
            metalCartucho = 80;
            CambiarTxtCartucho();
        }
        if (nivelCartucho == 4)
        {
            metalCartucho = 150;
            CambiarTxtCartucho();
        }
        if (nivelCartucho == 5)
        {
            metalCartucho = 250;
            CambiarTxtCartucho();
        }
        if (nivelCartucho == 6)
        {            
            cantidadMBCartucho.text = "Max";
            textoCantidadXCartucho.text = cartuchoMejora.ToString();
        }
    }

    public void CambiarTxtCartucho()
    {
        cantidadMBCartucho.text = metalCartucho.ToString();
        textoCantidadXCartucho.text = cartuchoMejora.ToString();
    }

    public void MejoraCartucho()
    {
        if (nivelCartucho == 0)
        {
            if (mCartucho == true)
            {
                AudioScript.instanciar.ReproducirEfectos("mejora");
                nivelCartucho = 1;
                barraCartucho.fillAmount = 0.17f;
                contadorMetal -= metalCartucho;
                cartuchoMejora = 15;
            }
            else
            {
                AudioScript.instanciar.ReproducirEfectos("denegar");
                StartCoroutine(Nometal());
            }
        }
        else if (nivelCartucho == 1)
        {
            if (mCartucho == true)
            {
                AudioScript.instanciar.ReproducirEfectos("mejora");
                nivelCartucho = 2;
                barraCartucho.fillAmount = 0.34f;
                contadorMetal -= metalCartucho;
                cartuchoMejora = 20;
            }
            else
            {
                AudioScript.instanciar.ReproducirEfectos("denegar");
                StartCoroutine(Nometal());
            }
        } 
        else if (nivelCartucho == 2)
        {
            if (mCartucho == true)
            {
                AudioScript.instanciar.ReproducirEfectos("mejora");
                nivelCartucho = 3;
                barraCartucho.fillAmount = 0.51f;
                contadorMetal -= metalCartucho;
                cartuchoMejora = 25;
            }
            else
            {
                AudioScript.instanciar.ReproducirEfectos("denegar");
                StartCoroutine(Nometal());
            }
        }
        else if (nivelCartucho == 3)
        {
            if (mCartucho == true)
            {
                AudioScript.instanciar.ReproducirEfectos("mejora");
                nivelCartucho = 4;
                barraCartucho.fillAmount = 0.68f;
                contadorMetal -= metalCartucho;
                cartuchoMejora = 30;
            }
            else
            {
                AudioScript.instanciar.ReproducirEfectos("denegar");
                StartCoroutine(Nometal());
            }
        }
        else if(nivelCartucho == 4)
        {
            if (mCartucho == true)
            {
                AudioScript.instanciar.ReproducirEfectos("mejora");
                nivelCartucho = 5;
                barraCartucho.fillAmount = 0.85f;
                contadorMetal -= metalCartucho;
                cartuchoMejora = 35;
            }
            else
            {
                AudioScript.instanciar.ReproducirEfectos("denegar");
                StartCoroutine(Nometal());
            }
        }
        else if (nivelCartucho == 5)
        {
            if (mCartucho == true)
            {
                AudioScript.instanciar.ReproducirEfectos("mejora");
                nivelCartucho = 6;
                barraCartucho.fillAmount = 1f;
                contadorMetal -= metalCartucho;
                cartuchoMejora = 40;
            }
            else
            {
                AudioScript.instanciar.ReproducirEfectos("denegar");
                StartCoroutine(Nometal());
            }
        }
        else if (nivelCartucho == 6)
            btncartucho.enabled = false;
    }
    #endregion

    #region Mejora Daño
    public void InfoMDaño()
    {
        if (nivelDaño == 0)
        {
            barraDaño.fillAmount = 0;
            dañoMejora = 5;
            metalDaño = 50;
            cantidadMBDaño.text = metalDaño.ToString();
            textoPorcentajeDaño.text = "4.5%";
        }
        if (nivelDaño == 1)
        {            
            metalDaño = 100;
            cantidadMBDaño.text = metalDaño.ToString();
            textoPorcentajeDaño.text = "9.1%";
        }
        if (nivelDaño == 2)
        {            
            metalDaño = 250;
            cantidadMBDaño.text = metalDaño.ToString();
            textoPorcentajeDaño.text = "18.2%";
        }
        if (nivelDaño == 3)
        {            
            metalDaño = 400;
            cantidadMBDaño.text = metalDaño.ToString();
            textoPorcentajeDaño.text = "36.4%";
        }
        if (nivelDaño == 4)
        {            
            metalDaño = 600;
            cantidadMBDaño.text = metalDaño.ToString();
            textoPorcentajeDaño.text = "63.6%";
        }
        if (nivelDaño == 5)
        {            
            cantidadMBDaño.text = "Max";
            textoPorcentajeDaño.text = "100%";
        }
    }    

    public void MejoraDaño()
    {
        if (nivelDaño == 0)
        {            
            if (mDaño == true)
            {
                AudioScript.instanciar.ReproducirEfectos("mejora");
                nivelDaño = 1;
                barraDaño.fillAmount = 0.2f;
                contadorMetal -= metalDaño;
                dañoMejora = 10;
            }
            else
            {
                AudioScript.instanciar.ReproducirEfectos("denegar");
                StartCoroutine(Nometal());
            }
        }
        else if (nivelDaño == 1)
        {
            if (mDaño == true)
            {
                AudioScript.instanciar.ReproducirEfectos("mejora");
                nivelDaño = 2;
                barraDaño.fillAmount = 0.4f;
                contadorMetal -= metalDaño;
                dañoMejora = 20;
            }
            else
            {
                AudioScript.instanciar.ReproducirEfectos("denegar");
                StartCoroutine(Nometal());
            }
        }
        else if (nivelDaño == 2)
        {
            if (mDaño == true)
            {
                AudioScript.instanciar.ReproducirEfectos("mejora");
                nivelDaño = 3;
                barraDaño.fillAmount = 0.6f;
                contadorMetal -= metalDaño;
                dañoMejora = 40;
            }
            else
            {
                AudioScript.instanciar.ReproducirEfectos("denegar");
                StartCoroutine(Nometal());
            }
        }
        else if (nivelDaño == 3)
        {
            if (mDaño == true)
            {
                AudioScript.instanciar.ReproducirEfectos("mejora");
                nivelDaño = 4;
                barraDaño.fillAmount = 0.8f;
                contadorMetal -= metalDaño;
                dañoMejora = 70;
            }
            else
            {
                AudioScript.instanciar.ReproducirEfectos("denegar");
                StartCoroutine(Nometal());
            }
        }
        else if (nivelDaño == 4)
        {
            if (mDaño == true)
            {
                AudioScript.instanciar.ReproducirEfectos("mejora");
                nivelDaño = 5;
                barraDaño.fillAmount = 1f;
                contadorMetal -= metalDaño;
                dañoMejora = 110;
            }
            else
            {
                AudioScript.instanciar.ReproducirEfectos("denegar");
                StartCoroutine(Nometal());
            }
        }
        else if (nivelDaño == 5)
            btndaño.enabled = false;
    }
    #endregion

    #region Mejora Municion
    public void InfoMMunicion()
    {
        if(nivelMunicion == 0)
        {
            barraMunicion.fillAmount = 0;
            metalMunicion = 5;
            CambiarTxtMunicion();
        }
        if (nivelMunicion == 1)
        {
            metalMunicion = 20;
            CambiarTxtMunicion();
        }
        if (nivelMunicion == 2)
        {
            metalMunicion = 50;
            CambiarTxtMunicion();
        }
        if (nivelMunicion == 3)
        {
            metalMunicion = 120;
            CambiarTxtMunicion();
        }
        if (nivelMunicion == 4)
        {
            metalMunicion = 250;
            CambiarTxtMunicion();
        }
        if (nivelMunicion == 5)
        {
            cantidadMBMunicion.text = "Max";
            textoCantidadMunicion.text = municionMejora.ToString();
        }
    }

    public void CambiarTxtMunicion()
    {
        cantidadMBMunicion.text = metalMunicion.ToString();
        textoCantidadMunicion.text = municionMejora.ToString();
    }

    public void MejoraMunicion()
    {
        if (nivelMunicion == 0)
        {
            if (mMunicion == true)
            {
                AudioScript.instanciar.ReproducirEfectos("mejora");
                nivelMunicion = 1;
                barraMunicion.fillAmount = 0.2f;
                contadorMetal -= metalMunicion;
                municionMejora = 120;
            }
            else
            {
                AudioScript.instanciar.ReproducirEfectos("denegar");
                StartCoroutine(Nometal());
            }
        }
        else if (nivelMunicion == 1)
        {
            if (mMunicion == true)
            {
                AudioScript.instanciar.ReproducirEfectos("mejora");
                nivelMunicion = 2;
                barraMunicion.fillAmount = 0.4f;
                contadorMetal -= metalMunicion;
                municionMejora = 140;
            }
            else
            {
                AudioScript.instanciar.ReproducirEfectos("denegar");
                StartCoroutine(Nometal());
            }
        }
        else if (nivelMunicion == 2)
        {
            if (mMunicion == true)
            {
                AudioScript.instanciar.ReproducirEfectos("mejora");
                nivelMunicion = 3;
                barraMunicion.fillAmount = 0.6f;
                contadorMetal -= metalMunicion;
                municionMejora = 180;
            }
            else
            {
                AudioScript.instanciar.ReproducirEfectos("denegar");
                StartCoroutine(Nometal());
            }
        }
        else if (nivelMunicion == 3)
        {
            if (mMunicion == true)
            {
                AudioScript.instanciar.ReproducirEfectos("mejora");
                nivelMunicion = 4;
                barraMunicion.fillAmount = 0.8f;
                contadorMetal -= metalMunicion;
                municionMejora = 230;
            }
            else
            {
                AudioScript.instanciar.ReproducirEfectos("denegar");
                StartCoroutine(Nometal());
            }
        }
        else if (nivelMunicion == 4)
        {
            if (mMunicion == true)
            {
                AudioScript.instanciar.ReproducirEfectos("mejora");
                nivelMunicion = 5;
                barraMunicion.fillAmount = 1f;
                contadorMetal -= metalMunicion;
                municionMejora = 300;
            }
            else
            {
                AudioScript.instanciar.ReproducirEfectos("denegar");
                StartCoroutine(Nometal());
            }
        }
        else if (nivelMunicion == 5)
            btnmunicion.enabled = false;
    }
    #endregion

    #region Mejora Municion por caja
    public void InfoMCaja()
    {
        if (nivelCaja == 0)
        {
            barraCaja.fillAmount = 0;
            metalCaja = 50;
            CambiarTxtCaja();
        }
        if (nivelCaja == 1)
        {
            metalCaja = 100;
            CambiarTxtCaja();
        }
        if (nivelCaja == 2)
        {
            metalCaja = 200;
            CambiarTxtCaja();
        }
        if (nivelCaja == 3)
        {
            metalCaja = 300;
            CambiarTxtCaja();
        }
        if (nivelCaja == 4)
        {
            cantidadMBCaja.text = "Max";
            textoCantidadCaja.text = cajaMejora.ToString();
        }
    }

    public void CambiarTxtCaja()
    {
        cantidadMBCaja.text = metalCaja.ToString();
        textoCantidadCaja.text = cajaMejora.ToString();
    }

    public void MejoraCaja()
    {
        if (nivelCaja == 0)
        {
            if (mCaja == true)
            {
                AudioScript.instanciar.ReproducirEfectos("mejora");
                nivelCaja = 1;
                barraCaja.fillAmount = 0.25f;
                contadorMetal -= metalCaja;
                cajaMejora = 30;
            }
            else
            {
                AudioScript.instanciar.ReproducirEfectos("denegar");
                StartCoroutine(Nometal());
            }
        }
        else if (nivelCaja == 1)
        {
            if (mCaja == true)
            {
                AudioScript.instanciar.ReproducirEfectos("mejora");
                nivelCaja = 2;
                barraCaja.fillAmount = 0.5f;
                contadorMetal -= metalCaja;
                cajaMejora = 40;
            }
            else
            {
                AudioScript.instanciar.ReproducirEfectos("denegar");
                StartCoroutine(Nometal());
            }
        }
        else if (nivelCaja == 2)
        {
            if (mCaja == true)
            {
                AudioScript.instanciar.ReproducirEfectos("mejora");
                nivelCaja = 3;
                barraCaja.fillAmount = 0.75f;
                contadorMetal -= metalCaja;
                cajaMejora = 50;
            }
            else
            {
                AudioScript.instanciar.ReproducirEfectos("denegar");
                StartCoroutine(Nometal());
            }
        }
        else if (nivelCaja == 3)
        {
            if (mCaja == true)
            {
                AudioScript.instanciar.ReproducirEfectos("mejora");
                nivelCaja = 4;
                barraCaja.fillAmount = 1f;
                contadorMetal -= metalCaja;
                cajaMejora = 60;
            }
            else
            {
                AudioScript.instanciar.ReproducirEfectos("denegar");
                StartCoroutine(Nometal());
            }
        }
        else if (nivelCaja == 4)
            btncaja.enabled = false;
    }
    #endregion

    public void CompararContadores() // se compara la cantidad de metal que se necesita con la que dispone el jugador y asi saber si es posible hacer la mejora o no
    {
        if (contadorMetal >= metalRecarga)
            mRecarga = true;
        else
            mRecarga = false;

        if (contadorMetal >= metalCartucho)
            mCartucho = true;
        else
            mCartucho = false;

        if (contadorMetal >= metalDaño)
            mDaño = true;
        else
            mDaño = false;

        if (contadorMetal >= metalMunicion)
            mMunicion = true;
        else
            mMunicion = false;

        if (contadorMetal >= metalCaja)
            mCaja = true;
        else
            mCaja = false;
    }

    IEnumerator Nometal()
    {
        metal.color = Color.red; // el texto se torna rojo
        yield return new WaitForSeconds(0.2f); // luego de 2 microsegundos 
        metal.color = Color.white; //vuelve blanco
    }

    public void CargarDatos(DatosJuego datos)
    {
        this.contadorTotem = datos.vidas;
        this.contadorPG = datos.contadorPG;
        this.contadorPP = datos.contadorPP;
        this.contadorMetal = datos.contadorMetal;
        this.nivelRecarga = datos.nivelRecarga;
        this.nivelMunicion = datos.nivelMunicion;
        this.nivelCartucho = datos.nivelCartucho;
        this.nivelDaño = datos.nivelDaño;
        this.nivelCaja = datos.nivelCaja;
    }

    public void GuardarDatos(ref DatosJuego datos)
    {
        datos.vidas = this.contadorTotem;
        datos.contadorPG = this.contadorPG;
        datos.contadorPP = this.contadorPP;
        datos.contadorMetal = this.contadorMetal;
        datos.nivelRecarga = this.nivelRecarga;
        datos.nivelMunicion = this.nivelMunicion;
        datos.nivelCartucho = this.nivelCartucho;
        datos.nivelDaño = this.nivelDaño;
        datos.nivelCaja = this.nivelCaja;
    }

    public bool RestringirInventario()
    {
        bool restriccion = false;
        if(FindObjectOfType<ConversacionManagerScript>().conversando == true)
        {
            restriccion = true;
        }
        if(FindObjectOfType<SistemaInteraccionScript>().examinando == true)
        {
            restriccion = true;
        }
        if(FindObjectOfType<MenuPausaScript>().Pausado == true)
        {
            restriccion = true;
        }
        return restriccion;
    }
}
