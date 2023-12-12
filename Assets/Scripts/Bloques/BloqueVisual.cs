using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demokratos{
    public class BloqueVisual : MonoBehaviour
    {
        [SerializeField] SpriteRenderer sprite;

        public void SetearColores(PropiedadesBloqueAsset _prop)
        {
            sprite.color = _prop.color;
        }
    }
}