using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallGameReset : MonoBehaviour
{

    public GameObject holderObject; // Holding location
    public CustomGameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Reset
    public void PublicResetPosandGame()
    {
        transform.position = holderObject.transform.position;
        gameManager.ResetMemoryGame();
        //PrivateResetPosandGame();
    }

    private IEnumerator PrivateResetPosandGame()
    {
        yield return new WaitForSeconds(1f);  // Wait for 1 second
        transform.position = holderObject.transform.position;
        gameManager.ResetMemoryGame();
    }

}
