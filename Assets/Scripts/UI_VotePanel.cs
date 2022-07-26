using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_VotePanel : MonoBehaviour
{
    [SerializeField] VoteControl vControl;
    [SerializeField] UI_PanelActa acta_izq;
    [SerializeField] UI_PanelActa acta_der;
    [Space]
    int votosSi;
    int votosNo;
    [Space]
    [SerializeField] Image barra_B;
    [SerializeField] Image barra_A;
    [SerializeField]float nuevoValor_A;
    [SerializeField]float nuevoValor_B;
    [Range(1f, 10f)] public float valorLerp;
    

    private void Start()
    {
        Limpiar();
        VoteControl.Ev_NuevoVotoSi += NuevoVoto;
        VoteControl.Ev_NuevoVotoNo += NuevoVoto;
        Mostrar();
    }

    public void OverrideVotos(int a, int b)
    {
        votosSi = a;
        votosNo = b;
        int votosTotales = a + b;

        nuevoValor_A = votosSi/ (float)votosTotales;
        nuevoValor_B = votosNo/ (float)votosTotales;
    }

    public void NuevoVoto(bool si)
    {
        if (si)
        {
            votosSi++;
        }
        else
            votosNo++;

        nuevoValor_A = votosSi/3f;
        nuevoValor_B = votosSi/3f;
    }

    private void Update()
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

    public void Limpiar()
    {
        gameObject.SetActive(false);
        votosSi = votosNo = 0;
        barra_A.fillAmount = 0;
        barra_B.fillAmount = 0;
    }
}
