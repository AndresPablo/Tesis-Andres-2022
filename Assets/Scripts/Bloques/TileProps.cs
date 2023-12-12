using Demokratos;
using UnityEngine;
using Viejo;

public class TileProps : MonoBehaviour
{
    public TipoObjeto Tipo;
    public bool solido = true;
    public bool peligroso;
    public bool victoria;
    [Range(0,3f)]public float poderRebote = 1f;
    UI_RefeColores paletaColores;
    Collider2D mCol;
    SpriteRenderer mRenderer;
    Rigidbody2D mRb;

    private void Start()
    {
        mCol = GetComponent<Collider2D>();
        mRenderer = GetComponentInChildren<SpriteRenderer>();
        mRb = GetComponent<Rigidbody2D>();
        paletaColores = Game_Manager_Nuevo.singleton.Interfaz.paletaColores;
        ActualizarVisuales();
    }

    void ActualizarVisuales()
    {
        
        if (peligroso)
        {
            mRenderer.color = paletaColores.color_letal;
        }else
        {
            if (poderRebote > 0)
            {
                mRenderer.color = paletaColores.color_Gelatina;
            }else
                mRenderer.color = paletaColores.colorNeutro;
        }

        Color colorActual = mRenderer.color;

        if (solido)
        {
            colorActual.a = 1f;
        }else
        {
            colorActual = paletaColores.colorFantasma;
            colorActual.a = .5f;
        }

        mRenderer.color = colorActual;
    }

    public void SetSolido()
    {
        solido = !solido;
        mCol.enabled = solido;
        ActualizarVisuales();
    }

    public void SetRebote(float valor)
    {
        poderRebote = valor;
        ActualizarVisuales();
    }

    public void SetPeligro()
    {
        peligroso = !peligroso;
        ActualizarVisuales();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!solido) return;

        if(collision.transform.CompareTag("Player"))
        {
            if (peligroso)
            {
                //TODO: obsoleta GameManager.singleton.player.Matar();
                return;
            }

            if (poderRebote > 0)
            {
                //TODO: obsoleta GameManager.singleton.player.Rebotar(poderRebote);
                Game_Manager_Nuevo.singleton.Jugador.Rebotar(poderRebote);
            }
        }

    }

    public Sprite GetMySprite()
    {
        return mRenderer.sprite;
    }
}
