using UnityEngine;
using TMPro;

namespace Demokratos {
    public class UI_NivelTexto : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI label;
        int nivel;

        private void OnEnable() {
            label = GetComponent<TextMeshProUGUI>();
            Game_Manager_Nuevo.Ev_PasoNivel += ActualizarNivelActual;
        }

        void ActualizarNivelActual(int numero)
        {
            nivel = numero;
            ActualizarTexto();
        }

        void ActualizarTexto()
        {
            label.text = "Nivel " + nivel;
        }
    }
}