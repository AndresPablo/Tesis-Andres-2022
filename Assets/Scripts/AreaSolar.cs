using Demokratos;
using UnityEngine;

namespace Demokratos{
    public class AreaSolar : MonoBehaviour
    {
        [SerializeField] float escalaArea = 2;
        [SerializeField] float incrementoEnergia = 1f;

        private void OnTriggerStay2D(Collider2D other) {
            if(other.CompareTag("Player"))
            {
                other.GetComponent<JugadorLogica>().
                AumentarEnergia(incrementoEnergia * Time.deltaTime, TipoEnergia.SOLAR);
            }
        }

        private void Start() {
            transform.localScale *= escalaArea;
        }
    }
}
