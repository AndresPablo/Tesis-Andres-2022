using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Demokratos;
using System.Text.RegularExpressions;

namespace SistemaVotacion {

    public class UI_Acta : MonoBehaviour
    {
        [SerializeField] Canvas miCanvas;
        [SerializeField] CanvasGroup canvasGroup;
        [SerializeField] TextMeshProUGUI titulo_label;
        [SerializeField] TextMeshProUGUI frase_label;
        [SerializeField] TextMeshProUGUI adjetivo_label;
        [SerializeField] Image image_panel;
        [SerializeField] Image fondo_caja_icono;
        [SerializeField] Image icono;
        [SerializeField] UI_RefeColores paleta_colores;
        public Color mi_color;

        void Start()
        {
            miCanvas = GetComponent<Canvas>();
            canvasGroup = GetComponent<CanvasGroup>();
            ApagarVisuales();
        }

        public void CargarInfo(Acta acta)
        {
            //if(titulo_label) titulo_label.text = acta.votos + "";
            if(frase_label) frase_label.text = acta.name;
            //if(adjetivo_label) adjetivo_label.text = "";
            if(acta.sprite_A)
                icono.sprite = acta.sprite_A;


            switch (acta.efecto)
            {
                case (TipoEfecto.FOSIL):
                    mi_color = paleta_colores.colorFosil;
                    break;
                case (TipoEfecto.EOLICO):
                    mi_color = paleta_colores.colorEolica;
                    break;
                case (TipoEfecto.SOLAR):
                    mi_color = paleta_colores.colorSolar;
                    break;
                case (TipoEfecto.HIDRO):
                    mi_color = paleta_colores.colorHidro;
                    break;
                case (TipoEfecto.TERMICO):
                    mi_color = paleta_colores.colorTermica;
                    break;
                default:
                    mi_color = paleta_colores.colorNeutro;
                    break;
            }

            image_panel.color = mi_color;
            fondo_caja_icono.color = mi_color;
            //imagen_B.color = mi_color;

            // TODO: refactorizar
            /*
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
                        adjetivo.text = "SOLIDO";
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
                        adjetivo.text = "SOLIDO";
                        adjetivo.color = colorSolido;
                    }
                    break;

            }
            // TODO: Ver
            
            tileB.sprite = tileA.sprite;
            tileB.color = adjetivo.color;

            Invoke("OrdenarLayout", .05f);

            // Animacion
            if(esIsq == true)
            {
                GetComponent<Animator>().SetTrigger("OnMostrarIzquierda");
            }
            if (esDer)
            {
                GetComponent<Animator>().SetTrigger("OnMostrarDerecha");
            }
            */
            Mostrar();
        }
    
        public void Esconder(bool animar = true)
        {
            if(animar)
            {
                //AnimacionSalida();
                ApagarVisuales();
            }else
            {
                ApagarVisuales();
            }
        }

        public void ApagarVisuales()
        {
            miCanvas.enabled = false;
        }

        public void Mostrar()
        {
            miCanvas.enabled = true;
            // animacion
            //AnimacionEntrada();
        }

        #region ANIMACIONES
        void AnimacionEntrada()
        {
            FadeIn();
        }

        void AnimacionSalida()
        {
            //slide up over 1 second
            LeanTween.moveY(gameObject, -500f, 1f).setOnComplete( () => {
                ApagarVisuales(); 
         });; 
        }

        void AnimacionSacudir()
        {
            LeanTween.rotateAround(gameObject, Vector3.forward, 10f, 0.5f) ; //shake effect
        }

        void FadeIn()
        {
            //canvasGroup.alpha = 0;
            LeanTween.alpha(gameObject, 1f, 3f);
        }

        void EntrarDesdeArriba()
        {
            transform.position = new Vector2(transform.position.x, 1080); //asumiendo que la altura de la pantalla es 1080
            LeanTween.moveY(gameObject, 0, 1f).setEase(LeanTweenType.easeOutQuad);
        }
        #endregion
    }
}