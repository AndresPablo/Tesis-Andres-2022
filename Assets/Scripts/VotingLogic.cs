using System.Collections;
using System.Collections.Generic;
using Demokratos;
using Demokratos.UI;
using UnityEngine;
using UnityEngine.Events;
using Viejo;


namespace SistemaVotacion{

public enum EstadoVotacion {APAGADO, ESPERA, VOTANDO}

[RequireComponent(typeof(VotingCounter))]
public class VotingLogic : MonoBehaviour
{
    #region  SINGLETON
        public static VotingLogic singleton;
        private void Awake()
        {
            singleton = this;
        }
    #endregion
    // La totalidad de opciones para elegir
    [SerializeField] Acta[] ActasTotales;
    [Space]
    [SerializeField] float duracionVotacion = 4;
    [SerializeField] float tiempoEntreElecciones = 10;
    public EstadoVotacion Estado;
    
    VotingCounter ScriptContador;
    [SerializeField]UI_VotosVisual visual;
    [SerializeField]float tiempoTranscurrido;


    #region EVENTOS de VOTACION
        public delegate void VoidDelegate();
        public delegate void FloatDelegate(float _tiempo);
        public delegate void ResultDelegate(Acta acta);
        public delegate void StateDelegate(bool estado);
        public static event StateDelegate Ev_EstadoVotacion;
        public static event VoidDelegate Ev_Abre;
        public static event FloatDelegate Ev_Cierra;
        public static event ResultDelegate Ev_AplicarResultado;
    #endregion


    void Start()
    {
        ScriptContador = GetComponent<VotingCounter>();
    }


    void Update()
    {
        switch (Estado)
        {
            case EstadoVotacion.VOTANDO:
                tiempoTranscurrido += 1 * Time.deltaTime;
                if(tiempoTranscurrido >= duracionVotacion)
                {
                    AplicarEfecto();
                }
            break;
            case EstadoVotacion.APAGADO:
            break;
            case EstadoVotacion.ESPERA:
                tiempoTranscurrido += 1 * Time.deltaTime;
                if(tiempoTranscurrido >= tiempoEntreElecciones)
                {
                    AbrirVotacion();
                }
            break;
        }
    }

    void AplicarEfecto()
    {
        Acta actaGanadora = GetActaGanadora();
        
        // Cargar los efectos del acta elegida
        if(Ev_AplicarResultado != null)
            Ev_AplicarResultado.Invoke(actaGanadora);
        // Cerramos la votacion
        CerrarVotacion();
    }

    Acta GetActaGanadora()
    {
        return ScriptContador.GetGanadora();
    }


    // toma 2 actas y las envia al Script_Contador para que cuente los votos a cada una
    void SeleccionarActasParaElegir()
    {
        List<Acta> actas = new List<Acta>();

        // Selecciona la primera acta aleatoria
        Acta primeraActa = GetActaRandom();
        actas.Add(primeraActa);

        // Selecciona una segunda acta que no sea igual a la primera
        Acta segundaActa;
        do
        {
            segundaActa = GetActaRandom();
        } while (segundaActa == primeraActa);

        actas.Add(segundaActa);

        ScriptContador.SetActasParaEleccion(actas.ToArray());
        visual.MostrarVotosActuales(actas.ToArray());
    }

    Acta GetActaRandom()
    {
        int i = Random.Range(0, ActasTotales.Length);
        return ActasTotales[i];
    }

    public void SetearEstado(EstadoVotacion _nuevoEstado)
    {
        switch (_nuevoEstado)
        {
            case EstadoVotacion.VOTANDO:
                tiempoTranscurrido = 0;
            break;
            case EstadoVotacion.APAGADO:
            break;
            case EstadoVotacion.ESPERA:
                tiempoTranscurrido = 0;
            break;
        }
        Estado = _nuevoEstado;
    }

    public void AbrirVotacion()
    {
        tiempoTranscurrido = 0;
        SeleccionarActasParaElegir();
        SetearEstado(EstadoVotacion.VOTANDO);
        if(Ev_Abre != null)
        {
            Ev_Abre.Invoke();
        }
    }

    public void CerrarVotacion(bool reabrirLuego = true)
    {
        tiempoTranscurrido = 0;
        // dicta si se va a hacer otra votacion
        if(reabrirLuego)
            SetearEstado(EstadoVotacion.ESPERA);
        if(Ev_Cierra != null)
        {
            Ev_Cierra.Invoke(tiempoEntreElecciones);
        }
    }

    public void ToogleVotacion(bool reiniciarTimer = false)
    {
        if(reiniciarTimer)
            tiempoTranscurrido = 0;
        if(Estado == EstadoVotacion.APAGADO)
            SetearEstado(EstadoVotacion.ESPERA);
            else
            SetearEstado(EstadoVotacion.APAGADO);
    }

    public void ReinicarVotacion(){
        tiempoTranscurrido = 0;
        ToogleVotacion();
    }
}
}