using UnityEngine;

namespace Demokratos {

    [CreateAssetMenu(fileName ="Nueva Paleta", menuName ="Paleta de Colores")]
    public class UI_RefeColores : ScriptableObject
    {
        [Header("Energia 2024 - DemokriBot")]
        public Color colorNeutro = Color.white;
        public Color colorLegadoDefault = Color.cyan;
        public Color colorFosil = Color.grey;
        public Color colorEolica = Color.green;
        public Color colorTermica = Color.red;
        public Color colorHidro = Color.blue;
        public Color colorSolar = Color.yellow;
        [Header("Legado 2022 - Demokratos")]
        public Color colorSolido = Color.white;
        public Color colorFantasma = Color.gray;
        public Color color_Gelatina = Color.magenta;
        public Color color_letal = Color.red;
        public Color color_victoria = Color.cyan;

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
    }
    
}