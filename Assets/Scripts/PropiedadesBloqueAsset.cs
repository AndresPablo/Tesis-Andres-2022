using UnityEngine;

namespace Demokratos{
    [CreateAssetMenu(fileName ="Propiedad de bloque", menuName ="Propiedades")]
    public class PropiedadesBloqueAsset : ScriptableObject
    {
        public Color color;
        public bool letal;
        public bool fantasma;
        [Range(0,1.5f)]public float poderRebote;
    }
}