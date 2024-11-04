using System.Collections;
using UnityEngine;
using extOSC;
using Demokratos;
using SistemaVotacion;

public class OSC_ReceptorVotos : MonoBehaviour
{
    public string oscAddress = "/";
    public int oscPuerto = 7001;
    public bool camaraEspejadaHorizontal;
    private OSCReceiver _oscReceiver;


    #region EVENTOS
        public delegate void DelegadoMensajeOSC_Int(int valor);
        public static event DelegadoMensajeOSC_Int Ev_MensajeOSCRecibidoInt;
    #endregion


    VotingCounter contadorVotos;

    void Start()
    {
        _oscReceiver = gameObject.AddComponent<OSCReceiver>();
        _oscReceiver.LocalPort = oscPuerto;  // Ensure this matches your Python script
        _oscReceiver.Bind(oscAddress, OnReceiveMessage);
        contadorVotos = GetComponent<VotingCounter>();
    }

    private void OnReceiveMessage(OSCMessage message)
    {
        // Assuming two integers (izq, der) are sent from the Python script
        var izq = message.Values[0].IntValue;
        var der = message.Values[1].IntValue;

        // invierte los valores si la camara esta espejada horizontalmente
        if(camaraEspejadaHorizontal){
            int izq_viejo = izq;
            int der_viejo = der;
            der = izq_viejo;
            izq = der_viejo;
        }

        // Envia la data al contador de votos VotingCOunter.cs
        if(contadorVotos != null){
            contadorVotos.CambiarVotos(0, izq);
            contadorVotos.CambiarVotos(1, der);
        }

        if(izq != 0 || der != 0)
        {
            Debug.Log("Votos:   " + izq + " - " + der);
        }
    }

    public void Espejar_Camara_Horizontalmente(){
        Debug.Log("CAMARA INVERTIDA");
        camaraEspejadaHorizontal = !camaraEspejadaHorizontal;
    }
}
