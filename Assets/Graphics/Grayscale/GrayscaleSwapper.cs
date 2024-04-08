using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GrayscaleSwapper : MonoBehaviour
{

    [SerializeField]
    private FullScreenPassRendererFeature grayscaleFeature;
    
    public void setGrayscale(bool state) {
        grayscaleFeature.SetActive(state);
    }

    void OnApplicationQuit() {
        grayscaleFeature.SetActive(false);
    }
}
