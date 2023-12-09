using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioScript : MonoBehaviour
{
    public static AudioScript instanciar; //para poder ser utilizado en cualquier script
    
    [Header("SFX Jugador")]
    public AudioClip saltar1, saltar2, impulso, aterrizar, caminar1, caminar2, disparo, herido, curar, recargar;

    [Header("SFX Objetos")]
    public AudioClip objetoLore, metal, totem, pocion, balas, porton, interruptor, descomprimir, llave, puertaElectrica, targeta;

    [Header("SFX Enemigos")]
    public AudioClip srp1, srp2, srmuerte;

    [Header("SFX UI")]
    public AudioClip tomarPocion, mejorar, denegado, aceptar;
    
    [Header("Musica Background")]
    public AudioClip bgNivel1, bgNivel2;

    [Header("Prefab Audio")]
    public GameObject objetoAudio; 

    [Header("AudioMixer")]
    public AudioMixerGroup amSFX;
    public AudioMixerGroup amMusica;   

    public GameObject objetoReproduciendo; //musica que se estará reproduciendo actualmente

    public bool reproduciendoSFX = false;

    private void Awake()
    {
        instanciar = this; //al iniciar, se instancia
    }

    public void ReproducirEfectos(string tituloSFX) //lista de efectos, estos serán elegidos por su nombre
    {
        switch (tituloSFX)
        {
            case "saltar1":
                GameObjectSFX(saltar1);
                break;

            case "saltar2":
                GameObjectSFX(saltar2);
                break;

            case "impulso":
                GameObjectSFX(impulso);
                break;

            case "aterrizar":
                GameObjectSFX(aterrizar);
                break;

            case "caminar1":
                GameObjectSFX(caminar1);
                break;

            case "caminar2":
                GameObjectSFX(caminar2);
                break;

            case "Herido":
                GameObjectSFX(herido);
                break;

            case "pocion":
                GameObjectSFX(pocion);
                break;

            case "objetoLore":
                GameObjectSFX(objetoLore);
                break;

            case "consumir":
                GameObjectSFX(tomarPocion);
                break;

            case "disparo":
                GameObjectSFX(disparo);
                break;

            case "metal":
                GameObjectSFX(metal);
                break;

            case "totem":
                GameObjectSFX(totem);
                break;

            case "curar":
                GameObjectSFX(curar);
                break;

            case "srp1":
                GameObjectSFX(srp1);
                break;

            case "srp2":
                GameObjectSFX(srp2);
                break;

            case "srmuerte":
                GameObjectSFX(srmuerte);
                break;

            case "recargar":
                GameObjectSFX(recargar);
                break;

            case "balas":
                GameObjectSFX(balas);
                break;

            case "porton":
                GameObjectSFX(porton);
                break;

            case "interruptor":
                GameObjectSFX(interruptor);
                break;

            case "descomprimir":
                GameObjectSFX(descomprimir);
                break;

            case "puertaElectrica":
                GameObjectSFX(puertaElectrica);
                break;

            case "llave":
                GameObjectSFX(llave);
                break;

            case "targeta":
                GameObjectSFX(targeta);
                break;

            case "mejora":
                GameObjectSFX(mejorar);
                break;

            case "denegar":
                GameObjectSFX(denegado);
                break;

            case "aceptar":
                GameObjectSFX(aceptar);
                break;

            default:
                break;
        }
    }

    private void GameObjectSFX(AudioClip ac)
    {
        GameObject nuevoAudio = Instantiate(objetoAudio, transform); //se referencia el gameObject, (instantiate)se crea un objeto de un prefab existente, en el campo gameOject SFX
        nuevoAudio.GetComponent<AudioSource>().outputAudioMixerGroup = amSFX; // se le asigna el audioMixerGroup
        nuevoAudio.GetComponent<AudioSource>().clip = ac; //asignar el audioclip al audiosource
        nuevoAudio.GetComponent<AudioSource>().Play(); //reproducir el audio
        reproduciendoSFX = true;
    }

    public void ReproducirMusica(string tituloMusica)
    {
        switch (tituloMusica)
        {
            case "Nivel1":
                GameobjectMusica(bgNivel1);
                break;

            case "Nivel2":
                GameobjectMusica(bgNivel2);
                break;

            default:
                break;
        }
    }

    private void GameobjectMusica(AudioClip ac)
    {
        if (objetoReproduciendo == true) // si hay una cancion que se está reproduciendo actualmente
        {
            Destroy(objetoReproduciendo); // se destruye (sirve para poder incorporar diferentes audios en el juego dependiendo de la situación)
        }
        objetoReproduciendo = Instantiate(objetoAudio, transform); //se referencia el gameObject, (instantiate)se crea un objeto de un prefab existente, en el campo gameObject Musica
        objetoReproduciendo.GetComponent<AudioSource>().outputAudioMixerGroup = amMusica; // se le asigna el audioMixerGroup
        objetoReproduciendo.GetComponent<AudioSource>().clip = ac; //asignar el audioclip al audiosource
        objetoReproduciendo.GetComponent<AudioSource>().loop = true; // se activa el modo loop (se repite una y otra vez)
        objetoReproduciendo.GetComponent<AudioSource>().Play(); //reproducir el audio        
    }
}
