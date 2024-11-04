using UnityEngine;

namespace Demokratos {

    public enum TipoEfecto { GRAV, NO_ENERGIA, EOLICO, HIDRO, SOLAR, TERMICO, SWAP, FOSIL, COUNT}

    [CreateAssetMenu(fileName ="Nuevo efecto", menuName ="Efecto")]
    public class EfectoAsset : ScriptableObject
    {
        
    }

}