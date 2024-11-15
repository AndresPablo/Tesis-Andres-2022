using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Demokratos;

namespace Demokratos.UI{
public class UI_ScreenVictoria : MonoBehaviour
{
    [SerializeField] GameObject panelGO;
    [SerializeField] TextMeshProUGUI muertesLabel;
    [SerializeField] TextMeshProUGUI tiempoLabel;
    int vecestocado; 

    private void Start()
    {
        EsconderPantalla();
    }

    void Update()
    {
        // si el panel no esta activo no hacemos nada
        if(panelGO.activeSelf == false)
            return;
        
        // si esta abierta la pantalla, reiniciamos el juego al tocar un boton
        if(Input.GetButtonDown("Turbo") || Input.GetButtonDown("Jump"))
        {
            vecestocado++;

            if (vecestocado >= 3)
            {
                EsconderPantalla();
                Game_Manager_Nuevo.singleton.Reiniciar();
            }
        }
    }

    public void AbrirPantalla()
    {
        panelGO.SetActive(true);
        muertesLabel.text = Game_Manager_Nuevo.singleton.muertes.ToString("0");

        float tiempoTotal = Game_Manager_Nuevo.singleton.tiempo;
        int minutes = Mathf.FloorToInt(tiempoTotal / 60F);
        int seconds = Mathf.FloorToInt(tiempoTotal - minutes * 60);
        string niceTime = string.Format("{0:00}:{1:00}", minutes, seconds);
        tiempoLabel.text = niceTime;
    }

    public void EsconderPantalla(){
        panelGO.SetActive(false);
            vecestocado = 0;
    }
}
}