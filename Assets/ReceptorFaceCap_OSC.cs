using UnityEngine;

namespace extOSC
{
	public class ReceptorFaceCap_OSC : MonoBehaviour
	{
		public string Address_Izquierda = "/voto/izq";
		public string Address_Derecha = "/voto/der";

		[Header("OSC Settings")]
		public OSCReceiver Receiver;
		[Space]
		[SerializeField] VoteControl votador;

		public int votos_I = 0;
		public int votos_D = 0;

		protected virtual void Start()
		{
			Receiver.Bind(Address_Izquierda, RecibirIzquierda);
			Receiver.Bind(Address_Derecha, RecibirDerecha);
		}

		private void RecibirIzquierda(OSCMessage message)
		{
			if (message.ToInt(out var value))
			{
				if (value > -1)
				{
					votos_I = value;
				}
			}
		}

		private void RecibirDerecha(OSCMessage message)
		{
			if (message.ToInt(out var value))
			{
				if (value > -1)
				{
					votos_D = value;
				}
			}
		}

		private void Update()
		{
			// Aca mandamos la cantidades de caras que recibimos y guardamos desde OpenCV
			votador.OverrideVotos(votos_I, votos_D);
        }

    }
}