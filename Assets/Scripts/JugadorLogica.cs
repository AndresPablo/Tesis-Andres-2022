using System;
using TarodevController;
using UnityEngine;
using System.Collections;
using Viejo;

namespace Demokratos{

    public enum EstadoJugador   {   QUIETO, MUERTO, EN_AIRE, CAMINAR      }
    public enum TipoEnergia     {   NINGUNO, FOSIL, EOLICA, HIDRO, TERMO, SOLAR, COUNT    }

    public class JugadorLogica : MonoBehaviour
    {
        PlayerController controladorMov; // el controlador de TaroDev
        Rigidbody2D rb;
        Collider2D col;
        Animator anim;
        JugadorVisual visual;
        SFX_Jugador misSonidos;
        [SerializeField] ScriptableStats defaultControllerStats;
        [SerializeField] ScriptableStats turboControllerStats;
        public EstadoJugador estado;
        
        // Valores energia
        public TipoEnergia tipoEnergia = TipoEnergia.FOSIL;
        public bool turboMode = false;
        public float energiaMax = 8f;
        public float energiaMaxDisponible = 8f;
        public float energiaInicial = 1f;
        [Tooltip("Cuando esta en modo fosil, pierde energia constantemente.")]
        public float gastoPasivoFosil = .05f;
        [Tooltip("La energia que consume el modo TURBO cada segundo.")]
        [SerializeField] float gastoTurbo = 1f;
        [SerializeField] public float debug_mi_energia;
        public float energiaActual { get; private set; }

        [SerializeField] float tiempoRespawn = 3f;
        public Vector3 SpawnPoint;

        #region EVENTOS de JUGADOR
            // Delegados
            public delegate void JugadorDelegate();
            public delegate void EnergiaDelegate(float nuevoValor);
            public delegate void TipoEnergiaDelegate(TipoEnergia tipo);
            // Eventos
            public static event JugadorDelegate Ev_TurboOn;
            public static event JugadorDelegate Ev_TurboOff;
            public static event JugadorDelegate Ev_Muere;
            public static event JugadorDelegate Ev_Spawnea;
            public static event EnergiaDelegate Ev_OnEnergiaCambia;
            public static event EnergiaDelegate Ev_OnEnergiaMaximaCambia;
            public static event TipoEnergiaDelegate Ev_OnTipoEnergiaCambia;
        #endregion

        #region Funciones INICIALES
            void Start()
            {
                TomarReferencias();
                StartCoroutine(Check_CaidaAlVacio()); // Si las coordenadas son muy grandes, asume que caiste al vacio y mata al jugador
                SetearTipoEnergia(tipoEnergia); // Configura el tipo de energia inicial
            }

            void TomarReferencias()
            {
                rb = GetComponent<Rigidbody2D>();
                col = GetComponent<Collider2D>();
                anim = GetComponent<Animator>();
                controladorMov = GetComponent<PlayerController>();
                visual = GetComponent<JugadorVisual>();
                misSonidos = GetComponent<SFX_Jugador>();
                controladorMov.GroundedChanged += OnPlayerLanded;  
            }

            // Enlaza eventos
            private void OnEnable() {
                Game_Manager_Nuevo.Ev_InvertirGravedad += InvertirGravedad;
                Item_Bateria.Ev_BateriaPickup += PickUpBateria;
                Item_Bateria.Ev_BateriaPickup += Bidon_Nafta_recogido;
            }

            private void OnDisable() {
                Game_Manager_Nuevo.Ev_InvertirGravedad -= InvertirGravedad;
                Item_Bateria.Ev_BateriaPickup -= PickUpBateria;
                Item_Bateria.Ev_BateriaPickup -= Bidon_Nafta_recogido;
            }
        #endregion

        void Update()
        {
            if(tipoEnergia == TipoEnergia.FOSIL)
            {
                AumentarEnergia(-gastoPasivoFosil * Time.deltaTime);
            }

            TurboInput();
            if(energiaActual == 0 && turboMode)
            {
                ApagarTurbo();
            }

            if(turboMode)
            {
                AumentarEnergia(-gastoTurbo * Time.deltaTime);
            }

            debug_mi_energia = energiaActual;

            // Ordena la rotacion del sprite
            if(controladorMov.lastHorizontalVector > 0)
                visual.SetSpriteDirection(false);
            if(controladorMov.lastHorizontalVector < 0)
                visual.SetSpriteDirection(true);
        }

        #region TURBO
            void TurboInput()
            {
                if(estado == EstadoJugador.MUERTO)
                    return;
                // Joystick button 0
                if(Input.GetButtonDown("Turbo"))
            {
                    if(!turboMode && energiaActual >= 1)
                    {
                        // activa el turno porque esta vivo y tiene suficiente energia
                        EncenderTurbo();
                    }else if(turboMode)
                    {
                        ApagarTurbo();
                    }
                }
            }
            
            public void ApagarTurbo()
            {
                if (turboMode)
                    UI_NotificacionFlotante.singleton.CrearEnJugador("TURBO OFF");
            
                turboMode = false;
                controladorMov.SetStats(defaultControllerStats);
                visual.SetEmisionRastro(false);
                AudioManager.instance.PlayOneShot(misSonidos.turboApagar_SFX);
                if (Ev_TurboOff != null)
                    Ev_TurboOff.Invoke();
            }
            
            public void EncenderTurbo()
            {
                turboMode = true;
                controladorMov.SetStats(turboControllerStats);
                visual.SetColorRastro(tipoEnergia);
                visual.SetEmisionRastro(true);
                AudioManager.instance.PlayOneShot(misSonidos.turboActivar_SFX);
                UI_NotificacionFlotante.singleton.CrearEnJugador("TURBO ON!");
                if(Ev_TurboOn != null)
                Ev_TurboOn.Invoke();
            }
        #endregion

        #region ENERGIA
            public void AumentarEnergia(float _valor)
            {
                energiaActual += _valor;

                if(energiaActual <= 0)
                    energiaActual = 0;
                if(energiaActual >= energiaMax)
                    energiaActual = energiaMax;  
                if(energiaActual >= energiaMaxDisponible)
                    energiaActual = energiaMaxDisponible;
            // muestra texto arriba del jugador    

            if (Ev_OnEnergiaCambia != null)
                    Ev_OnEnergiaCambia(energiaActual);    
            }

            public void AumentarEnergia(float _valor, TipoEnergia _tipo)
            {
                if(tipoEnergia == _tipo)
                {
                    UI_NotificacionFlotante.singleton.CrearEnJugador("Recargando...");
                    AumentarEnergia(_valor);
                }
            }

            public void ResetearEnergia()
            {
                SetEnergiaMaximaDisponible(energiaInicial);              
                AumentarEnergia(energiaInicial);
            }

            public void AumentarEnergiaMaximaDisponible(float _cantidad)
            {
                energiaMaxDisponible += _cantidad;
                if(energiaMaxDisponible >= energiaMax)
                {
                    energiaMaxDisponible = energiaMax;  

                }
                if(Ev_OnEnergiaMaximaCambia != null)
                    Ev_OnEnergiaMaximaCambia(energiaMaxDisponible);
                if(Ev_OnEnergiaCambia != null)
                    Ev_OnEnergiaCambia(energiaActual);    
            }

            public void SetEnergiaMaximaDisponible(float _cantidad)
            {
                energiaMaxDisponible = _cantidad;
                if(energiaMaxDisponible >= energiaMax)
                    energiaMaxDisponible = energiaMax;  
                if(Ev_OnEnergiaMaximaCambia != null)
                    Ev_OnEnergiaMaximaCambia(energiaMaxDisponible);
                if(Ev_OnEnergiaCambia != null)
                    Ev_OnEnergiaCambia(energiaActual);    
            }

            public void SetearTipoEnergia(TipoEnergia _tipoEnergia)
            {
                tipoEnergia = _tipoEnergia;
                visual.UpdateColorEnergia(tipoEnergia);
                if(Ev_OnTipoEnergiaCambia != null)
                    Ev_OnTipoEnergiaCambia.Invoke(tipoEnergia);
            }

            public void Bidon_Nafta_recogido(){
                AumentarEnergia(1);
            }
        #endregion

        public void PickUpBateria()
        {
            AumentarEnergiaMaximaDisponible(1);
            AumentarEnergia(0);
        }

        #region VIDA / MUERTE
            public void Matar()
            {
                // Modo turbo te hace invencible
                if (turboMode == true) {
                    return;
                }
                if(estado != EstadoJugador.MUERTO)
                {
                    AudioManager.instance.PlayOneShot(misSonidos.muerte_SFX);
                    ApagarTurbo();
                    estado = EstadoJugador.MUERTO;
                    ToogleControlador(false);
                    if(Ev_Muere != null) 
                        Ev_Muere.Invoke();
                    Game_Manager_Nuevo.singleton.AgregarMuerte();
                    LlamarRespawn();
                }
            }

            public void Spawn()
            {
                Spawn(SpawnPoint);
            }
            
            public void Spawn(Vector3 _spawnPos)
            {
                Teleport(_spawnPos);
                ToogleControlador(true);
                AumentarEnergia(0);
                visual.UpdateColorEnergia(tipoEnergia);
                AudioManager.instance.PlayOneShot(misSonidos.revive_SFX);
                estado = EstadoJugador.QUIETO;
                turboMode = false;
                if(Ev_TurboOff != null) 
                    Ev_TurboOff.Invoke();
                if(Ev_Spawnea != null) 
                    Ev_Spawnea.Invoke();
            }

            void LlamarRespawn()
            {
                Invoke("Spawn", tiempoRespawn);
            }

            // Si las coordenadas son muy grandes, asume que caiste al vacio y mata al jugador
            IEnumerator Check_CaidaAlVacio()
            {
                yield return new WaitForSeconds(2f);
                if(estado != EstadoJugador.MUERTO)
                {
                    if(Vector2.Distance(transform.position, Vector2.zero) > 200)
                    {
                        Matar();
                    }
                }
                // vuelve a iniciar el timer de chequeo
                StartCoroutine("Check_CaidaAlVacio");
            }
        #endregion

        public void ToogleControlador(bool estado)
        {
            controladorMov.enabled = estado;
            rb.simulated = estado;
            if(estado == false)  rb.velocity = Vector2.zero; // frena la vel del rigidbody
            controladorMov.SetExternalForce(Vector2.zero); // quita la fuerza externa del controlador de movimiento
            controladorMov.Frenar(); // Frena la velocidad y el input
        }

        public void Rebotar(float poderRebote)
        {
            //TODO: test
            controladorMov.ForzarSalto(poderRebote);
        }

        public void SetSpawn(Vector3 _spawnPos)
        {
            SpawnPoint = _spawnPos;
        }

        public void Teleport(Vector3 _newPos)
        {
            transform.position = _newPos;
        }

        void InvertirGravedad()
        {
                UI_NotificacionFlotante.singleton.CrearEnJugador("Gravedad Invertida");
            Debug.LogWarning("TODO: no implementado.");
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