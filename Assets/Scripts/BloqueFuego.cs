using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demokratos {
 
    public class BloqueFuego : BloqueBase
    {
        [SerializeField] float tiempoEntreFuegos = 2f;
        [SerializeField] float duracionFuego = 1f;
        [SerializeField] AudioClip SFX_arde;
        [SerializeField] GameObject[] areasFuego;

        protected void Start() {
            base.Start();
            Invoke("EncenderTodos", tiempoEntreFuegos);
        }

        public void ApagarFuegos()
        {
            if(areasFuego.Length <= 0)
            {
                Debug.LogWarning("No hay areas de fuego");
                return;
            }
            foreach(GameObject llama in areasFuego)
            {
                llama.SetActive(false);
            }
            Invoke("EncenderTodos", tiempoEntreFuegos);
        }

        public void EncenderFuego(int _i)
        {
            if(areasFuego.Length <= 0)
            {
                Debug.LogWarning("No hay areas de fuego");
                return;
            }
            areasFuego[_i].SetActive(true);
            AudioManager.instance.PlayOneShot(SFX_arde);
            Invoke("ApagarFuegos", duracionFuego);
        }

        public void EncenderTodos()
        {
            if(areasFuego.Length <= 0)
            {
                Debug.LogWarning("No hay areas de fuego");
                return;
            }
            for (int i = 0; i < areasFuego.Length; i++)
            {
                EncenderFuego(i);
            }
        }
        
    }

}