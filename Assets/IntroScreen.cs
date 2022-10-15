using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroScreen : MonoBehaviour
{
    [SerializeField] csOpenTSPSListener TSPS_Listener;
    [SerializeField] Canvas canvas;
    [Space]
    [SerializeField] Image barraIzq;
    [SerializeField] Image barraDer;
    [SerializeField] int cantidadMinima = 3;
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


        if (cantidad >= cantidadMinima)
        {
            if(listo == false)
            {
                Invoke("OnListoParaEmpezar", 5f);
                listo = true;
            }
            
        }
    }

    public void OnListoParaEmpezar()
    {
        canvas.enabled = false;
        GameManager.singleton.EmpezarJuego();
        this.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        MonitorearPosiciones();
        barraIzq.fillAmount = (float)cantidad / cantidadMinima;
        barraDer.fillAmount = (float)cantidad / cantidadMinima;
    }
}
