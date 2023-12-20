using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Demokratos.Energia{
    [RequireComponent(typeof(Collider2D))]
    public class AreaFuego : MonoBehaviour
    {
        [SerializeField] TipoEnergia mi_tipoEnergia = TipoEnergia.TERMO;
        Collider2D collider;
        JugadorLogica jugadorLogica;
        public float recarga;


        private void Start() {
            jugadorLogica = Game_Manager_Nuevo.singleton.Jugador;
            collider = GetComponent<Collider2D>();
        }

        private void OnTriggerStay2D(Collider2D other) {
            if(other.tag == "Player")
            {
                // Recarga la energia si el tipo coincide
                if(jugadorLogica.tipoEnergia == mi_tipoEnergia)
                {
                    jugadorLogica.AumentarEnergia(recarga * Time.deltaTime, mi_tipoEnergia);
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if(jugadorLogica.tipoEnergia != mi_tipoEnergia)
            {
                jugadorLogica.Matar();
            } 
        }

        public void SetEstadoCollider(bool estado)
        {
            collider.enabled = estado;
        }
    }
}