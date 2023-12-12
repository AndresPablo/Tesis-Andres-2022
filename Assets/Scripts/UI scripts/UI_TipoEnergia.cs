using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Demokratos.UI {

    public class UI_TipoEnergia : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI label;

        private void OnEnable() {
            JugadorLogica.Ev_OnTipoEnergiaCambia += MostrarCambioEnergia;  
        }

        void MostrarCambioEnergia(TipoEnergia tipo)
        {
            label.text = tipo.ToString();
        }
    }

}