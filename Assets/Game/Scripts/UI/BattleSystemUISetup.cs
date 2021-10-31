using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleSystemUISetup : MonoBehaviour
{
    public Button healerAttack;
    public Button meleeAttack;
    public Button healAbility;
    public Button tankAbility;
    public Button dpsAbility;
    
    public GameObject shield;
    public GameObject mace;
    public GameObject staff;
    public GameObject longSword;


    public void SetUIButtons(CharacterInfoSO character)
    {
        if (character.classType == TypeEnums.ClassTypes.Healer)
        {
            staff.SetActive(true);
            healAbility.gameObject.SetActive(true);
            healerAttack.gameObject.SetActive(true);
        }
        if (character.classType == TypeEnums.ClassTypes.DamageDealer)
        {
            longSword.SetActive(true);
            dpsAbility.gameObject.SetActive(true);
            meleeAttack.gameObject.SetActive(true);
        }

        if (character.classType == TypeEnums.ClassTypes.Tank)
        {
            mace.SetActive(true);
            shield.SetActive(true);
            tankAbility.gameObject.SetActive(true);
            meleeAttack.gameObject.SetActive(true);
        }
    }
}
