using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))] // se agrega un box collider al gameObject
public class PEIDCardScript : MonoBehaviour
{
    public Transform puntoMovimiento; // punto al cual se movera la puerta
    private bool abierto = false; // bool para verificar si esta abierto
    public int linkPuertaEId; // id para linkear la puerta con el trigger
    public ParticleSystem descompresion;
    public Transform puntoDescompresion;

    // Start is called before the first frame update
    void Start()
    {
        EventoManagerScript.instanciar.abrirPEIDCardEvento += Abrir; // se suscribe la puerta al evento
    }

    // Update is called once per frame
    void Update()
    {
        if (abierto == true) // si la puerta esta abierta
        {
            transform.position = Vector2.MoveTowards(transform.position, puntoMovimiento.position, 4.5f * Time.deltaTime); // la puerta se mueve desde su punto inicial, hasta el punto de apertura a cierta velocidad
        }
    }

    private void Abrir(int linkElectricaId) // la funcion recibirá una variabe
    {
        if (linkElectricaId == linkPuertaEId) // si el link del trigger es igual al link de la puerta
        {
            StartCoroutine(TiempoDescompresion());
        }
    }

    IEnumerator TiempoDescompresion()
    {
        yield return new WaitForSeconds(0.3f);
        AudioScript.instanciar.ReproducirEfectos("descomprimir");
        Efecto();
        yield return new WaitForSeconds(1.5f);
        AudioScript.instanciar.ReproducirEfectos("puertaElectrica");
        abierto = true; // se abre 
    }

    private void Efecto()
    {
        if (descompresion != null)
        {
            ParticleSystem descomp = Instantiate(descompresion, puntoDescompresion.position, Quaternion.identity);
            descomp.Play();
            Destroy(descomp, 3f);
        }
    }

    private void OnDisable() // en caso de que la puerta se deshabilite
    {
        EventoManagerScript.instanciar.abrirPEIDCardEvento -= Abrir; // de desuscribe al evento
    }
}
