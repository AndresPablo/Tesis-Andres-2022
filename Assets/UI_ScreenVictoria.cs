using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_ScreenVictoria : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI muertesLabel;
    [SerializeField] TextMeshProUGUI tiempoLabel;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (gameObject.activeSelf == false)
            return;

        if(Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2"))
        {
            GameManager.singleton.Reiniciar();
            gameObject.SetActive(false);
        }
    }

    public void AbrirPantalla()
    {
        muertesLabel.text = GameManager.singleton.muertes.ToString("0");

        float tiempoTotal = GameManager.singleton.tiempo;
        int minutes = Mathf.FloorToInt(tiempoTotal / 60F);
        int seconds = Mathf.FloorToInt(tiempoTotal - minutes * 60);
        string niceTime = string.Format("{0:00}:{1:00}", minutes, seconds);
        tiempoLabel.text = niceTime;
    }
}
