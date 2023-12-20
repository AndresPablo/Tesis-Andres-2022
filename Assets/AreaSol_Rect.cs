using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demokratos.Energia{
    public class AreaSol_Rect : MonoBehaviour
    {
        [SerializeField] float incrementoEnergia = 1f;
        JugadorLogica Jugador;


        void Start()
        {
            Jugador = Game_Manager_Nuevo.singleton.Jugador;
        }

        private void OnTriggerStay2D(Collider2D other) {
            if(other.CompareTag("Player"))
            {
                Jugador.AumentarEnergia(incrementoEnergia * Time.deltaTime, TipoEnergia.SOLAR);
            }
        }
    }
}
