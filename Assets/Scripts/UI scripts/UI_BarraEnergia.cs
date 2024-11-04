using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace Demokratos.UI{
public class UI_BarraEnergia : MonoBehaviour
{
    [SerializeField] Image fondo_texto;
    [SerializeField] Image fondo_barra;
    [SerializeField] Image fondo_ico;
    [SerializeField] Image icono;
    [SerializeField] float tiempoParaEsconder = 5f;
    [SerializeField]float cantidad;
    [SerializeField]float cantidad_maxima;
    private float ultimaCantidad = -0;
    private float ultimaCantidadMaxima = -0;
    [SerializeField] private float velocidadLlenado = 0.5f;  // Controla la velocidad de llenado de la última batería
    private float ultimaCantidadLlenado;  // Almacena el progreso de la última batería
    private float cantidadInterpolada;  // Valor que incrementa gradualmente hasta alcanzar `cantidad`


    [SerializeField] private Image[] ui_baterias_obtenidas;  // Bloques que componen la barra de energía
    [SerializeField] private Image[] ui_baterias_faltantes;  // Bloques que componen la barra de energía
    [SerializeField] TextMeshProUGUI textoBarra;
    Color color_energia_actual = Color.white;
    UI_RefeColores paleta;


    private void Start() {
        JugadorLogica.Ev_OnEnergiaCambia += UpdateCantidad; 
        JugadorLogica.Ev_OnEnergiaMaximaCambia += UpdateCantidad_Maxima; 
        JugadorLogica.Ev_OnTipoEnergiaCambia += MostrarCambioTipo; 
        JugadorLogica.Ev_TurboOn += Mostrar;
        paleta = Game_Manager_Nuevo.singleton.Interfaz.paletaColores;
        EsconderTodasBaterias();
        Mostrar();
    }

    public void MostrarCambioTipo(TipoEnergia tipoEnergia)
    {
        color_energia_actual = Game_Manager_Nuevo.singleton.Interfaz.paletaColores.GetColorEnergia(tipoEnergia);
        fondo_ico.color = color_energia_actual;
        icono.color = color_energia_actual;
        icono.sprite = Game_Manager_Nuevo.singleton.Interfaz.paletaColores.GetIconoEnergia(tipoEnergia);
        LeanTween.rotateAround(fondo_ico.gameObject, Vector3.up, 360f, 1f).setOnComplete( () => 
        {
            fondo_texto.color = color_energia_actual;
        });; 
    }
         
   private void Update()
    {
        // Interpolación gradual hacia el valor objetivo de `cantidad`
        if (Mathf.Abs(cantidadInterpolada - cantidad) > 0.01f)  // Pequeño umbral para evitar oscilaciones
        {
            cantidadInterpolada = Mathf.Lerp(cantidadInterpolada, cantidad, velocidadLlenado * Time.deltaTime);
            UpdateVisual();  // Actualizamos visualmente cada vez que `cantidadInterpolada` cambia
        }
    }

    // Método para actualizar visualmente la barra de energía
    public void UpdateVisual()
    {
        int i = 0;
        int cantidadRedondeada = Mathf.FloorToInt(cantidadInterpolada);

        // Baterías completamente llenas
        for (; i < cantidadRedondeada && i < ui_baterias_obtenidas.Length; i++)
        {
            ui_baterias_obtenidas[i].enabled = true;
            ui_baterias_obtenidas[i].color = color_energia_actual;
            ui_baterias_obtenidas[i].fillAmount = 1.0f; // Completamente llena
        }

        // Llenado progresivo de la última batería (si hay un valor decimal)
        if (i < cantidad && i < ui_baterias_obtenidas.Length)
        {
            ui_baterias_obtenidas[i].enabled = true;
            float porcentajeLlenado = cantidadInterpolada - cantidadRedondeada;
            ui_baterias_obtenidas[i].color = color_energia_actual;
            ui_baterias_obtenidas[i].fillAmount = porcentajeLlenado; // Usar fillAmount para llenado parcial
        }
        
        // Baterías vacías después de la última llena
        for (; i < cantidad_maxima && i < ui_baterias_obtenidas.Length; i++)
        {
            ui_baterias_obtenidas[i].enabled = true;
            ui_baterias_obtenidas[i].color = paleta.colorGrisApagado;
            ui_baterias_obtenidas[i].fillAmount = 1.0f; // Marcada como vacía pero habilitada
        }

        // Ocultar baterías fuera del rango de cantidad_maxima
        for (; i < ui_baterias_obtenidas.Length; i++)
        {
            ui_baterias_obtenidas[i].enabled = false;
        }
    }

    public void UpdateCantidad(float _nuevaCantidad)
    {
        if (_nuevaCantidad != ultimaCantidad)
        {
            cantidad = _nuevaCantidad;
            ultimaCantidad = cantidad;
            
            // Evita que cantidadInterpolada vuelva a bajar (si `cantidad` disminuye)
            if (cantidadInterpolada > cantidad) cantidadInterpolada = cantidad;

            UpdateVisual();  // Actualiza visualmente en cada cambio
        }
    }

    void UpdateCantidad_Maxima(float _nuevaCantidadMaxima)
    {
        if (_nuevaCantidadMaxima != ultimaCantidadMaxima)
        {
            cantidad_maxima = _nuevaCantidadMaxima;
            ultimaCantidadMaxima = cantidad_maxima;
            UpdateVisual();
        }
    }

    // Método para actualizar el texto de recarga
    private void ActualizarTextoRecargando(bool recargando)
    {
        textoBarra.text = recargando ? "Recargando..." : "";
    }

    public void Esconder()
    {
        //gameObject.GetComponent<CanvasGroup>().alpha = 0;
        LeanTween.alpha(gameObject, 0f, 2f);
        //oculto = true;
    }

    public void Mostrar()
    {
        LeanTween.alpha(gameObject, 1f, 2f);
        UpdateVisual();
        //oculto = false;
        //tiempoInactividad = 0;
        Invoke("Esconder",tiempoParaEsconder);
    }

    void EsconderTodasBaterias()
    {
        foreach(Image bat in ui_baterias_obtenidas){
            bat.enabled = false;
        }
    }
}
}