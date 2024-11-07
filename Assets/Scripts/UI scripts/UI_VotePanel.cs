using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SistemaVotacion;

namespace Viejo { 
public class UI_VotePanel : MonoBehaviour
{
    [SerializeField] VoteControl vControl;
    [SerializeField] VotingLogic logicaVotacion;
    [SerializeField] UI_PanelActa acta_izq;
    [SerializeField] UI_PanelActa acta_der;
    [Space]
    int votosA;
    int votosB;
    [Space]
    [SerializeField] Image barra_B;
    [SerializeField] Image barra_A;
    [SerializeField]float nuevoValor_A;
    [SerializeField]float nuevoValor_B;
    [Range(1f, 10f)] public float valorLerp = 3f;
    

    private void Start()
    {
        //Limpiar();
        //VoteControl.Ev_NuevoVotoSi += NuevoVoto;
        //VoteControl.Ev_NuevoVotoNo += NuevoVoto;
        VotingCounter.Ev_CambiarVotos += MostrarVotos;
        VotingLogic.Ev_Abre += MostrarPanelesDeActa;
        VotingLogic.Ev_Cierra += Limpiar;
    }

    public void MostrarVotos(SistemaVotacion.Acta[] actas)
    {
        int a = actas[0].votos;
        int b = actas[1].votos;
        votosA = a;
        votosB = b;
        int votosTotales = a + b;

        nuevoValor_A = votosA / (float)votosTotales;
        nuevoValor_B = votosB/ (float)votosTotales;

        if (nuevoValor_A < 0) nuevoValor_A = 0;
        if (nuevoValor_B < 0) nuevoValor_B = 0;


        Debug.Log(votosA + "(" + nuevoValor_A + ")" + " / " + votosB + "(" + nuevoValor_B + ")");
    }

    public void OverrideVotos(int a, int b)
    {
        votosA = a;
        votosB = b;
        int votosTotales = a + b;

        if(votosTotales <= 0)
        {
            nuevoValor_A = 0;
            nuevoValor_B = 0;
            return;
        }

        /*if(votosSi > votosNo)
        {
            nuevoValor_A = 1;
        }else
        if(votosNo > votosSi)
        {
            nuevoValor_B = 1;
        }else
        {
            if(votosNo == votosSi)
            {
                nuevoValor_A = .5f;
                nuevoValor_B = .5f;
            }
        }*/

        nuevoValor_A = votosA / (float)votosTotales;
        nuevoValor_B = votosB/ (float)votosTotales;

        if (nuevoValor_A < 0) nuevoValor_A = 0;
        if (nuevoValor_B < 0) nuevoValor_B = 0;


        Debug.Log(votosA + "(" + nuevoValor_A + ")" + " / " + votosB + "(" + nuevoValor_B + ")");

    }

    public void NuevoVoto(bool si)
    {
        Debug.Log("nuevo voto, UI vieja");
        if (si)
        {
            votosA++;
        }
        else
            votosB++;

        nuevoValor_A = votosA/3f;
        nuevoValor_B = votosA/3f;
    }

    private void Update()
    {
        if(barra_A && barra_B)
            MostrarBarras();
    }

    public void MostrarBarras()
    {
        float aValue = Mathf.Lerp( barra_A.fillAmount, nuevoValor_A, valorLerp * Time.deltaTime);
        float BValue = Mathf.Lerp( barra_B.fillAmount, nuevoValor_B, valorLerp * Time.deltaTime);

        barra_A.fillAmount = aValue;
        barra_B.fillAmount = BValue;
    }

    public void Mostrar()
    {
        Invoke("MostrarPanelesDeActa", .1f);
    }

    void MostrarPanelesDeActa()
    {
        gameObject.SetActive(true);
        acta_izq.CargarInfo(vControl.dataActual.actaA);
        acta_der.CargarInfo(vControl.dataActual.actaB);
    }

    public void Limpiar(float _tiempoEntreElecciones)
    {
        gameObject.SetActive(false);
        votosA = votosB = 0;
        if(barra_A && barra_B)
        {
            barra_A.fillAmount = 0;
            barra_B.fillAmount = 0;
        }
    }
}
}