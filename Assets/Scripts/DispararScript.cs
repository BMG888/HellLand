using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DispararScript : MonoBehaviour, InterfaceDataPersistenceScript
{
    public GameObject Bala; // prefab de la bala
    public Transform puntoDisparo; // lugar de donde saldrá la bala    
    public bool nodisparar = false; // variable para restringir el disparo
    private bool recargando = false; // variable para restringir el disparo mientras recarga
    public float ultimoDisparo;
    public int municionDisponible; // municion disponible en total
    public int maxMunicionDisponible; // maximo por municion disponible en total
    public int cantidadMunicionXCaja; // cantidad de municion que se agrega por cada objeto recogido
    public int cartucho; // cantidad de municion por cartucho
    public int maxCartucho; // maximo por cartucho
    public float tiempoRecarga; // tiempo en que durará recargando
    public int totalBalas; // balas en total
    private int recarga; // cantidad de balas que se agregarán por recarga
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();        
    }

    // Update is called once per frame
    void Update()
    {
        totalBalas = cartucho + municionDisponible; // el total de balas es igual a la cantidad que hay en el cartucho y en la municion guardada

        if (gameObject.GetComponent<JugadorScript>().RestringirMovimiento() == false)
        {
            return;
        }

        if (nodisparar == true) // si nodisparar es true
        {
            return; // no realiza ningun movimiento
        }              

        if (Input.GetKeyDown(KeyCode.R) && recargando == false && municionDisponible > 0) // si se preciona r y no esta recargando y la municion es mayor a 0
        {
            StartCoroutine(Recargar());
        }

        if (cartucho == 0 && municionDisponible == 0) // si el cartucho y la municion disponible estan en 0
        {            
            animator.SetBool("Disparar", false);
            return;
        }

        if(recargando == true) // si esta recargando
        {
            animator.SetBool("Disparar", false);
            return;
        }

        if (Input.GetKey(KeyCode.Space) && Time.time > ultimoDisparo + 0.3) // cuando se presiona espacio y el tiempo del ultimo disparo es mayor a 0.3 segundos 
        {           
            Disparar(); // se ejecuta la funcion
            ultimoDisparo = Time.time;
            animator.SetBool("Disparar", true);
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {            
            animator.SetBool("Disparar", false);
        }

        if (cartucho == 0 && recargando == false && municionDisponible > 0) // si el cartucho no tiene balas y no esta recargando y la municion disponible es mayor a 0
        {
            StartCoroutine(Recargar());
        }
    }

    public void Disparar()
    {        
        cartucho--;
        GameObject bala = Instantiate(Bala, puntoDisparo); // se obtiene el prefab de la bala, la cual sale del punto de disparo
        bala.transform.parent = null;                 
    }

    IEnumerator Recargar()
    {
        int mc = maxCartucho;
        recargando = true;        
        yield return new WaitForSeconds(tiempoRecarga);
        AudioScript.instanciar.ReproducirEfectos("recargar");
        if (municionDisponible >= maxCartucho)
        {
            recarga = mc - cartucho;
            municionDisponible -= recarga;
            cartucho += recarga;
        }
        else
        {
            cartucho = municionDisponible;
            municionDisponible = 0;
        }
        recargando = false;
    }

    public void Municion(GameObject item) // se obtiene el gameObject
    {
        if(item.tag == "Bala") // si el tag es igual a balas 
        {
            AudioScript.instanciar.ReproducirEfectos("balas");
            municionDisponible += cantidadMunicionXCaja; // se suma a la municion disponible
            if(municionDisponible > maxMunicionDisponible) // si la municion es mayor a la maxima municion que se puede tener
            {
                municionDisponible = maxMunicionDisponible; // lo que se agregue no será mayor a la maxima municion
            }
        }
    }

    public void CargarDatos(DatosJuego datos)
    {
        this.municionDisponible = datos.municionDisponible;
        this.maxMunicionDisponible = datos.maxMunicionDisponible;
        this.cartucho = datos.cartucho;
        this.maxCartucho = datos.maxCartucho;
        this.cantidadMunicionXCaja = datos.maxXCaja;
        this.tiempoRecarga = datos.tiempoRecarga;
    }

    public void GuardarDatos(ref DatosJuego datos)
    {
        datos.municionDisponible = this.municionDisponible;
        datos.maxMunicionDisponible = this.maxMunicionDisponible;
        datos.cartucho = this.cartucho;
        datos.maxCartucho = this.maxCartucho;
        datos.maxXCaja = this.cantidadMunicionXCaja;
        datos.tiempoRecarga = this.tiempoRecarga;
    }
}
