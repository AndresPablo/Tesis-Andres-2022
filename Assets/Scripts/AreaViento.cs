using System.Collections;
using System.Collections.Generic;
using TarodevController;
using UnityEngine;

public enum Direccion { IZQUIERDA, DERECHA, ARRIBA, ABAJO }
namespace Demokratos
{
    
    public class AreaViento : MonoBehaviour
    {
        public Vector2 vectorViento;

        private void Start() {
        }

        void OnTriggerStay2D(Collider2D other)
        {
            if(other.tag == "Player")
            {
                // TODO: revisar, empuja lentamente
                    //other.GetComponent<Rigidbody2D>().AddForce(vectorViento,ForceMode2D.Force);
                // VIEJO: es para que haya viento en todo el mundo
                    //other.GetComponent<PlayerController>().SetExternalForce(Game_Manager_Nuevo.singleton.fuerzaVientoGlobal) ;
                other.GetComponent<PlayerController>().SetExternalForce(vectorViento) ;
            }
        }

        void OnTriggerExit2D(Collider2D other)
        {
            // TODO: ver si puedo hacer que la fuerza externa se desvanezca en lugar de ir a Cero
            other.GetComponent<PlayerController>().SetExternalForce(new Vector2(0,0)) ;
        }

        
    }

}