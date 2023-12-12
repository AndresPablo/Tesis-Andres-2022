using SistemaVotacion;
using UnityEngine;
using TMPro;

namespace SistemaVotacion{
    public class UI_VotosDebug : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI label;

        void Start()
        {
            VotingCounter.Ev_CambiarVotos+=MostrarCantidades;
        }


        void MostrarCantidades(Acta[] actas)
        {
            label.text = actas[0].votos + " + " + actas[1].votos;
        }
    }
}