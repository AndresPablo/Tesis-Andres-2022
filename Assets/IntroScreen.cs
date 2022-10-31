using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IntroScreen : MonoBehaviour
{
    [SerializeField] csOpenTSPSListener TSPS_Listener;
    [SerializeField] Canvas canvas;
    [Space]
    [SerializeField] Image barraIzq;
    [SerializeField] Image barraDer;
    [SerializeField] int cantidadMinima = 3;
    [SerializeField] Transform contenedorEstatuas;
    [SerializeField] TextMeshProUGUI readyLabel;
    public int cantidad;
    public float magnitudTotal;
    bool listo;


    // Aca tomamos la data del TSPS y la traducimos
    void MonitorearPosiciones()
    {
        int a = 0;
        int b = 0;
        float magnitud = 0;
        foreach (KeyValuePair<int, GameObject> blob in TSPS_Listener.blobGameObjects)
        {
            magnitud += blob.Value.transform.localScale.magnitude;
            if (blob.Value.transform.position.x < 0)
            {
                a++;
            }
            else
            {
                b++;
            }
        }

        magnitudTotal = magnitud;
        cantidad = a + b;

        foreach(Transform child in contenedorEstatuas)
        {
            child.gameObject.SetActive(false);
        }

        for (int i = 0; i < cantidad+1; i++)
        {
            contenedorEstatuas.GetChild(i).gameObject.SetActive(true);
        }

        if (cantidad >= cantidadMinima)
        {
            if(listo == false)
            {
                listo = true;
                readyLabel.text = "Pulsen un botón para empezar";
            }
        }
        else
        {
            listo = false;
            readyLabel.text = "Esperando jugadores...";
        }
    }



    public void Empezar()
    {
        canvas.enabled = false;
        GameManager.singleton.EmpezarJuego();
        this.enabled = false;
    }


    void Update()
    {
        MonitorearPosiciones();
        //barraIzq.fillAmount = (float)cantidad / cantidadMinima;
        //barraDer.fillAmount = (float)cantidad / cantidadMinima;

        if(listo)
        {
            if(Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2"))
            {
                Empezar();
            }
        }
    }
}
