using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadoutSpawner : MonoBehaviour
{
    
    public List<GameObject> healerLoadOuts;
    public List<GameObject> tankLoadOuts;
    public List<GameObject> damageDealerLoadouts;
    public Transform backAttachment;


    public void SpawnLoadOut(TypeEnums.ClassTypes classType)
    {
        if (classType == TypeEnums.ClassTypes.Healer)
        {
            Instantiate(healerLoadOuts[Random.Range(0,healerLoadOuts.Count - 1)], backAttachment);
        }
        if (classType == TypeEnums.ClassTypes.Tank)
        {
            Instantiate(tankLoadOuts[Random.Range(0,tankLoadOuts.Count - 1)], backAttachment);
        }
        if (classType == TypeEnums.ClassTypes.DamageDealer)
        {
            Instantiate(damageDealerLoadouts[Random.Range(0,damageDealerLoadouts.Count - 1)], backAttachment);
        }
    }

}
