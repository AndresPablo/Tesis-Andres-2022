using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;

namespace Demokratos{
    public class LevelManager : MonoBehaviour
    {
        public Transform[] levelsPrefabs;   
        public static Vector3 spawnPos {get; private set;}
        public static int currentLevel {get; private set;}
        Transform levelTransform;
        
        void Start()
        {
        }

        public void LoadNextLevel()
        {
            currentLevel++;
            if(currentLevel >= levelsPrefabs.Length)
            {
                currentLevel = 0; 
            }

            LoadLevel(currentLevel);
        }

        public void LoadLevel(int index)
        {
            // Crea una instancia del nivel prefabricado como hijo de este objeto
            levelTransform = Instantiate(levelsPrefabs[index], transform).transform;
            
            // Destruye el nivel anterior
            if(transform.childCount > 1)
            { 
                Destroy(transform.GetChild(0).gameObject);
            }

            // Toma el punto en el que apareceria el jugador
            EncontrarSpawn();
        }

        // Busca un objeto con la tag "Spawn" dentro del nivel, sino el jugador apareceria en el (0,0)
        void EncontrarSpawn()
        {
            spawnPos = new Vector2(0,0);
            foreach (Transform child in levelTransform)
            {
                if(child.gameObject.tag == "Spawn")
                {
                    spawnPos = child.localPosition;
                }
            }
        }

        public int GetCantidadBaterias()
        {
            int cantidadBaterias = 0;
            foreach (Transform child in levelTransform)
            {
                if(child.GetComponent<Item_Bateria>())
                    cantidadBaterias++;
            }
            return cantidadBaterias-1;
        } 
    }
}