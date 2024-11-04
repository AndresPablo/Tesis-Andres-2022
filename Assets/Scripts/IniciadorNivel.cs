using UnityEngine;

namespace Demokratos{
    public class IniciadorNivel : MonoBehaviour
    {
        public bool congelarCamara;

        void Start()
        {
            Invoke("AplicarInicioDeNivel", 0.1f);               
        }
        
        void AplicarInicioDeNivel(){
                if(congelarCamara)
                    Game_Manager_Nuevo.singleton.Interfaz.seguidorCamara.Quieta();
                else
                    Game_Manager_Nuevo.singleton.Interfaz.seguidorCamara.Seguir();

        }
    }
}
