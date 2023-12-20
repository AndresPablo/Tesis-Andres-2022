using System.Collections;
using Demokratos;
using UnityEngine;
using Viejo;

namespace Demokratos.Energia{
    public class AreaSolar : MonoBehaviour
    {
        [SerializeField] float radioArea = 3f;
        [SerializeField] float incrementoEnergia = 1f;
        JugadorLogica Jugador;

        private void OnTriggerStay2D(Collider2D other) {
            if(other.CompareTag("Player"))
            {

            }
        }

        private void Start() {
            transform.localScale *= radioArea;
            Jugador = Game_Manager_Nuevo.singleton.Jugador;
            StartCoroutine(RutinaDistanciaJugador());
        }

        IEnumerator RutinaDistanciaJugador()
        {
            // Suma un margen al radio de luz para que el jugador no tenga que estar complemente dentro del circul ode luz
            float margen = .05f;
            // Evalua si el jugador esta dentro de la luz
            if(Game_Manager_Nuevo.singleton.DistanciaDelJugador(transform.position) < radioArea + 0.5f)
                DarEnergiaJugador();
            // Mide la distancia cada un tiempo discrecional para optimizar CPU
            yield return new WaitForSeconds(.05f);
            // Vuelve a llamar la rutina
            StartCoroutine(RutinaDistanciaJugador());
        }

        void DarEnergiaJugador()
        {
            Jugador.AumentarEnergia(incrementoEnergia * Time.deltaTime, TipoEnergia.SOLAR);
        }
    }
}
