using System.Collections;
using System.Collections.Generic;
using Demokratos.Energia;
using TarodevController;
using UnityEngine;

public enum Direccion { IZQUIERDA, DERECHA, ARRIBA, ABAJO }
namespace Demokratos
{
    
    public class AreaViento : MonoBehaviour
    {
        [SerializeField] Vector2 vectorViento;
        [SerializeField] TipoEmpuje tipoEmpuje;
        [SerializeField] [Range(.5f, 5f)]float recarga = 1f;
        JugadorLogica JugadorLogica;

        private void Start() {
            JugadorLogica = Game_Manager_Nuevo.singleton.Jugador;
        }

        void OnTriggerStay2D(Collider2D other)
        {
            if(other.tag == "Player")
            {
                // TODO: revisar, empuja lentamente
                    //other.GetComponent<Rigidbody2D>().AddForce(vectorViento,ForceMode2D.Force);
                // VIEJO: es para que haya viento en todo el mundo
                    //other.GetComponent<PlayerController>().SetExternalForce(Game_Manager_Nuevo.singleton.fuerzaVientoGlobal) ;
                if(tipoEmpuje == TipoEmpuje.ADITIVA)
                    other.GetComponent<PlayerController>().AddExternalForce(vectorViento) ;
                else
                if(tipoEmpuje == TipoEmpuje.FIJA)
                    other.GetComponent<PlayerController>().SetExternalForce(vectorViento) ;

                // Recarga la energia
                JugadorLogica.AumentarEnergia(recarga * Time.deltaTime, TipoEnergia.EOLICA);
            }
        }

        void OnTriggerExit2D(Collider2D other)
        {
            // TODO: ver si puedo hacer que la fuerza externa se desvanezca en lugar de ir a Cero
            other.GetComponent<PlayerController>().SetExternalForce(new Vector2(0,0)) ;
        }

        public void CargarValores(Vector2 _vectorViento, float _recarga, TipoEmpuje _tipoEmpuje)
        {
            vectorViento = _vectorViento;
            recarga = _recarga;
            tipoEmpuje = _tipoEmpuje;
        }
        
    }

}