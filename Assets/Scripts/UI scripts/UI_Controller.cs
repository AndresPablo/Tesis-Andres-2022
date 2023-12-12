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
        [SerializeField] UI_Debug Debug_pantalla;
        [SerializeField] RectTransform Menu_pantalla;
        [SerializeField] RectTransform Gameplay_pantalla;
        [SerializeField] UI_VotosVisual Votacion_pantalla;
        [SerializeField] RectTransform Victoria_pantalla;
        [SerializeField] UI_BarraEnergia barraEnergia ;
        [Space]
        [SerializeField] Sprite iconoFosil;
        [SerializeField] Sprite iconoEolica;
        [SerializeField] Sprite iconoHidro;
        [SerializeField] Sprite iconoTermica;
        [SerializeField] Sprite iconoSolar;
        [Space]
        public UI_RefeColores paletaColores;



        // Start is called before the first frame update
        void Start()
        {
            Debug_pantalla.Cerrar();
        }



        // Update is called once per frame
        void Update()
        {
            
        }
    }
}