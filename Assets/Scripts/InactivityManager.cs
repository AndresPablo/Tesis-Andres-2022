using UnityEngine;
using UnityEngine.SceneManagement;


namespace Demokratos { 
public class InactivityManager : MonoBehaviour {
    public float inactivityThreshold = 60f; // Tiempo en segundos antes de cambiar de escena.

    private float timeSinceLastActivity = 0f;

    void Update() {
        // Incrementa el tiempo de inactividad.
        timeSinceLastActivity += Time.deltaTime;

        // Detecta actividad del teclado o del ratón.
        if (Input.anyKeyDown || Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0) {
            timeSinceLastActivity = 0f; // Reinicia el contador al detectar actividad.
        }

        // Comprueba si el tiempo de inactividad supera el umbral.
        if (timeSinceLastActivity >= inactivityThreshold) {
            LoadTargetScene();
        }
    }

    private void LoadTargetScene() {
        Game_Manager_Nuevo.singleton.Reiniciar();
    }
}
}