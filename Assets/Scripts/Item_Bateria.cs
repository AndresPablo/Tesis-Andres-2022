using UnityEngine;

namespace Demokratos{
public class Item_Bateria : MonoBehaviour
{
    [SerializeField] float cantidadEnergia = 50f;
    [SerializeField] GameObject pickupParticulas;
    [SerializeField] AudioClip pickUp_SFX;


    #region EVENTOS de BATERIA
        public delegate void BateriaDelegate();
        public delegate void VoidFloatDelegate(float _energia);
        public static event BateriaDelegate Ev_BateriaPickup;
        public static event VoidFloatDelegate Ev_Pickup;
    #endregion

    private void OnTriggerEnter2D(Collider2D otro)
    {
        if(otro.CompareTag("Player"))
        {
            // Aumenta el nivel de energia del jugador
            if(Ev_Pickup != null)
                Ev_Pickup.Invoke(cantidadEnergia);
            // logica de paso de nivel
            if(Ev_BateriaPickup != null)
                Ev_BateriaPickup.Invoke();
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