using UnityEngine;

namespace Viejo{
public class Bandera : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D otro)
    {
        if(otro.CompareTag("Player"))
        {
            GameManager.singleton.PasarNivel();
        }
    }
    
}
}