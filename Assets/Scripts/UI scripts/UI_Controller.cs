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
        [SerializeField] UI_BarraEnergia barraEnergia;
        [SerializeField] UI_NivelTexto texto_nivel;
        [SerializeField] UI_TimerVotos timer_votos;
        public UI_RefeColores paletaColores;



        void Start()
        {
            Debug_pantalla.Cerrar();
        }



        void Update()
        {
            
        }
    }
}