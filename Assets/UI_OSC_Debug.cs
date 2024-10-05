using SistemaVotacion;
using UnityEngine;
using TMPro;

namespace SistemaVotacion{
    public class UI_OSC_Debug : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI label;

        void Start()
        {
            UnityOSCReceiver.Ev_MensajeOSCRecibido+=MostrarMensajesOSC;
        }


        void MostrarMensajesOSC(OSC.NET.OSCMessage message)
        {
            label.text = message.Values.ToString();
        }
    }
}