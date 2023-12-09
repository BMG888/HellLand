using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JugadorScript : MonoBehaviour
{
    #region Componentes y GameObjects
    private Rigidbody2D rigidbody2d; // referencia al componente rigidbody
    private Animator animator; // referencia al componente de animación    
    [SerializeField] private Transform checkSueloColision; // objeto para detectar el layer suelo
    [SerializeField] private Transform checkCabezaColision; // objeto para detectar objetos en la cabeza
    [SerializeField] private Transform checkParedColision; //  objeto para detectar el layer pared                                                        
    [SerializeField] private Collider2D colliderparado; // referenciar el colider de pie
    [SerializeField] private Collider2D collideragachado; //referenciar el colider agachado                                                        
    public LayerMask layerSuelo; // referenciar el layer del suelo
    public LayerMask layerPared; // referencia al layer pared
    public ParticleSystem polvo; // referencia a las particulas de polvo
    public ParticleSystem particulaSalto1; // referencia a particulas de salto
    public ParticleSystem particulaSalto2;
    public ParticleSystem particulaDeslizar; // referencia a la particula al deslizarse por la pared
    #endregion

    #region UI
    public GameObject efectoSalto; // referencia a la imagen en la ui para el efecto de salto
    public GameObject efectoVelocidad; // referencia a la imagen en la ui para el efecto de velocidad
    #endregion

    #region variables numericas
    private float horizontal; // variable a utilizar para el tipo de movimiento en el eje x para el personaje       
    const float checkSueloRadio = 0.2f; //radio que tendrá el GameObject CheckSuelo
    const float checkCabezaRadio = 0.2f; //radio que tendrá el GameObject CheckCabeza
    const float checkParedRadio = 0.2f; // radio que tendrá el GameObject CheckPared
    [SerializeField] private float velocidad = 500; //variable para modificar velocidad
    [SerializeField] private float fuerzaSalto = 13; // variable para modificar cuan alto salta
    [SerializeField] private float deslizarse = 0.3f; // variable para caida constante en deslizamiento    
    public float modificadorVelocidad = 2f; // variable para modificar la velocidad
    [SerializeField] private int cantidadSaltos = 2; // variable para identificar cuantos saltos se pueden dar    
    public int saltosRestantes; // variable para identificar cuantos saltos se dieron    
    #endregion

    #region Booleanos
    public bool aterrizar = false; // variable para verificar si el jugador aterrizo al suelo
    public bool agachar = false; // variable para verificar si esta agachado
    public bool saltosMultiples = false; // variable para identificar si se realizo otro salto
    public bool saltoAereo = false; // variable para identificar si se realizó el salto aereo
    public bool deslizando = false; // variable para identificar si se esta deslizando    
    #endregion

    // Start is called before the first frame update
    void Awake() 
    {        
        saltosRestantes = cantidadSaltos;        

        rigidbody2d = GetComponent<Rigidbody2D>(); // obtiene el componente referenciado        
        animator = GetComponent<Animator>();        
    }

    // Update is called once per frame
    void Update()
    {        
        if (RestringirMovimiento() == false)
        {
            return;
        }

        horizontal = Input.GetAxisRaw("Horizontal"); // se obtiene la tecla utilizada para el movimiento en el eje x                              

        if (Input.GetButtonDown("Jump")) // si se presiona la tecla w y se aterrizar es true
        {
            Saltar(); // se llama la función            
        }        
        
        if (Input.GetButtonDown("Crouch")) //si se deja presionado el boton s o abajo, se agacha
        {
            agachar = true;            
        }else if (Input.GetButtonUp("Crouch")) //si no se pone de pie
        {
            agachar = false;
        }

        CheckPared();
    }    

    private void FixedUpdate() //la actualizacion es mas constante
    {
        CheckSuelo();
        Mover(agachar);
    }

    #region Checks
    private void CheckSuelo() // verificar si el GameObject "CheckSuelo" esta colisionando con otros objetos que estan en el layer Suelo
    {
        bool habiaAterrizado = aterrizar;

        aterrizar = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(checkSueloColision.position, checkSueloRadio, layerSuelo); // el collider dependerá de los objetos que haya alrededor del GameObject en la posición, radio y layer
        if(colliders.Length > 0) //si la longitud es mayor a 0
        {
            aterrizar = true; // esta en el suelo
            if (habiaAterrizado == false)
            {
                saltosRestantes = cantidadSaltos; // se resetean los saltos
                saltosMultiples = false;

                AudioScript.instanciar.ReproducirEfectos("aterrizar");
            }            
        }
        else
        {            
            if (habiaAterrizado == true) // si estuvo en el suelo
            {
                StartCoroutine(SaltoAereoDelay()); // se llama el metodo
            }            
        }
        animator.SetBool("Saltar", !aterrizar);
    }

    private void CheckPared()
    {
        if(Physics2D.OverlapCircle(checkParedColision.position, checkParedRadio, layerPared) && Mathf.Abs(horizontal) > 0 && rigidbody2d.velocity.y < 0 && !aterrizar) // si el collider del gameobject CheckPared, en un radio de 0.2f, al layerPared y la direccion en x es mayor a 10 en cualquier direccion y la velocidad en y es mayor a 0 y no esta tocando el suelo
        {          
            if(deslizando == false) // si no se esta deslizando 
            {
                saltosRestantes = cantidadSaltos; // se resetean los saltos restantes
                saltosMultiples = false;
                FindObjectOfType<DispararScript>().nodisparar = false;
            }
            Vector2 velocidad = rigidbody2d.velocity; //se crea un nuevo vector
            velocidad.y = -deslizarse; // al vector se le asigna la variable deslizarse en y
            rigidbody2d.velocity = velocidad; // se modifica la velocidad en el rigidbody2D
            deslizando = true;            
            ParticulasDeslizar(); // se activan las particulas
            animator.SetBool("Deslizar", true);
            FindObjectOfType<DispararScript>().nodisparar = true;

            if (Input.GetButtonDown("Jump"))
            {
                saltosRestantes--; //se resta un salto

                rigidbody2d.velocity = Vector2.up * fuerzaSalto; // al comoponente rigidbody se le agrega una fuerza de salto en el eje y 
                animator.SetBool("Saltar", true); //se activa la animación de salto
                Polvo(); // referencia a la funcion de particula
                AudioScript.instanciar.ReproducirEfectos("saltar1"); //se activa el audio
            }
        }else
        {
            deslizando = false;
            animator.SetBool("Deslizar", false);
        }
    }
    #endregion

    #region Movimiento
    private void Mover(bool agachadoflag)
    {        
        #region Agacharse

        if (!agachadoflag)
        {
            if (Physics2D.OverlapCircle(checkCabezaColision.position, checkCabezaRadio, layerSuelo) && aterrizar == true) // si alrededor del jugador hay un objeto de colision, en la posicion del GameObject CCC, en el radio 0.2f, en el layer suelo
            {
                agachadoflag = true; //continuara agachado
            }
        }

        if (aterrizar == true && agachadoflag == true) // si esta en el suelo y se presiono la tecla asignada
        {
            colliderparado.enabled = false; //el colider de pie se deshabilita
            collideragachado.enabled = true; // el colider agachado se habilita            
        }
        else if (aterrizar == true && agachadoflag == false) // si esta en el suelo y suelta la tecla asignada
        {
            colliderparado.enabled = true; //el colider de pie se habilita
            collideragachado.enabled = false; //el colider agachado se deshabilita            
        }

        if (agachadoflag == true)
        {
            rigidbody2d.velocity = new Vector2(horizontal * velocidad / modificadorVelocidad * Time.fixedDeltaTime, rigidbody2d.velocity.y); // velocidad de movimiento en el eje x dividido a la mitad por agacharse
            FindObjectOfType<DispararScript>().nodisparar = true; // mientras este agachado, no puede disparar
        }
        else if (agachadoflag == false)
        {
            rigidbody2d.velocity = new Vector2(horizontal * velocidad * Time.fixedDeltaTime, rigidbody2d.velocity.y); // velocidad de movimiento en el eje x
            FindObjectOfType<DispararScript>().nodisparar = false; // al estar de pie puede volver a disparar
        }

        animator.SetBool("Agacharse", agachadoflag);
        #endregion

        #region Direccion(vision)
        if (horizontal < 0.0f) //si el eje x es menor a 0.0f (se mueve a la izquierda pulsando la A)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f); //el eje x del localscale se hace negativo
        }
        else
        if (horizontal > 0.0f) //si el eje x es mayor a 0.0f (se mueve a la derecha pulsando la D)
        {
            transform.localScale = new Vector3(1f, 1f, 1f); // el eje x se hace negativo
        }

        animator.SetFloat("Velocidady", rigidbody2d.velocity.y); // se configura el eje y en el animador de acuerdo a la velocidad
        #endregion

        animator.SetFloat("Velocidadx", Mathf.Abs(rigidbody2d.velocity.x)); // en el float del animador, la dependencia sera de acuerdo a la velocidad                                                                                   
    }
    #endregion    

    #region Saltar
    private void Saltar()
    {
        if (aterrizar == true) // si esta en el suelo
        {
            saltosMultiples = true; // se activan los saltos multiples
            saltosRestantes--; //se resta un salto

            rigidbody2d.velocity = Vector2.up * fuerzaSalto; // al comoponente rigidbody se le agrega una fuerza de salto en el eje y 
            animator.SetBool("Saltar", true); //se activa la animación de salto
            Polvo(); // referencia a la funcion de particula
            AudioScript.instanciar.ReproducirEfectos("saltar1"); //se activa el audio
        }
        else
        {
            if (saltoAereo == true)
            {
                saltosMultiples = true; // se activan los saltos multiples
                saltosRestantes--; //se resta un salto

                rigidbody2d.velocity = Vector2.up * fuerzaSalto; // al comoponente rigidbody se le agrega una fuerza de salto en el eje y 
                animator.SetBool("Saltar", true); //se activa la animación de salto
                ParticulasSalto();
                AudioScript.instanciar.ReproducirEfectos("impulso"); //se activan audios
                AudioScript.instanciar.ReproducirEfectos("saltar2");
            }

            if (saltosMultiples && saltosRestantes > 0) // si saltos multiples es verdadero y saltosRestantes son mayores a 0
            {
                saltosRestantes--; //se resta un salto

                rigidbody2d.velocity = Vector2.up * fuerzaSalto; // al comoponente rigidbody se le agrega una fuerza de salto en el eje y 
                animator.SetBool("Saltar", true); //se activa la animación de salto
                ParticulasSalto();
                AudioScript.instanciar.ReproducirEfectos("impulso"); // se activan los audios
                AudioScript.instanciar.ReproducirEfectos("saltar2");
            }
        }
    }    

    IEnumerator SaltoAereoDelay() //metodo para realizar pausas
    {
        saltoAereo = true; // se activa el salto aereo
        yield return new WaitForSeconds(0.2f); // luego de dos segundos
        saltoAereo = false; // se desactiva
    }
    #endregion

    public bool RestringirMovimiento()
    {
        bool moverse = true;

        if (FindObjectOfType<SistemaInteraccionScript>().examinando)
        {
            moverse = false;
        }
        if (FindObjectOfType<SistemaInventarioScript>().abrirInventario)
        {
            moverse = false;
        }
        if (FindObjectOfType<ConversacionManagerScript>().conversando)
        {
            moverse = false;
        }
        if (FindObjectOfType<MenuPausaScript>().Pausado)
        {
            moverse = false;
        }
        return moverse;
    }

    #region Efectos pociones
    public void DuplicarAlturaSalto()
    {
        fuerzaSalto = 26; // se ducplica el valor del salto
        efectoSalto.SetActive(true); // se activa el sprite en la ui
        StartCoroutine(ResetearFuerzaSalto()); // empieza la corrutina
    }
    
    IEnumerator ResetearFuerzaSalto()
    {
        yield return new WaitForSeconds(10); // delay de 10 segundos
        fuerzaSalto = 13; // salto vuelve a la normalidad
        efectoSalto.SetActive(false); // se desactiva el sprite en la ui
    }

    public void AumentarVelocidad()
    {
        velocidad = 750; //la velocidad aumenta un 50%
        efectoVelocidad.SetActive(true); // se activa el sprite en la ui
        StartCoroutine(ResetearVelocidad()); // empieza la corrutina
    }

    IEnumerator ResetearVelocidad()
    {
        yield return new WaitForSeconds(10); // delay de 10 segundos
        velocidad = 500; // velocidad vuelve a la normalidad
        efectoVelocidad.SetActive(false); // se desactiva el sprite en la ui
    }
    #endregion

    public void Paso1() // referenciado en en el animation
    {
        AudioScript.instanciar.ReproducirEfectos("caminar1"); // audio de paso1
    }

    public void Paso2()
    {
        AudioScript.instanciar.ReproducirEfectos("caminar2"); // audio de paso2
    }

    #region Particulas
    public void Polvo()
    {
        polvo.Play(); // se activan las particulas
    }

    public void ParticulasSalto()
    {
        particulaSalto1.Play(); // se activan las particulas
        particulaSalto2.Play();
    }

    public void ParticulasDeslizar()
    {
        particulaDeslizar.Play(); // se activan las particulas
    }
    #endregion

    public void Golpe(bool golpe)
    {
        if(golpe == true)
        {
            animator.SetBool("Herir", true);
            AudioScript.instanciar.ReproducirEfectos("Herido");
            StartCoroutine(GolpeDelay());
        }        
    }

    IEnumerator GolpeDelay()
    {
        yield return new WaitForSeconds(0.2f);
        animator.SetBool("Herir", false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(checkSueloColision.position, checkSueloRadio);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(checkCabezaColision.position, checkCabezaRadio);
        Gizmos.color = Color.grey;
        Gizmos.DrawSphere(checkParedColision.position, checkParedRadio);
    }    
}
