using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SistemaVotacion;
using UnityEngine.UI;
using TMPro;

namespace Demokratos.UI {


    public class UI_TimerVotos : MonoBehaviour
    {
        [SerializeField] GameObject panel;
        [SerializeField] Image fill_imagen_reloj;
        [SerializeField] TextMeshProUGUI label_temporizador; 

        float timerDuration = 10f;  // Duración total del timer (en segundos)
        private float timeRemaining;      // Tiempo restante

        private bool isTimerRunning = false;  // Control para saber si el timer está corriendo

        void Start()
        {
            ToggleTimerUI(false);
        }

        void Update()
        {
            if (isTimerRunning)
            {
                if (timeRemaining > 0)
                {
                    // Reducir el tiempo restante
                    timeRemaining -= Time.deltaTime;

                    // Actualizar el texto del timer
                    UpdateTimerText(timeRemaining);

                    // Actualizar el valor de la barra de progreso (llenado circular)
                    UpdateTimerFill(timeRemaining);
                }
                else
                {
                    // Asegurarse de que el tiempo no sea negativo
                    timeRemaining = 0;
                    isTimerRunning = false;
                    ToggleTimerUI(false);
                }
            }
        }

        // Iniciar el timer
        public void StartTimer(float duration)
        {
            timerDuration = duration;
            timeRemaining = timerDuration;
            isTimerRunning = true;
        }

        // Actualiza el texto del timer
        void UpdateTimerText(float time)
        {
            int seconds = Mathf.CeilToInt(time);  // Redondea hacia arriba el tiempo restante
            label_temporizador.text = seconds.ToString();  // Muestra solo los segundos

        }

        // Actualiza la barra de progreso circular (fill 360)
        void UpdateTimerFill(float time)
        {
            float fillAmount = Mathf.Clamp01(time / timerDuration);  // Normaliza el tiempo restante (de 1 a 0)
            fill_imagen_reloj.fillAmount = fillAmount;  // Actualiza la cantidad de llenado de la imagen
        }

        public void ToggleTimerUI(bool isVisible)
        {
            panel.SetActive(isVisible);
        }
    }

}