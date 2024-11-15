using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SistemaVotacion;

namespace Demokratos.UI{
    public class UI_Controller : MonoBehaviour
    {
        public CameraFollower seguidorCamara;
        [Space]
        [SerializeField] UI_Tutorial Tutorial_Pantalla;
        [SerializeField] UI_GameplayScreen Gameplay_Pantalla;
        [SerializeField] UI_ScreenVictoria Victoria_Pantalla;
        [SerializeField] UI_Debug Debug_pantalla;
        [Space]
        [SerializeField] UI_VotosVisual Votacion_pantalla;
        [SerializeField] UI_BarraEnergia barraEnergia;
        [SerializeField] UI_NivelTexto texto_nivel;
        [SerializeField] UI_TimerVotos timer_votos;
        public UI_RefeColores paletaColores;



        void Start()
        {
            Debug_pantalla.Cerrar();
        }

        public void Mostrar_Tutorial()
        {
            Gameplay_Pantalla.Mostrar(false);
            Tutorial_Pantalla.Mostrar(true);
        }

        public void Mostrar_Gameplay(){
            Gameplay_Pantalla.Mostrar(true);
            Victoria_Pantalla.EsconderPantalla();
            Tutorial_Pantalla.Mostrar(false);
        }

        public void Mostrar_Victoria()
        {
            Tutorial_Pantalla.Mostrar(false);
            Gameplay_Pantalla.Mostrar(false);
            Victoria_Pantalla.AbrirPantalla();
        }
    }
}