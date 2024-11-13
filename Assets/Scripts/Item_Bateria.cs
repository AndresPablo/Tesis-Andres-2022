using UnityEngine;
using UnityEngine.Events;

namespace Demokratos{
public class Item_Bateria : MonoBehaviour
{
    [SerializeField] float cantidadEnergia = 1f;
    [SerializeField] GameObject pickupParticulas;
    [SerializeField] AudioClip pickUp_SFX;


    #region EVENTOS de BATERIA
        public delegate void BateriaDelegate();
        public delegate void VoidFloatDelegate(float _energia);
        public static event BateriaDelegate Ev_BateriaPickup;
        public UnityEvent onScenarioPickupEvent; // Evento específico para el tutorial
    #endregion

    private void OnTriggerEnter2D(Collider2D otro)
    {
        if(otro.CompareTag("Player"))
        {
            if(Ev_BateriaPickup != null)
                Ev_BateriaPickup.Invoke();
            
            onScenarioPickupEvent?.Invoke();
            
            // Genera particulas cuando el jugador lo junta
            if(pickupParticulas)
            {
                GameObject go = Instantiate(pickupParticulas);
                go.transform.position = this.transform.position;
            }

                // Muestra un texto arriba del jugador
                UI_NotificacionFlotante.singleton.CrearEnJugador("Bateria maxima +1");

            // efecto de sonido
            AudioManager.instance.PlayOneShot(pickUp_SFX);
    
            // Destruye el objeto
            Destroy(gameObject);
        }
    }
}
}