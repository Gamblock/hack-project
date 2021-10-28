using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetupCardFromCharacterSO : MonoBehaviour
{
    public TextMeshProUGUI damage;
    public TextMeshProUGUI armor;
    public TextMeshProUGUI health;
    public TextMeshProUGUI name;
    public TextMeshProUGUI className;
    public RawImage cardImage;
    public CanvasGroup cardCanvasGroup;

    public void SetUpCard(CharacterInfoSO characterInfoSo)
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
        cardImage.texture = characterInfoSo.cardTexture;
        cardCanvasGroup.alpha = 1;
    }

}
