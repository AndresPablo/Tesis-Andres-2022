using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demokratos.Energia {
 
    public class BloqueFuego : BloqueBase
    {
        [SerializeField] AreaFuego areaFuego;
        [SerializeField] [Range(.5f, 5f)]float recarga = 1f;
        [SerializeField] float tiempoEntreFuegos = 2f;
        [SerializeField] float duracionFuego = 1f;
        [SerializeField] AudioClip SFX_arde;
        [SerializeField] GameObject[] areasFuego;
        [SerializeField] ParticleSystem particulas; // TODO: mover a su propio visual
        [SerializeField] AudioSource fuenteAudio;

        protected void Start() {
            base.Start();
            fuenteAudio.clip = SFX_arde;
            areaFuego.recarga = this.recarga;
            if(areasFuego.Length <= 0)
            {
                Debug.LogWarning("No hay areas de fuego");
                return;
            }else
            {
                Invoke("EncenderTodos", tiempoEntreFuegos);
            }
        }

        public void ApagarFuegos()
        {
            foreach(GameObject llamaGO in areasFuego)
            {
            }
            areaFuego.SetEstadoCollider(false);
            Invoke("EncenderTodos", tiempoEntreFuegos);
        }

        public void EncenderFuego(int _i)
        {
            GameObject llamaGO = areasFuego[_i];
            fuenteAudio.Play();
            ParticleSystem particulas = llamaGO.GetComponent<ParticleSystem>();
            areaFuego.SetEstadoCollider(true);
            particulas.Play(); // TODO: hacer que el tiempo de emision sea dinamico, esta coordinado con este script manualmente
            Invoke("ApagarFuegos", duracionFuego);
        }

        public void EncenderTodos()
        {
            for (int i = 0; i < areasFuego.Length; i++)
            {
                EncenderFuego(i);
            }
        }
        
    }

}