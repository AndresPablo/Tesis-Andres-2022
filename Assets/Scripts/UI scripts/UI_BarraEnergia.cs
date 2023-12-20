using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using Viejo;

namespace Demokratos.UI{
public class UI_BarraEnergia : MonoBehaviour
{
    [SerializeField] private Image relleno_L;
    [SerializeField] private Image relleno_R;
    [SerializeField] Image fondo_texto;
    [SerializeField] Image fondo_ico;
    [SerializeField] Image icono;
    [SerializeField] float tiempoParaEsconder = 5f;
    [SerializeField]float tiempoInactividad;
    float cantidad;
    [SerializeField]bool oculto;


    private void Start() {
        JugadorLogica.Ev_OnEnergiaCambia += UpdateCantidad; 
        JugadorLogica.Ev_OnTipoEnergiaCambia += MostrarCambioTipo; 
        JugadorLogica.Ev_TurboOn += Mostrar;
        Mostrar();
    }

    public void MostrarCambioTipo(TipoEnergia tipoEnergia)
    {
        if(oculto)
            Mostrar();
        Color nuevoColor = Game_Manager_Nuevo.singleton.Interfaz.paletaColores.GetColorEnergia(tipoEnergia);
        fondo_ico.color = nuevoColor;
        LeanTween.rotateAround(fondo_ico.gameObject, Vector3.up, 360f, 1f).setOnComplete( () => {
                relleno_L.color = relleno_R.color = nuevoColor;
                fondo_texto.color = nuevoColor;
         });; 
    }

    public void UpdateCantidad(float nuevaCantidad)
    {
        cantidad = nuevaCantidad;
        relleno_L.fillAmount = cantidad/100;
        relleno_R.fillAmount = cantidad/100;
        tiempoInactividad = 0;
        if(oculto && tiempoInactividad > 1f)
        {
            Mostrar();
        }
             
    }

    void Update()
    {
        if(oculto)
        {
            tiempoInactividad += Time.deltaTime;
        }
    }

    public void Esconder()
    {
        //gameObject.GetComponent<CanvasGroup>().alpha = 0;
        LeanTween.alpha(gameObject, 0f, 2f);
        oculto = true;
    }

    public void Mostrar()
    {
        LeanTween.alpha(gameObject, 1f, 2f);
        oculto = false;
        tiempoInactividad = 0;
        Invoke("Esconder",tiempoParaEsconder);
    }
}
}