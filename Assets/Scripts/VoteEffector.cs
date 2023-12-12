using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;
using Viejo;

public class VoteEffector : MonoBehaviour
{
    public delegate void EfectoVoid();
    public delegate void EfectoBool(bool state);
    public static event EfectoVoid Ev_CambiarTile;
    public static event EfectoBool Ev_Gravedad;
    public static event EfectoBool Ev_TilePeligro;
    public static event EfectoBool Ev_TileSolida;

    public TipoObjeto[] objectosPosibles;
    public TipoObjeto objetosAfectados;
    public GameObject[] TilePrefabs;


    public void ProcesarResultado(Acta resultado)
    {
        // TODO: MUCHO TRABAJO ACA
        switch (resultado.Tipo)
        {
            case TipoVoto.GRAV:
                if (Ev_Gravedad != null)
                { Ev_Gravedad.Invoke(true); }
                break;
            case TipoVoto.TILE_DANGER:
                GameManager.singleton.SetPeligroTodos(resultado.tipoObjeto);
                break;
            case TipoVoto.TILE_GELA:
                GameManager.singleton.CambiarReboteDeObjetos(resultado.tipoObjeto, resultado.numf);
                break;
            case TipoVoto.TILE_SOLID:
                GameManager.singleton.SetSolidezTodos(resultado.tipoObjeto);
                break;
        }
    }

    // Genera la votacion (poll)
    public VotacionData GetRandomPoll()
    {
        int A;
        int B;
        A = Random.Range(0, (int)TipoVoto.COUNT);
        B = Random.Range(0, (int)TipoVoto.COUNT);
        while (A == B)
        {
            B = Random.Range(0, (int)TipoVoto.COUNT);
        }
        TipoVoto _tipoA = (TipoVoto)A;
        TipoVoto _tipoB = (TipoVoto)B;
        Debug.Log(_tipoA + " "+ _tipoB);

        TipoObjeto tipoObj_A = GetTipodeObjetoRandom();
        TipoObjeto tipoObj_B = GetTipodeObjetoRandom();
        GameObject obj_A = GameManager.singleton.GetObjetoDeTipoEnNivel(tipoObj_A);
        GameObject obj_B = GameManager.singleton.GetObjetoDeTipoEnNivel(tipoObj_B);
        //Debug.Log(obj_A.name + " " + obj_B.name);
        //Debug.Log(tipoObj_A + " " + tipoObj_B);
        Acta acta_A = new Acta(_tipoA, obj_A);
        Acta acta_B = new Acta(_tipoB, obj_B);
        VotacionData data = new VotacionData(acta_A, acta_B);
        
        return data;
    }

    public TipoObjeto GetTipodeObjetoRandom()
    {
        int index = Random.Range(1, objectosPosibles.Length);
        return objectosPosibles[index];
    }

    public GameObject GetRandomPrefab()
    {
        return GameManager.singleton.GetRandomObjectFromlevel();
        //return TilePrefabs[Random.Range(0, TilePrefabs.Length)];
    }
}

[System.Serializable]
public class VotacionData
{
    public Acta actaA;
    public Acta actaB;

    public VotacionData(Acta _a, Acta _b)
    {
        actaA = _a;
        actaB = _b;
    }
}

[System.Serializable]
public class Acta
{
    public TipoVoto Tipo;
    public int num;
    public float numf;
    public bool estado;
    public TipoObjeto tipoObjeto;
    public Sprite sprite;
    public Color color;
    public GameObject TileSustituta;

    public Acta(TipoVoto _Tipo, GameObject sampleGO)
    {
        Tipo = _Tipo;
        SpriteRenderer _renderer = sampleGO.GetComponent<SpriteRenderer>();
        if (_renderer != null)
        {
            sprite = _renderer.sprite;
            color = sampleGO.GetComponent<SpriteRenderer>().color;
        }
        TileProps props = sampleGO.GetComponent<TileProps>();
        tipoObjeto = props.Tipo; 
        if(Tipo == TipoVoto.TILE_DANGER)
        {
            estado = !props.peligroso;
        }else
        if (Tipo == TipoVoto.TILE_GELA)
        {
            if (props.poderRebote == 0)
                numf = 1.5f;
            else
                numf = 0;
        }else
        if (Tipo == TipoVoto.TILE_SOLID)
        {
           estado = !props.solido;
        }
    }
}

public enum TipoVoto { GRAV, TILE_DANGER, TILE_SOLID, TILE_GELA, COUNT  }
