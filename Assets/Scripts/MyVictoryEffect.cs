using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyVictoryEffect : MonoBehaviour
{
    [SerializeField] private GameObject mainCameraObject;
    [SerializeField] private SoundManager soundManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        mainCameraObject = GameObject.Find("MainCamera");
        soundManager = mainCameraObject.GetComponent<SoundManager>();
        soundManager.PlayClip(0);
    }
}
