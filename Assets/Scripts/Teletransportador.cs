using UnityEngine;
using SistemaVotacion;

namespace Demokratos{
public class Teletransportador : MonoBehaviour
{
    public bool congelarCamara;
    public bool activarVotos;
    public bool activarEnergia;
    public Transform destino;


    void OnTriggerEnter2D(Collider2D otro){
        if(otro.CompareTag("Player")){
            HacerCosas();
        }
    }

    void HacerCosas(){
        if(destino){
            Game_Manager_Nuevo.singleton.Jugador.Teleport(destino.position);
            Game_Manager_Nuevo.singleton.Interfaz.seguidorCamara.MoverHastaJugador();
        }

        if(congelarCamara){
//            Game_Manager_Nuevo.singleton.Interfaz.seguidorCamara.SeguirObjetivo(destino);
            Game_Manager_Nuevo.singleton.Interfaz.seguidorCamara.Quieta();
            Game_Manager_Nuevo.singleton.Interfaz.seguidorCamara.MoverHastaJugador();

        }
        else
        {
            Game_Manager_Nuevo.singleton.Interfaz.seguidorCamara.Seguir();
        }

        if(activarVotos){
            VotingLogic.singleton.ToogleVotacion(true);
        }

        if(activarEnergia){

        }
    }
}
}