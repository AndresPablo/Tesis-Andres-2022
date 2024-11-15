using UnityEngine.UI;
using UnityEngine;
using SistemaVotacion;
using Demokratos;

namespace Demokratos.UI {
    public class UI_Debug : MonoBehaviour
    {
        [SerializeField] RectTransform panel;
        [SerializeField] UI_VotosDebug votosDebug;

        public void Cerrar()
        {
            panel.gameObject.SetActive(false);
        }

        public void Abrir()
        {
            panel.gameObject.SetActive(true);
        }
    }
}