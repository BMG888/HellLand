using UnityEngine;

public class EliminarSonidoScript : MonoBehaviour
{
    public AudioSource audioSourceEliminar; //referencia al audio
    private bool rSFX;

    // Start is called before the first frame update
    void Start()
    {
        audioSourceEliminar = GetComponent<AudioSource>(); // se obtiene el audio
        rSFX = AudioScript.instanciar.reproduciendoSFX;
    }

    // Update is called once per frame
    void Update()
    {
        if(audioSourceEliminar.isPlaying == false && rSFX == true) // si paro de reproducirce
        {            
            Destroy(gameObject); // se destruye el objeto
        }        
    }
}
