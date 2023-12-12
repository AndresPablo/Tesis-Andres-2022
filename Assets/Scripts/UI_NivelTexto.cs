using UnityEngine;
using TMPro;
using Viejo;

namespace Demokratos {
    public class UI_NivelTexto : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI label;
        int nivel;

        private void OnEnable() {
            label = GetComponent<TextMeshProUGUI>();
            Game_Manager_Nuevo.Ev_CuentaBaterias += ActualizarTexto;
            Game_Manager_Nuevo.Ev_PasoNivel += ActualizarNivelActual;
        }

        void ActualizarNivelActual(int numero)
        {
            nivel = numero;
        }

        void ActualizarTexto(int _bateriasRestantes, int _bateriasTotales)
        {
            int bateriasRestantes = _bateriasRestantes;
            label.text = "<b>Nivel " + nivel + "</b> - Faltan <b>" + bateriasRestantes + "</b> baterías.";
        }
    }
}