using UnityEngine;
using Viejo;

namespace Demokratos{
    public class IniciadorNivel : MonoBehaviour
    {
        public bool congelarCamara;
        public int energiaInicial;

        void Start()
        {
            Invoke("AplicarInicioDeNivel", 0.1f);               
        }
        
        void AplicarInicioDeNivel(){
                if(congelarCamara)
                    Game_Manager_Nuevo.singleton.Interfaz.seguidorCamara.Quieta();
                else
                    Game_Manager_Nuevo.singleton.Interfaz.seguidorCamara.Seguir();
                
                Game_Manager_Nuevo.singleton.Jugador.SetEnergiaMaximaDisponible (energiaInicial);
        }
    }
}
