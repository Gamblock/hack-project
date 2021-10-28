using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuCardSetter : MonoBehaviour
{
    public RawImage cardImage;
    public TextureHolderSO textureHolderSo;
    public CanvasGroup cardCanvas;
    private void Start()
    {
        if (textureHolderSo.cardTexture != null)
        {
            cardCanvas.alpha = 1;
            cardImage.texture = textureHolderSo.cardTexture;
        }
    }

    private void OnApplicationQuit()
    {
        textureHolderSo.cardTexture = null;
    }
}
