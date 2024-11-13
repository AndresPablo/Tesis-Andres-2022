using Demokratos;
using TMPro;
using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using Demokratos.UI;
using SistemaVotacion;

namespace SistemaVotacion {
    public class UI_VotosVisual : MonoBehaviour
    {
        [SerializeField] UI_Acta acta_A;
        [SerializeField] UI_Acta acta_B;
        [SerializeField] UI_TimerVotos timerVotos;
        [SerializeField] TextMeshProUGUI labelResultado;
        [SerializeField] float tiempoMuestraResultado = 2f;
        [Space]
        [SerializeField] Animator animator;
        
        [Header("Personas con iconos")]
        [SerializeField] GameObject personas_GO;
        [SerializeField] GameObject personas_GO_izquierda;
        [SerializeField] GameObject personas_GO_derecha;
        
        [Header("Barras")]
        [SerializeField] GameObject barras_GO;
        [SerializeField] Image barra_A;
        [SerializeField] Image barra_B;
        float target_barra_A;
        float target_barra_B;


        void Start()
        {
            VotingCounter.Ev_CambiarVotos += MostrarVotosActuales;
            VotingLogic.Ev_Cierra += ApagarActas;
            VotingLogic.Ev_Abre += MostrarActas;
            VotingLogic.Ev_AplicarResultado += MostrarResutado;
            EsconderTextoResultado();
            barras_GO.SetActive(false);
            personas_GO.SetActive(false);
        }

        void MostrarResutado(Acta _actaGanadora, int index_ganadora)
        {
            //labelResultado.text = "Gana la opcion: " + _actaGanadora.name;
            //labelResultado.transform.parent.gameObject.SetActive(true);
            EsconderBarras();
            Invoke("EsconderTextoResultado", 3f);
            //animar
            if (index_ganadora == 0)
                animator.Play("gana_A");
            else
            if (index_ganadora == 1)
                animator.Play("gana_B");
        }

        void EsconderTextoResultado()
        {
            labelResultado.text = "";
            labelResultado.transform.parent.gameObject.SetActive(false);
        }

        IEnumerator AbrirTimer(float _segundosDeTimer, float _esperaInvoke){
            yield return new WaitForSeconds(_esperaInvoke);
            timerVotos.ToggleTimerUI(true);
            timerVotos.StartTimer(_segundosDeTimer);
        }

        void ApagarActas(float _tiempoEntreElecciones)
        {
            barras_GO.SetActive(false);
            personas_GO.SetActive(false);
            // mostrar el timer en X segundos
            float _tiempoParaTimer = 3;
            StartCoroutine(AbrirTimer(_tiempoParaTimer, _tiempoEntreElecciones - _tiempoParaTimer));
        }

        public void EsconderBarras()
        {
            barras_GO.SetActive(false);
            personas_GO.SetActive(false);
        }

        public void MostrarActas()
        {
            animator.Play("Entra abajo");
            barra_A.color = acta_A.mi_color;
            barra_B.color = acta_B.mi_color;
            barras_GO.SetActive(true);
            personas_GO.SetActive(true);
            timerVotos.ToggleTimerUI(false);
        }

        public void MostrarVotosActuales(SistemaVotacion.Acta[] actas)
        {
            acta_A.CargarInfo(actas[0]);
            acta_B.CargarInfo(actas[1]);

            float votos_A = actas[0].votos;
            float votos_B = actas[1].votos;           

            float totalVotos = votos_A +votos_B;
            barra_A.fillAmount =  totalVotos / votos_A;
            barra_B.fillAmount = votos_B / totalVotos;

            ActualizarPersonas(votos_A, personas_GO_izquierda);
            ActualizarPersonas(votos_B, personas_GO_derecha);
        }

        // Método para actualizar los hijos visibles según la cantidad de personas
        public void ActualizarPersonas(float cantidad, GameObject go_contenedor)
        {
            // Obtenemos la cantidad total de hijos en el contenedor
            int totalHijos = go_contenedor.transform.childCount;

            // Limitar el número de personas a la cantidad de hijos disponibles
            cantidad = Mathf.Clamp(cantidad, 0, totalHijos);

            // Recorre cada hijo del contenedor y los activa o desactiva según la cantidad de personas
            for (int i = 0; i < totalHijos; i++)
            {
                go_contenedor.transform.GetChild(i).gameObject.SetActive(i < cantidad);
            }
        }

    }
}