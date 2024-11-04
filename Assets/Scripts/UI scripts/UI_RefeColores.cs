using UnityEngine;

namespace Demokratos {

    [CreateAssetMenu(fileName ="Nueva Paleta", menuName ="Paleta de Colores")]
    public class UI_RefeColores : ScriptableObject
    {
        [Header("Legado 2022 - Demokratos")]
        public Color colorSolido = Color.white;
        public Color colorFantasma = Color.gray;
        public Color color_Gelatina = Color.magenta;
        public Color color_letal = Color.red;
        public Color color_victoria = Color.cyan;
        [Header("Energia 2024 - DemokriBot")]
        public Color colorNeutro = Color.white;
        public Color colorLegadoDefault = Color.cyan;
        public Color colorFosil = Color.grey;
        public Color colorEolica = Color.green;
        public Color colorTermica = Color.red;
        public Color colorHidro = Color.blue;
        public Color colorSolar = Color.yellow;
        public Color colorGrisApagado = Color.gray;
        [Header("Energia 2024 - DemokriBot")]
        [SerializeField] Sprite icono_default;
        [SerializeField] Sprite iconoFosil;
        [SerializeField] Sprite iconoEolica;
        [SerializeField] Sprite iconoHidro;
        [SerializeField] Sprite iconoTermica;
        [SerializeField] Sprite iconoSolar;

        public Color GetColorEnergia(TipoEnergia tipo){
            switch(tipo)
            {
                case TipoEnergia.NINGUNO:
                    return colorNeutro; 
                case TipoEnergia.FOSIL:
                    return colorFosil;
                break;
                case TipoEnergia.EOLICA:
                    return colorEolica;
                break;
                case TipoEnergia.HIDRO:
                    return colorHidro;
                break;
                case TipoEnergia.TERMO:
                    return colorTermica;
                break;
                case TipoEnergia.SOLAR:
                    return colorSolar;
                break;
                default:
                    return Color.white;
                    break;
            }
        }

        public Sprite GetIconoEnergia(TipoEnergia tipo){
            switch(tipo)
            {
                case TipoEnergia.NINGUNO:
                    return icono_default; 
                case TipoEnergia.FOSIL:
                    return iconoFosil;
                break;
                case TipoEnergia.EOLICA:
                    return iconoEolica;
                break;
                case TipoEnergia.HIDRO:
                    return iconoHidro;
                break;
                case TipoEnergia.TERMO:
                    return iconoTermica;
                break;
                case TipoEnergia.SOLAR:
                    return iconoSolar;
                break;
                default:
                    return icono_default;
                    break;
            }
        }


    }
    
}