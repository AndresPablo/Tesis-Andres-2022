using UnityEngine;
using TMPro;

namespace Demokratos {
    public class UI_BateriasRestantesTexto : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI label;

        private void OnEnable() {
            label = GetComponent<TextMeshProUGUI>();
            Game_Manager_Nuevo.Ev_CuentaBaterias += ActualizarTexto;
        }


        void ActualizarTexto(int _bateriasRestantes, int _bateriasTotales)
        {
            //label.text =  _bateriasRestantes + " / " + _bateriasTotales;
            label.text =  Game_Manager_Nuevo.singleton.Jugador.energiaMaxDisponible + " / " + _bateriasTotales + 1;
        }
    }
}