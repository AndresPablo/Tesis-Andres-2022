using System;
using TarodevController;
using UnityEngine;

namespace Demokratos{

    public enum EstadoJugador   {   QUIETO, MUERTO, EN_AIRE, CAMINAR      }
    public enum TipoEnergia     {   NINGUNO, FOSIL, EOLICA, HIDRO, TERMO, SOLAR, COUNT    }

    public class JugadorLogica : MonoBehaviour
    {
        [SerializeField] PlayerController controladorMov;
        [SerializeField] JugadorVisual visual;
        [SerializeField] SFX_Jugador misSonidos;
        [SerializeField] ScriptableStats defaultControllerStats;
        [SerializeField] ScriptableStats turboControllerStats;
        public EstadoJugador estado;
        
        // Valores energia
        public TipoEnergia tipoEnergia = TipoEnergia.FOSIL;
        public bool turboMode = false;
        public float energiaMax = 100;
        public float energiaInicial = 25;
        [Tooltip("Cuando esta en modo fosil, pierde energia constantemente.")]
        public float gastoPasivoFosil = 5;
        [Tooltip("La energia que consume el modo TURBO cada segundo.")]
        [SerializeField] float gastoTurbo = 10;
        public float energiaActual { get; private set; }

        [SerializeField] float respawnTime = 2f;

        #region EVENTOS de JUGADOR
            public delegate void JugadorDelegate();
            public delegate void EnergiaDelegate(float nuevoValor);
            public delegate void TipoEnergiaDelegate(TipoEnergia tipo);
            public static event JugadorDelegate Ev_Muere;
            public static event JugadorDelegate Ev_Spawnea;
            public static event EnergiaDelegate Ev_OnEnergiaCambia;
            public static event TipoEnergiaDelegate Ev_OnTipoEnergiaCambia;
        #endregion


        void Start()
        {
            Item_Bateria.Ev_Pickup += AumentarEnergia;
            controladorMov.GroundedChanged += OnPlayerLanded;  
            visual.UpdateColorEnergia(tipoEnergia); // se llama al empezar para configurar las visuales del tipo inicial
        }

        void Update()
        {
            if(tipoEnergia == TipoEnergia.FOSIL)
            {
                AumentarEnergia(gastoPasivoFosil * Time.deltaTime);
            }

            TurboInput();
            if(energiaActual <= 0 && turboMode)
            {
                ApagarTurbo();
            }

            if(turboMode)
            {
                AumentarEnergia(-gastoTurbo * Time.deltaTime);
            }

            if(controladorMov.lastHorizontalVector > 0)
            {
                visual.SetSpriteDirection(true);
            }else
            {
                visual.SetSpriteDirection(false);
            }
        }

        void TurboInput()
        {
            if(estado == EstadoJugador.MUERTO)
                return;
            if(Input.GetMouseButtonDown(0)){
                if(!turboMode && energiaActual > 10)
                {
                    // activa el turno porque esta vivo y tiene suficiente energia
                    EncenderTurbo();
                }else if(turboMode)
                {
                    ApagarTurbo();
                }
            }
        }

        public void AumentarEnergia(float _valor)
        {
            energiaActual += _valor;

            if(energiaActual <= 0)
            {
                energiaActual = 0;
            }
            else
            if(energiaActual >= energiaMax)
                energiaActual = energiaMax;  
            if(Ev_OnEnergiaCambia != null)
                Ev_OnEnergiaCambia(energiaActual);
        }

        public void AumentarEnergia(float _valor, TipoEnergia _tipo)
        {
            if(tipoEnergia == _tipo)
            {
                AumentarEnergia(_valor);
            }
        }

        public void PickUpBateria()
        {
            AumentarEnergia(30);
        }

        public void ResetearEnergia()
        {
            energiaActual = 0;
            AumentarEnergia(energiaInicial);
        }

        public void EncenderTurbo()
        {
            turboMode = true;
            controladorMov.SetStats(turboControllerStats);
            visual.SetColorRastro(tipoEnergia);
            visual.SetEmisionRastro(true);
            AudioManager.instance.PlayOneShot(misSonidos.turboActivar_SFX);
        }

        public void ApagarTurbo()
        {
            turboMode = false;
            controladorMov.SetStats(defaultControllerStats);
            visual.SetEmisionRastro(false);
            AudioManager.instance.PlayOneShot(misSonidos.turboApagar_SFX);
        }

        public void Matar()
        {
            AudioManager.instance.PlayOneShot(misSonidos.muerte_SFX);
            ApagarTurbo();
        }

        public void Rebotar(float poderRebote)
        {
            //TODO: test
            controladorMov.ForzarSalto(poderRebote);
        }

        public void SetearTipoEnergia(TipoEnergia _tipoEnergia)
        {
            tipoEnergia = _tipoEnergia;
            visual.UpdateColorEnergia(tipoEnergia);
            if(Ev_OnTipoEnergiaCambia != null)
                Ev_OnTipoEnergiaCambia.Invoke(tipoEnergia);
        }

        public void Spawn(Vector3 _spawnPos)
        {
            Teleport(_spawnPos);
            turboMode = false;
            AumentarEnergia(0);
            visual.UpdateColorEnergia(tipoEnergia);
            AudioManager.instance.PlayOneShot(misSonidos.revive_SFX);
        }

        public void Teleport(Vector3 _newPos)
        {
            transform.position = _newPos;
        }

        private void OnPlayerLanded(bool grounded, float velocity)
        {
            if(!grounded) 
                return;
            AudioManager.instance.PlayOneShot(misSonidos.aterriza_SFX, true);
            if(tipoEnergia == TipoEnergia.TERMO)
                visual.EmitirParticulas_Chispas();
        }


    }

}