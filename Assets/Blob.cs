using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Blob : MonoBehaviour
{
    [SerializeField] SpriteRenderer img;
    [SerializeField] TextMeshProUGUI nameLabel;
    [SerializeField] TextMeshProUGUI posLabel;
    public int id;

    public void Inicializar(int _id)
    {
        id = _id;
        if(nameLabel) nameLabel.text = id.ToString("00");
        Color nuevoColor = new Color(Random.Range(50, 255), Random.Range(50, 255), Random.Range(50, 255));
        img.color = nuevoColor;
        img.color = new Color(Random.Range(50, 255), Random.Range(50, 255), Random.Range(50, 255));
    }

    private void Update()
    {
        string x = transform.position.x.ToString();
        string y = transform.position.y.ToString();
        posLabel.text = "(" + x + ", " + ")";
    }
}
