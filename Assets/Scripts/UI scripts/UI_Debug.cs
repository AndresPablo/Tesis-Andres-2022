using UnityEngine.UI;
using UnityEngine;

namespace SistemaVotacion{
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