/* Copyright (c) 2022 dr. ext (Vladimir Sigalkin) */

using UnityEngine;

namespace extOSC.Examples
{
	public class SimpleMessageReceiver : MonoBehaviour
	{
		public string Address = "/example/1";

		[Header("OSC Settings")]
		public OSCReceiver Receiver;


		protected virtual void Start()
		{
			Receiver.Bind(Address, ReceivedMessage);
		}

		private void ReceivedMessage(OSCMessage message)
		{
			Debug.LogFormat("Received: {0}", message);
			if(message.ToInt(out var value))
            {
				if(value > -1)
                {

                }
            }
		}

	}
}