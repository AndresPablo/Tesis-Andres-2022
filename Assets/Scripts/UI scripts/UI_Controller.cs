using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SistemaVotacion;
using Viejo;

namespace Demokratos.UI{
    public class UI_Controller : MonoBehaviour
    {
        public CameraFollower seguidorCamara;
        [SerializeField] UI_Debug Debug_pantalla;
        [SerializeField] RectTransform Menu_pantalla;
        [SerializeField] RectTransform Gameplay_pantalla;
        [SerializeField] UI_VotosVisual Votacion_pantalla;
        [SerializeField] RectTransform Victoria_pantalla;
        [SerializeField] UI_Tutorial Tutorial_Rect;
        [SerializeField] UI_BarraEnergia barraEnergia;
        [SerializeField] UI_NivelTexto texto_nivel;
        [SerializeField] UI_ScreenVictoria pantalla_victoria;
        [SerializeField] UI_TimerVotos timer_votos;
        public UI_RefeColores paletaColores;



        void Start()
        {
            Debug_pantalla.Cerrar();
        }



        void Update()
        {
            
        }

        public void Mostrar_Tutorial()
        {
            Gameplay_pantalla.gameObject.SetActive(false);
            Tutorial_Rect.gameObject.SetActive(true);
        }

        public void Mostrar_Gameplay(){
            Gameplay_pantalla.gameObject.SetActive(true);
            Tutorial_Rect.gameObject.SetActive(false);
            
        }

        public void Mostrar_Victoria()
        {
            Gameplay_pantalla.gameObject.SetActive(false);
            Tutorial_Rect.gameObject.SetActive(false);
            pantalla_victoria.AbrirPantalla();
        }
    }
}