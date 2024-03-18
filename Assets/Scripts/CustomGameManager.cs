using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGameManager : MonoBehaviour
{
    public GameObject memoryGameObject;
    private Vector3 memoryGamePosition;

    // Start is called before the first frame update
    void Start()
    {
        memoryGamePosition = new Vector3(0, 1.5f, 8);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetMemoryGame()
    {
        Instantiate(memoryGameObject, memoryGamePosition, Quaternion.identity);
    }
}
