
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] Transform target;
    public bool follow = true;

    private void Start()
    {
        Jugador.Ev_Spawnea += Seguir;
        Jugador.Ev_Muere += Quieta;
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
    }

    void Seguir()
    {
        follow = true;
    }

    void Quieta()
    {
        follow = false;

    }
}
