using UnityEngine;

public class Autodestructor : MonoBehaviour
{
    public float time = 10f;

    void Start()
    {
        if(time > 0f)
            Invoke("DestroySelf", time);
        else
            DestroySelf();
    }

    public void DestroySelf()
    {
        Destroy(this.gameObject);
    }
}