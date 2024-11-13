using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Demokratos;

public class UI_ScreenVictoria : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI muertesLabel;
    [SerializeField] TextMeshProUGUI tiempoLabel;

    private void Start()
    {
        AbrirPantalla();
    }

    void Update()
    {
        if(Input.GetButtonDown("Turbo") || Input.anyKey || Input.GetButtonDown("Jump"))
        {
            gameObject.SetActive(false);
            Game_Manager_Nuevo.singleton.Reiniciar();
        }
    }

    public void AbrirPantalla()
    {
        muertesLabel.text = Game_Manager_Nuevo.singleton.muertes.ToString("0");

        float tiempoTotal = Game_Manager_Nuevo.singleton.tiempo;
        int minutes = Mathf.FloorToInt(tiempoTotal / 60F);
        int seconds = Mathf.FloorToInt(tiempoTotal - minutes * 60);
        string niceTime = string.Format("{0:00}:{1:00}", minutes, seconds);
        tiempoLabel.text = niceTime;
    }
}
