using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_VotePanel : MonoBehaviour
{

    [SerializeField] UI_PanelActa acta_izq;
    [SerializeField] UI_PanelActa acta_der;
    public Text no_label;
    public Text si_label;
    int votosSi;
    int votosNo;
    [SerializeField] Image barra_si;
    [SerializeField] Image barra_no;
    [SerializeField] VoteControl vControl;

    private void Start()
    {
        Limpiar();
        VoteControl.Ev_NuevoVotoSi += NuevoVoto;
        VoteControl.Ev_NuevoVotoNo += NuevoVoto;
    }

    public void OverrideVotos(int a, int b)
    {
        votosSi = a;
        votosNo = b;
        barra_no.fillAmount = votosNo / 10f;
        barra_si.fillAmount = votosSi / 10f;
    }

    public void NuevoVoto(bool si)
    {
        if (si)
        {
            votosSi++;
        }
        else
            votosNo++;

        //si_label.text = "SI: "+votosSi;
        //no_label.text = "NO: "+votosNo;

        barra_no.fillAmount = votosNo / 20f;
        barra_si.fillAmount = votosSi / 20f;
    }

    public void Mostrar()
    {
        gameObject.SetActive(true);
        acta_izq.CargarInfo(vControl.dataActual.actaA);
        acta_der.CargarInfo(vControl.dataActual.actaB);
    }

    public void Limpiar()
    {
        gameObject.SetActive(false);
        //si_label.text = "SI: 0";
        //no_label.text = "NO: 0";
        votosSi = votosNo = 0;
        barra_no.fillAmount = 0;
        barra_si.fillAmount = 0;
    }
}
