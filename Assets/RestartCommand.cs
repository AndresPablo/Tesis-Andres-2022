using UnityEngine;
using Demokratos;

public class RestartCommand : MonoBehaviour
{
    public float holdDuration = 3f; // Tiempo que las teclas deben mantenerse presionadas
    private float holdTimer = 0f;  // Temporizador para rastrear el tiempo de presión

    void Update()
    {
        // Detectar si ambas teclas están presionadas
        if (Input.GetButton("Turbo") && Input.GetButton("Jump"))
        {
            holdTimer += Time.deltaTime;

            // Si se supera la duración establecida, se reinicia el juego
            if (holdTimer >= holdDuration)
            {
                Orenar_Reinicio();
            }
        }
        else
        {
            // Reiniciar el temporizador si no se mantienen ambas teclas
            holdTimer = 0f;
        }
    }

    void Orenar_Reinicio()
    {
        holdTimer = 0f;
        // GM reinicia todo
        Debug.Log("HARD RESET");
        Game_Manager_Nuevo.singleton.Reiniciar();
    }
}
