using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace Demokratos.UI{
public class UI_BarraEnergia : MonoBehaviour
{
    [SerializeField] private Image relleno;
    [SerializeField] Image icono;
    float cantidad;

    private void Start() {
        JugadorLogica.Ev_OnEnergiaCambia += UpdateCantidad; 
    }

    public void MostrarCambioTipo(TipoEnergia tipoEnergia, Color color)
    {

    }

    public void UpdateCantidad(float nuevaCantidad)
    {
        cantidad = nuevaCantidad;
        relleno.fillAmount = cantidad/100;
    }
}
}