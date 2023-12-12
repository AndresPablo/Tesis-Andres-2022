using UnityEngine;

namespace Demokratos{

    public class BloqueBase : MonoBehaviour
    {
        [SerializeField] protected PropiedadesBloqueAsset propiedades;
        BloqueVisual visual;

        protected void Awake() {
            visual = GetComponentInChildren<BloqueVisual>();
        }

        protected void Start()
        {
            
            if(propiedades)
            {
                visual.SetearColores(propiedades);
            }
        }

        public PropiedadesBloqueAsset GetPropiedades()
        {
            return propiedades;
        }

        public void SetPropiedades(PropiedadesBloqueAsset _p)
        {
            propiedades = _p;
        }
    }

}