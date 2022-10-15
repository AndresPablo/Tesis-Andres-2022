using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_PanelActa : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI frase;
    [SerializeField] TextMeshProUGUI adjetivo;
    [SerializeField] Image tileA;
    [SerializeField] Image tileB;
    [Space]
    [SerializeField] Color colorSolido = Color.white;
    [SerializeField] Color colorFantasma = Color.white;
    [SerializeField] Color colorGelatina = Color.magenta;
    [SerializeField] Color colorPeligro = Color.red;
    [SerializeField] Color colorVictoria = Color.cyan;

    private void Start()
    {
        LimpiarInfo();
    }

    public void CargarInfo(Acta acta)
    {
        tileA.color = acta.color;
        tileB.enabled = true;

        switch (acta.Tipo)
        {
            case (TipoVoto.GRAV):
                frase.text = "Invertir la\nGRAVEDAD!";
                adjetivo.text = "";
                tileA.enabled = false;
                tileB.enabled = false;
                break;
            case (TipoVoto.TILE_SOLID):
                tileA.sprite = acta.sprite;
                tileA.enabled = true;
                if(acta.estado == false)
                {
                    frase.text = "Convertir en";
                    adjetivo.text = "FANTASMA";
                    adjetivo.color = colorFantasma;
                }
                else
                {
                    frase.text = "Convertir en";
                    adjetivo.text = "SOLIDO";
                    adjetivo.color = colorSolido;

                }
                break;
            case (TipoVoto.TILE_DANGER):
                tileA.sprite = acta.sprite;
                tileA.enabled = true;
                if (acta.estado == false)
                {
                    frase.text = "Convertir en";
                    adjetivo.text = "INOFENSIVO";
                    adjetivo.color = colorSolido;
                }
                else
                {
                    frase.text = "Convertir en";
                    adjetivo.text = "PELIGROSO!";
                    adjetivo.color = colorPeligro;
                }
                break;
            case (TipoVoto.TILE_GELA):
                tileA.sprite = acta.sprite;
                tileA.enabled = true;
                if (acta.numf > 0)
                {
                    frase.text = "Convertir en";
                    adjetivo.text = "GELATINA!";
                    adjetivo.color = colorGelatina;
                }
                else
                {
                    frase.text = "Convertir en";
                    adjetivo.text = "DURO";
                    adjetivo.color = colorSolido;
                }
                break;

        }
        // TODO: Ver
        
        tileB.sprite = tileA.sprite;
        tileB.color = adjetivo.color;

        Invoke("OrdenarLayout", .05f);

        // Animacion
        GetComponent<Animator>().Play("mostrar");
    }

    void LimpiarInfo()
    {
        GetComponent<Animator>().Play("oculto");
        tileA.enabled = false;
        tileB.enabled = false;
        frase.text = "descripcion";
        adjetivo.text = "adjetivo";
        adjetivo.color = colorSolido;
    }

    void OrdenarLayout()
    {
        tileA.enabled = !tileA.enabled;
        tileA.enabled = !tileA.enabled;
    }
}
