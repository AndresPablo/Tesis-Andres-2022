using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demokratos{
public class JugadorVisual : MonoBehaviour
{
    [SerializeField] SpriteRenderer cabeza;
    [SerializeField] SpriteRenderer ojos;
    [SerializeField] SpriteRenderer pieIzq;
    [SerializeField] SpriteRenderer pieDer;
    [Space]
    [SerializeField] Sprite ojosOFF;
    [SerializeField] Sprite ojosON;
    [Space]
    [SerializeField] TrailRenderer rastro_velocidad;
    [SerializeField] ParticleSystem particulas_burbujas;
    [SerializeField] ParticleSystem particulas_lava;
    [SerializeField] ParticleSystem particulas_humo;
    UI_RefeColores paletaColores;

    private void Start() {
        paletaColores = Game_Manager_Nuevo.singleton.Interfaz.paletaColores;
    }

    private void OnEnable() {
        
    }

    public void SetEmisionRastro(bool estado)
    {
        rastro_velocidad.emitting = estado;
    }

    public void SetColorRastro(TipoEnergia tipoEnergia)
    {
        rastro_velocidad.startColor = paletaColores.GetColorEnergia(tipoEnergia);
    }

    public void UpdateColorEnergia(TipoEnergia tipo)
    {
        SetColorRastro(tipo);
        PintarPJ(paletaColores.GetColorEnergia(tipo));
        // apagar todos los efectos
        particulas_humo.gameObject.SetActive(false);
        particulas_burbujas.gameObject.SetActive(false);
        particulas_lava.gameObject.SetActive(false);
        SetEmisionRastro(false);
        // luego encendemos los correspondientes
        switch(tipo)
        {
            case TipoEnergia.FOSIL:
                particulas_humo.gameObject.SetActive(true);
            break;
            case TipoEnergia.EOLICA:
               //...
            break;
            case TipoEnergia.HIDRO:
                particulas_burbujas.gameObject.SetActive(true);
            break;
            case TipoEnergia.TERMO:
                particulas_lava.gameObject.SetActive(true);
            break;
            case TipoEnergia.SOLAR:
                //...
            break;
        }
    }

    void PintarPJ(Color nuevoColor)
    {
        cabeza.color = nuevoColor;
        ojos.color = nuevoColor;
        pieDer.color = nuevoColor;
        pieIzq.color = nuevoColor;
    }

    public void SetSpriteDirection(bool derecha)
    {
        Vector2 newScale = transform.localScale;
        if(derecha)
        {
            newScale.x = Mathf.Abs(transform.localScale.x);
        }else
        {
            newScale.x = -Mathf.Abs(transform.localScale.x);
        }
        transform.localScale = newScale;
    }

    public void EmitirParticulas_Chispas()
    {
        particulas_lava.Emit(20);
    }
}
}