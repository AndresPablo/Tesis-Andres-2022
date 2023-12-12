using Demokratos;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Demokratos.UI;

namespace SistemaVotacion {
    public class UI_VotosVisual : MonoBehaviour
    {
        [SerializeField] UI_Acta acta_A;
        [SerializeField] UI_Acta acta_B;
        [SerializeField] TextMeshProUGUI labelResultado;
        [SerializeField] float tiempoMuestraResultado = 3f;
        [Space]
        [SerializeField] Image barra_A;
        [SerializeField] Image barra_B;


        void Start()
        {
            VotingCounter.Ev_CambiarVotos += MostrarVotosActuales;
            VotingLogic.Ev_Cierra += ApagarActas;
            VotingLogic.Ev_Abre += MostrarActas;
            VotingLogic.Ev_AplicarResultado += MostrarResutado;
            EsconderTextoResultado();
        }

        void MostrarResutado(Acta _actaGanadora)
        {
            labelResultado.text = "Gana la opcion: " + _actaGanadora.name;
            labelResultado.transform.parent.gameObject.SetActive(true);
            EsconderActas(true);
            Invoke("EsconderTextoResultado", 3f);
        }

        void EsconderTextoResultado()
        {
            labelResultado.text = "";
            labelResultado.transform.parent.gameObject.SetActive(false);
        }

        void ApagarActas()
        {
            acta_A.ApagarVisuales();
            acta_B.ApagarVisuales();
        }

        public void EsconderActas(bool animar = true)
        {
            acta_A.Esconder(animar);
            acta_B.Esconder(animar);
        }

        public void MostrarActas()
        {
            acta_A.Mostrar();
            acta_B.Mostrar();
        }

        public void MostrarVotosActuales(SistemaVotacion.Acta[] actas)
        {
            acta_A.CargarInfo(actas[0]);
            acta_B.CargarInfo(actas[1]);
        }
    }
}