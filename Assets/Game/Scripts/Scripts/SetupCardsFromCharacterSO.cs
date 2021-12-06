using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetupCardsFromCharacterSO : MonoBehaviour
{
    public ServerCommunicationManager serverManager;
    public TextMeshProUGUI damage;
    public TextMeshProUGUI armor;
    public TextMeshProUGUI health;
    public TextMeshProUGUI name;
    public TextMeshProUGUI className;
    public RawImage cardImage;
    public CanvasGroup cardCanvasGroup;
    public CharacterInfoSO character;
    public Button characterSelecteButton;
    public CharacterSelectionView viewModel;

    private void Awake()
    {
        SetupCardFromServer();
    }

    public void SetUpCard(CharacterInfoSO characterInfoSo, RawImage image)
    {
        foreach (var stat in characterInfoSo.characterStats)
        {
            if (stat.statToUse == TypeEnums.StatTypes.armor)
            {
                armor.text = stat.StatValue.ToString();
            }
            if (stat.statToUse == TypeEnums.StatTypes.health)
            {
                health.text = stat.StatValue.ToString();
            }
            if (stat.statToUse == TypeEnums.StatTypes.strength)
            {
               damage.text = stat.StatValue.ToString();
            }
        }

        name.text = characterInfoSo.charNAme;
        className.text = characterInfoSo.classType.ToString();
        cardCanvasGroup.alpha = 1;
    }

    public void SetupCardFromServer()
    {
        serverManager.GetAllDataFromServer(cardImage,character);
        foreach (var stat in character.characterStats)
        {
            if (stat.statToUse == TypeEnums.StatTypes.armor)
            {
                armor.text = stat.StatValue.ToString();
            }
            if (stat.statToUse == TypeEnums.StatTypes.health)
            {
                health.text = stat.StatValue.ToString();
            }
            if (stat.statToUse == TypeEnums.StatTypes.strength)
            {
                damage.text = stat.StatValue.ToString();
            }
        }

        name.text = character.charNAme;
        className.text = character.classType.ToString();
        cardCanvasGroup.alpha = 1;
        if (characterSelecteButton != null)
        {
            characterSelecteButton.onClick.AddListener(SetSelected);
        }
        
    }

    public void SetSelected()
    {
        viewModel.SetSelectedCharacter(character, cardImage);
    }
    public void SetupSelectableCardFromServer()
    {
        serverManager.GetAllDataFromServer(cardImage,character);
        foreach (var stat in character.characterStats)
        {
            if (stat.statToUse == TypeEnums.StatTypes.armor)
            {
                armor.text = stat.StatValue.ToString();
            }
            if (stat.statToUse == TypeEnums.StatTypes.health)
            {
                health.text = stat.StatValue.ToString();
            }
            if (stat.statToUse == TypeEnums.StatTypes.strength)
            {
                damage.text = stat.StatValue.ToString();
            }
        }

        name.text = character.charNAme;
        className.text = character.classType.ToString();
        cardCanvasGroup.alpha = 1;
        
    }
    public Texture2D ReadTexture()
    {
        string base64 = PlayerPrefs.GetString("KEY");
        byte[] bytes = System.Convert.FromBase64String(base64);
        Texture2D tex = new Texture2D(400, 540);
        tex.LoadImage(bytes);
        return tex;
    }

}
