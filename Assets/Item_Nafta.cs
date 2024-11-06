using UnityEngine;
using UnityEngine.Events;

namespace Demokratos{
public class Item_Nafta : MonoBehaviour
{
    [SerializeField] float cantidadEnergia = 1f;
    [SerializeField] GameObject pickupParticulas;
    [SerializeField] AudioClip pickUp_SFX;

    #region EVENTOS 
        public delegate void GasDelegate();
        public delegate void VoidFloatDelegate(float _energia);
        public static event GasDelegate Ev_NaftaPickup;
        public UnityEvent onScenarioPickupEvent; // Evento específico para el tutorial
    #endregion

    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D otro)
        {
            if(otro.CompareTag("Player"))
            {
                if(Ev_NaftaPickup != null)
                    Ev_NaftaPickup.Invoke();
                
                onScenarioPickupEvent?.Invoke();
                
                // Genera particulas cuando el jugador lo junta
                if(pickupParticulas)
                {
                    GameObject go = Instantiate(pickupParticulas);
                    go.transform.position = this.transform.position;
                }

                // efecto de sonido
                AudioManager.instance.PlayOneShot(pickUp_SFX);
        
                // Destruye el objeto
                Destroy(gameObject);
            }
        }
}
}
