using UnityEngine.SceneManagement;
using UnityEngine;
using Demokratos;


namespace Demokratos.UI{
public class UI_Tutorial : MonoBehaviour
{
public GameObject[] tutorialPanels; // Lista de paneles del tutorial.
    private int currentPanelIndex = 0; // Índice del panel actual.
    [SerializeField] Canvas canvas;

    void Start() {
        ShowPanel(0); // Muestra el primer panel.
    }

    void Update() {
        if(Game_Manager_Nuevo.estadoJuego == GameState.TUTORIAL)
        {
            // Avanza al siguiente panel al presionar cualquier tecla o clic.
            if (Input.anyKeyDown)
            {
                ShowNextPanel();
            }
        }

    }

    private void ShowPanel(int index) {
        // Desactiva todos los paneles y activa solo el actual.
        for (int i = 0; i < tutorialPanels.Length; i++) {
            tutorialPanels[i].SetActive(i == index);
        }
    }

    private void ShowNextPanel() {
        currentPanelIndex++;

        if (currentPanelIndex < tutorialPanels.Length) {
            ShowPanel(currentPanelIndex); // Muestra el siguiente panel.
        } else {
            TerminarTutorial(); // Si no hay más paneles, carga la escena objetivo.
        }
    }

    private void TerminarTutorial()
    {
        currentPanelIndex = 0;
        ShowPanel(0);
        Game_Manager_Nuevo.singleton.EmpezarPartida();
    }

    public void Mostrar(bool _estado){
        canvas.enabled = _estado;
    }
}
}
