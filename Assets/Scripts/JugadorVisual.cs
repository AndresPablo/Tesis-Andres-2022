using System.Collections;
using System.Collections.Generic;
using TarodevController;
using Unity.Mathematics;
using UnityEngine;
using Viejo;

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
    [SerializeField] Transform contenedorEfectos;
    [SerializeField] TrailRenderer rastro_velocidad;
    [SerializeField] ParticleSystem particulas_burbujas;
    [SerializeField] ParticleSystem particulas_lava;
    [SerializeField] ParticleSystem particulas_humo;
    [Space]
    [SerializeField] GameObject VFXMuerte_prefab;
    UI_RefeColores paletaColores;
    JugadorLogica logica;
    IPlayerController controlMov;
    Animator _anim;
    private bool _grounded;


    private void Start() {
        paletaColores = Game_Manager_Nuevo.singleton.Interfaz.paletaColores;
        logica = Game_Manager_Nuevo.singleton.Jugador;
        controlMov = GetComponent<IPlayerController>();
    }

    private void OnEnable() {
        JugadorLogica.Ev_Muere += Esconder;
        JugadorLogica.Ev_Muere += EmitirParticulas_Muerte;
        JugadorLogica.Ev_Spawnea += Mostrar;
        controlMov.Jumped += OnJumped;
        controlMov.GroundedChanged += OnGroundedChanged;
    }

    void Esconder()
    {
        cabeza.gameObject.SetActive(false);
        contenedorEfectos.gameObject.SetActive(false);
    }

    void Mostrar()
    {
        cabeza.gameObject.SetActive(true);
        contenedorEfectos.gameObject.SetActive(true);
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
        if(Game_Manager_Nuevo.singleton.Jugador.turboMode == false)
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
            transform.rotation = Quaternion.Euler(0,180, 0);
        }else
        {
            transform.rotation = Quaternion.Euler(0,0, 0);
        }
    }

    public void HandleSpriteFlip(float x)
    {/*
        bool derecha = x < 0;
        if (derecha)
        {
            float rotacionX = cabeza.transform.rotation.x;
            cabeza.transform
            cabeza.transform.rotation = new Quaternion.Euler( new Vector3(-rotacionX,0,0));  
        } */
    }

    private void OnGroundedChanged(bool grounded, float impact)
    {
        _grounded = grounded;
        
        if (grounded)
        {
            //_anim.SetTrigger(GroundedKey);
            //_source.PlayOneShot(_footsteps[Random.Range(0, _footsteps.Length)]);
            //_moveParticles.Play();

            //_landParticles.transform.localScale = Vector3.one * Mathf.InverseLerp(0, 40, impact);
            //_landParticles.Play();
        }
        else
        {
            //_moveParticles.Stop();
        }
    }

    private void OnJumped()
    {
        //_anim.SetTrigger(JumpKey);
        //_anim.ResetTrigger(GroundedKey);


        if (_grounded) // Avoid coyote
        {
            EmitirParticulas_Chispas();
        }
    }

    public void EmitirParticulas_Chispas()
    {
        particulas_lava.Emit(20);
    }

    public void EmitirParticulas_Muerte()
    {
        ParticleSystem p = Instantiate(VFXMuerte_prefab, transform.position, Quaternion.identity)
            .GetComponent<ParticleSystem>();
        //p.main.startColor.color = paletaColores.GetColorEnergia(Game_Manager_Nuevo.singleton.Jugador.tipoEnergia);
    }
}
}