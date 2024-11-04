
using Cinemachine;
using UnityEngine;
using Viejo;

namespace Demokratos{
public class CameraFollower : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] CinemachineVirtualCamera camaraVirtual;

    public bool follow = true;

    private void Start()
    {
        //Jugador.Ev_Spawnea += Seguir;
        //Jugador.Ev_Muere += Quieta;
    }

    void Update()
    {
        if(follow)
        {
            if(target)
            {
                Vector3 newPos = new Vector3(target.position.x, target.position.y, transform.position.z);
                transform.position = newPos;
            }
        }
    }

    void Toogle()
    {
        follow = !follow;
        if(follow) Seguir();
        else
        Quieta();
    }

    public void Seguir()
    {
        camaraVirtual.Follow = Game_Manager_Nuevo.singleton.Jugador.transform;
    }

    public void Quieta()
    {
        camaraVirtual.Follow = null;
    }

    public void MoverHastaJugador(){
        camaraVirtual.Follow = Game_Manager_Nuevo.singleton.Jugador.transform;
        camaraVirtual.Follow = null;
    }
}
}