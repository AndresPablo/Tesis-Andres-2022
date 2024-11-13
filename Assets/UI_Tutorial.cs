using UnityEngine.SceneManagement;
using UnityEngine;

public class UI_Tutorial : MonoBehaviour
{
public GameObject[] tutorialPanels; // Lista de paneles del tutorial.
    public string targetScene = "MainGame"; // Escena a cargar al finalizar el tutorial.
    private int currentPanelIndex = 0; // Índice del panel actual.

    void Start() {
        ShowPanel(0); // Muestra el primer panel.
    }

    void Update() {
        // Avanza al siguiente panel al presionar cualquier tecla o clic.
        if (Input.anyKeyDown) {
            ShowNextPanel();
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
            LoadTargetScene(); // Si no hay más paneles, carga la escena objetivo.
        }
    }

    private void LoadTargetScene() {
        SceneManager.LoadScene(targetScene);
    }
}
