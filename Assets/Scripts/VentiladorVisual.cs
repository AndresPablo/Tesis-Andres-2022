using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demokratos.Energia{
    public class VentiladorVisual : BloqueVisual
    {
        [SerializeField] private float speed = 30f;
        [SerializeField] private  bool clockwise = true;
        [SerializeField] SpriteRenderer ventiladorSprite;

        protected void SetearColores(PropiedadesBloqueAsset _prop)
        {
            base.SetearColores(_prop);
            ventiladorSprite.color = _prop.color;
        }
        
        void Update() 
        {
            if(clockwise)
            {
                ventiladorSprite.transform.Rotate(0, 0, speed * Time.deltaTime);  
            }
            else
            {
                ventiladorSprite.transform.Rotate(0, 0, -speed * Time.deltaTime); 
            }
        }
        
        public void ToggleDirection()
        {
            clockwise = !clockwise; 
        }
        
        public void Stop()
        {
            speed = 0f; 
        }
    }
}