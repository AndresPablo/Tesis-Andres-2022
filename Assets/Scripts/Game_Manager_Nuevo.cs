using System.Collections;
using System.Collections.Generic;
using Demokratos.UI;
using SistemaVotacion;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

namespace Demokratos{
    public class Game_Manager_Nuevo : MonoBehaviour
    {
        #region SINGLETON
        public static Game_Manager_Nuevo singleton;
        private void Awake()
        {
            singleton = this;
        }
        #endregion
        
        // Referencias
        [SerializeField] JugadorLogica jugador;
        [SerializeField] LevelManager Nivel_Handler;
        public UI_Controller Interfaz;
        public UnityEvent OnEndLevel;
        public UnityEvent OnVictory;
        public UnityEvent OnRestart;
        public Vector2 fuerzaVientoGlobal;
        [SerializeField] float tiempoRespawn = 3f;
        [SerializeField] float tiempoEntreNiveles = 2f;


        public JugadorLogica Jugador { get { return jugador; }}
        public Vector2 JugadorPos { get { return jugador.transform.position; }}
        public int bateriasEnNivel;
        public int bateriasEnNivel_Max;

        public int muertes;
        public float tiempo;



        #region EVENTOS
            public delegate void VoidDelegate();
            public delegate void BateriasDelegate(int actuales, int totales);
            public delegate void NivelDelegate(int nivel_actual);
            public static event BateriasDelegate Ev_CuentaBaterias;
            public static event NivelDelegate Ev_PasoNivel;
            public static event VoidDelegate Ev_InvertirGravedad;
        #endregion

        void Start()
        {
            Item_Bateria.Ev_BateriaPickup += ChequearPasoNivel;
            VotingLogic.Ev_AplicarResultado += AplicarResultadoVotacion;
            JuegoIniciadoPrimeraVez();
        }

        void Update(){
            // sumamos los segundos en el Update
            tiempo += Time.deltaTime;
        }


        public void EmpezarPartida()
        {
            Nivel_Handler.LoadLevel(0);
            tiempo = 0;
            muertes = 0;
            Jugador.gameObject.SetActive(true);
            Jugador.SetSpawn(LevelManager.spawnPos);
            Jugador.Spawn(LevelManager.spawnPos) ;
            Jugador.ResetearEnergia();
            Jugador.SetearTipoEnergia(Jugador.tipoEnergia);
            bateriasEnNivel_Max = Nivel_Handler.GetCantidadBaterias();
            ChequearPasoNivel();
            Interfaz.Mostrar_Gameplay();
        }

        public void Reiniciar(){
            // mostramos el tutorial y despues desde ahi se llama a EmpezarPartida()
            Jugador.gameObject.SetActive(false);
            Interfaz.Mostrar_Tutorial();
        }

        public void EmpezarNuevoNivel()
        {
            VotingLogic.singleton.ReinicarVotacion();
            Nivel_Handler.LoadNextLevel();
            Jugador.SetSpawn(LevelManager.spawnPos);
            Jugador.Spawn(LevelManager.spawnPos) ;
            Jugador.ResetearEnergia();
            bateriasEnNivel_Max = Nivel_Handler.GetCantidadBaterias();
            if (Ev_PasoNivel != null)
                Ev_PasoNivel(LevelManager.currentLevel);
        }

        public void NivelCompletado()
        {
            Invoke("EmpezarNuevoNivel", tiempoEntreNiveles);
        }

        void ChequearPasoNivel()
        {
            // tiene -1 porque la bateria no se destruye aun cuando invoca esta funcion a traves del evento
            bateriasEnNivel = Nivel_Handler.GetCantidadBaterias() - 1;
            if(Ev_CuentaBaterias != null)
                Ev_CuentaBaterias(bateriasEnNivel, bateriasEnNivel_Max);
            if(bateriasEnNivel  <= 0)
            {
                NivelCompletado();
            }
            //Debug.Log(bateriasEnNivel.ToString() + "/" + bateriasEnNivel_Max.ToString());
        }

        public void Victoria()
        {
            if (OnVictory != null)
                OnVictory.Invoke();
            //SceneManager.LoadScene("Escena Victoria");
            Jugador.gameObject.SetActive(false);
            Interfaz.Mostrar_Victoria();
        }

        void JuegoIniciadoPrimeraVez(){
            Jugador.gameObject.SetActive(false);
            Interfaz.Mostrar_Tutorial();
        }

        #region VOTACIONES
        void AplicarResultadoVotacion(SistemaVotacion.Acta acta, int index_ganadora)
        {
            Jugador.SetearTipoEnergia(TipoEnergia.NINGUNO);
            switch(acta.efecto)
            {
                case TipoEfecto.EOLICO:
                    Jugador.SetearTipoEnergia(TipoEnergia.EOLICA);
                    break;
                case TipoEfecto.SOLAR:
                    Jugador.SetearTipoEnergia(TipoEnergia.SOLAR);
                    break;
                case TipoEfecto.TERMICO:
                    Jugador.SetearTipoEnergia(TipoEnergia.TERMO);
                    break;
                case TipoEfecto.FOSIL:
                    Jugador.SetearTipoEnergia(TipoEnergia.FOSIL);
                    break;                
                case TipoEfecto.NO_ENERGIA:
                    Jugador.SetearTipoEnergia(TipoEnergia.NINGUNO);
                    break;
                case TipoEfecto.GRAV:
                    // TODO:
                    break;
                case TipoEfecto.SWAP:
                    // TODO:
                    break;
                default:
                    // TODO:
                    break;
            }
        }

        
        #endregion

        #region FUNCIONES UTILITARIAS

        public float DistanciaDelJugador(Vector2 origen)
        {
            return Vector2.Distance(origen, JugadorPos);
        }
        
        public void InvertirGravedad()
        {
            // TODO
        }

        public void AgregarMuerte(){
            muertes++;
        }

        #endregion
    }
}