using UnityEngine;
using TMPro;

namespace Demokratos {
    public class UI_NuevoNivelLabel : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI label;
        [SerializeField] Animator animator;
        int nivel;

    void Start(){
        Game_Manager_Nuevo.Ev_PasoNivel += ActualizarNivelActual;
    }

    public void ActualizarNivelActual(int _nivelIndex){
            nivel = _nivelIndex;
            ActualizarTexto(nivel);
            animator.Play("NivelArribaIntro");
    }

    public void Ocultar(){
            label.text = "";
    }

    void ActualizarTexto(int _nivel)
        {
            label.text = "Nivel " + _nivel;
        }
    }
}