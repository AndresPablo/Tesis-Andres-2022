using UnityEngine;

public enum PlayerState { IDLE, WALK, AIR, DEAD }
public class Jugador : MonoBehaviour
{
    CharacterController2D charControl;
    Rigidbody2D rb;
    Collider2D col;
    Animator anim;
    [SerializeField]SpriteRenderer cabeza;
    [SerializeField] SpriteRenderer pieFrente;
    [SerializeField] SpriteRenderer pieAtras;
    [SerializeField] GameObject graficos;
    public PlayerState Estado;
    [Space]
    [SerializeField] AudioClip salto_SFX;
    [SerializeField] AudioClip rebote_SFX;
    [SerializeField] AudioClip spawn_SFX;
    [SerializeField] AudioClip muerte_SFX;

    public delegate void JugadorDelegate();
    public static event JugadorDelegate Ev_Muere;
    public static event JugadorDelegate Ev_Spawnea;


    void Start()
    {
        charControl = GetComponent<CharacterController2D>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        VoteEffector.Ev_Gravedad += OnSwitchGravity;
    }

    public void Matar()
    {
        if (Estado == PlayerState.DEAD)
            return;
        Estado = PlayerState.DEAD;
        //graficos.SetActive(false);
        //charControl.enabled = false;
        col.enabled = false;
        anim.SetBool("isDead", true);
        if (Ev_Muere != null)
        {
            Ev_Muere.Invoke();
        }
        if (muerte_SFX) AudioManager.instance.PlayOneShot(muerte_SFX);
    }

    public void Spawn()
    {
        Estado = PlayerState.IDLE;
        graficos.SetActive(true);
        col.enabled = true;
        charControl.enabled = true;
        anim.SetBool("isDead", false);
        rb.velocity = Vector2.zero;
        if (spawn_SFX) AudioManager.instance.PlayOneShot(spawn_SFX);
        if (Ev_Spawnea != null)
        {
            Ev_Spawnea.Invoke();
        }
    }

    public void Rebotar(float fuerza = 1f)
    {
        charControl.ForzarSalto(fuerza);
        if (rebote_SFX) AudioManager.instance.PlayOneShot(rebote_SFX);
    }

    public void OnSwitchGravity(bool state)
    {
        rb.gravityScale = (rb.gravityScale * -1);
        transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
    }

    public void CambiarEstado(PlayerState _nuevoEstado)
    {
        if (Estado == _nuevoEstado)
            return;
        /*if(Estado == PlayerState.IDLE)
        {
            anim.Play("Idle");
        }else
        if (Estado == PlayerState.AIR)
        {
            anim.Play("Air");
        }
        else
        if (Estado == PlayerState.DEAD)
        {
            
        }
        else
        if (Estado == PlayerState.WALK)
        {
            anim.Play("Walk");
        }*/
    }

    public void ApagarCharControl()
    {
        charControl.enabled = false;
    }
}
