using System.Collections;
using System.Collections.Generic;
using Demokratos.UI;
using SistemaVotacion;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

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
        [SerializeField] LevelManager Niveles;
        public UI_Controller Interfaz;
        public UnityEvent OnEndLevel;
        public UnityEvent OnVictory;
        public UnityEvent OnRestart;
        public Vector2 fuerzaVientoGlobal;

        public JugadorLogica Jugador { get { return jugador; }}
        public int bateriasEnNivel;
        public int bateriasEnNivel_Max;

        #region EVENTOS de BATERIA
            public delegate void BateriasDelegate(int actuales, int totales);
            public delegate void NivelDelegate(int nivel_actual);
            public static event BateriasDelegate Ev_CuentaBaterias;
            public static event NivelDelegate Ev_PasoNivel;
        #endregion

        void Start()
        {
            Item_Bateria.Ev_BateriaPickup += ChequearPasoNivel;
            VotingLogic.Ev_AplicarResultado += AplicarResultadoVotacion;
            Jugador.gameObject.SetActive(false);
            // TODO: borrar porque es para PROBAR
            EmpezarPartida();
        }

        void AplicarResultadoVotacion(SistemaVotacion.Acta acta)
        {
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
            }
        }

        void EmpezarPartida()
        {
            Niveles.LoadLevel(0);
            Jugador.gameObject.SetActive(true);
            Jugador.Spawn(Niveles.spawnPos) ;
            Jugador.ResetearEnergia();
        }

        public void NivelCompletado()
        {
            Niveles.LoadNextLevel();
            Jugador.Spawn(Niveles.spawnPos) ;
            Jugador.ResetearEnergia();
            Jugador.SetearTipoEnergia(TipoEnergia.FOSIL); // TODO: dejar el estado del nivel anterior?
            bateriasEnNivel_Max = Niveles.GetCantidadBaterias();
        }

        void Update()
        {
            
        }

        void ChequearPasoNivel()
        {
            int baterias = Niveles.GetCantidadBaterias();
            if(Ev_CuentaBaterias != null)
                Ev_CuentaBaterias(baterias, bateriasEnNivel_Max);
            if(baterias  <= 0)
            {
                NivelCompletado();
            }
        }
    }
}