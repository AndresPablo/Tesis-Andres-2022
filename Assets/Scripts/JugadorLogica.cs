using System;
using TarodevController;
using UnityEngine;
using System.Collections;

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
        public float energiaMax = 100;
        public float energiaInicial = 25;
        [Tooltip("Cuando esta en modo fosil, pierde energia constantemente.")]
        public float gastoPasivoFosil = 5;
        [Tooltip("La energia que consume el modo TURBO cada segundo.")]
        [SerializeField] float gastoTurbo = 10;
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
            public static event TipoEnergiaDelegate Ev_OnTipoEnergiaCambia;
        #endregion

        #region Funciones INICIALES
            void Start()
            {
                TomarReferencias();
                StartCoroutine(Check_CaidaAlVacio()); // Si las coordenadas son muy grandes, asume que caiste al vacio y mata al jugador
                visual.UpdateColorEnergia(tipoEnergia); // se llama al empezar para configurar las visuales del tipo inicial
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
                Item_Bateria.Ev_Pickup += AumentarEnergia;
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
            
            public void ApagarTurbo()
            {
                turboMode = false;
                controladorMov.SetStats(defaultControllerStats);
                visual.SetEmisionRastro(false);
                AudioManager.instance.PlayOneShot(misSonidos.turboApagar_SFX);
                if(Ev_TurboOff != null)
                    Ev_TurboOff.Invoke();
            }
            
            public void EncenderTurbo()
            {
                turboMode = true;
                controladorMov.SetStats(turboControllerStats);
                visual.SetColorRastro(tipoEnergia);
                visual.SetEmisionRastro(true);
                AudioManager.instance.PlayOneShot(misSonidos.turboActivar_SFX);
                if(Ev_TurboOn != null)
                    Ev_TurboOn.Invoke();
            }
        #endregion

        #region ENERGIA
            public void AumentarEnergia(float _valor)
            {
                energiaActual += _valor;

                if(energiaActual <= 0)
                {
                    energiaActual = 0;
                }
                else
                {
                    if(energiaActual >= energiaMax)
                        energiaActual = energiaMax;  
                    if(Ev_OnEnergiaCambia != null)
                        Ev_OnEnergiaCambia(energiaActual);
                }
            }

            public void AumentarEnergia(float _valor, TipoEnergia _tipo)
            {
                if(tipoEnergia == _tipo)
                {
                    AumentarEnergia(_valor);
                }
            }

            public void ResetearEnergia()
            {
                energiaActual = 0;
                AumentarEnergia(energiaInicial);
            }

            public void SetearTipoEnergia(TipoEnergia _tipoEnergia)
            {
                tipoEnergia = _tipoEnergia;
                visual.UpdateColorEnergia(tipoEnergia);
                if(Ev_OnTipoEnergiaCambia != null)
                    Ev_OnTipoEnergiaCambia.Invoke(tipoEnergia);
            }
        #endregion

        public void PickUpBateria()
        {
            AumentarEnergia(30);
        }

        #region VIDA / MUERTE
            public void Matar()
            {
                if(estado != EstadoJugador.MUERTO)
                {
                    AudioManager.instance.PlayOneShot(misSonidos.muerte_SFX);
                    ApagarTurbo();
                    estado = EstadoJugador.MUERTO;
                    ToogleControlador(false);
                    if(Ev_Muere != null) 
                        Ev_Muere.Invoke();
                    LlamarRespawn();
                }
            }

            public void Spawn()
            {
                Spawn(SpawnPoint);
            }
            
            public void Spawn(Vector3 _spawnPos)
            {
                Debug.Log("REVIVE");
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
                Debug.Log("reviviendo en " + tiempoRespawn);
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