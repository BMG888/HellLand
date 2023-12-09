using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DatosJuego
{
    // se recrean las variables a guardar del juego
    public int nivelJuego; // listo
    public int dañoBala; // listo
    public float barraVida; //listo
    public int vidas; // listo
    public int municionDisponible; // listo
    public int maxMunicionDisponible; // listo
    public int cartucho; // listo
    public int maxCartucho; // listo
    public int maxXCaja; // listo
    public float tiempoRecarga; // listo
    public int contadorPG; // listo
    public int contadorPP; // listo
    public int contadorMetal; // listo
    public int nivelRecarga; // listo
    public int nivelCartucho; // listo
    public int nivelDaño; // listo
    public int nivelMunicion; // listo
    public int nivelCaja; // listo

    public DatosJuego()
    {
        // se inicia cada variable a su valor inicial antes de empezar nueva partida
        this.nivelJuego = 0;
        this.dañoBala = 5;
        this.barraVida = 100;
        this.vidas = 0;
        this.municionDisponible = 0;
        this.maxMunicionDisponible = 100;
        this.cartucho = 0;
        this.maxCartucho = 10;
        this.maxXCaja = 20;
        this.tiempoRecarga = 2f;
        this.contadorPG = 0;
        this.contadorPP = 0;
        this.contadorMetal = 0;
        this.nivelRecarga = 0;
        this.nivelCartucho = 0;
        this.nivelDaño = 0;
        this.nivelMunicion = 0;
        this.nivelCaja = 0;
    }
}
