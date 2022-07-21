using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileProps : MonoBehaviour
{
    public TipoObjeto Tipo;
    public bool solido = true;
    public bool peligroso;
    public bool victoria;
    [Range(0,1.5f)]public float poderRebote;
    [SerializeField] Color colorNormal = Color.white;
    [SerializeField] Color colorVictoria = Color.cyan;
    [SerializeField] Color colorPeligro = Color.red;
    [SerializeField] Color colorRebote = Color.magenta;
    Collider2D mCol;
    SpriteRenderer mRenderer;
    Rigidbody2D mRb;

    private void Start()
    {
        mCol = GetComponent<Collider2D>();
        mRenderer = GetComponent<SpriteRenderer>();
        mRb = GetComponent<Rigidbody2D>();
        ActualizarVisuales();
    }

    void ActualizarVisuales()
    {
        
        if (peligroso)
        {
            mRenderer.color = colorPeligro;
        }else
        {
            if (poderRebote > 0)
            {
                mRenderer.color = colorRebote;
            }else
                mRenderer.color = Color.white;
        }

        Color colorActual = mRenderer.color;

        if (solido)
        {
            colorActual.a = 1f;
        }else
        {
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
                GameManager.singleton.player.Matar();
                return;
            }

            if (poderRebote > 0)
            {
                GameManager.singleton.player.Rebotar(poderRebote);
            }
        }

    }

    public Sprite GetMySprite()
    {
        return mRenderer.sprite;
    }
}
