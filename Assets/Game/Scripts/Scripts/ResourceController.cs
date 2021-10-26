using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourceController : MonoBehaviour
{
    public TextMeshProUGUI waterText;
    public TextMeshProUGUI fireText;
    public TextMeshProUGUI earthText;
    public Storage storage;

    public void UpdateEarthValue(int ammountToAdd)
    {
        earthText.text =  storage.UpdateResourceValue(TypeEnums.ResourceTypes.earth, ammountToAdd).ToString(); 
    }
    public void UpdateWaterValue(int ammountToAdd)
    {
        waterText.text =  storage.UpdateResourceValue(TypeEnums.ResourceTypes.water, ammountToAdd).ToString();
    }
    public void UpdateFireValue(int ammountToAdd)
    {
        fireText.text =  storage.UpdateResourceValue(TypeEnums.ResourceTypes.fire, ammountToAdd).ToString();
    }
    public void UpdateResourceValues(TypeEnums.ResourceTypes type,int amount)
    {
        if (type == TypeEnums.ResourceTypes.earth)
        {
            UpdateEarthValue(amount);
        }
        if (type == TypeEnums.ResourceTypes.fire)
        {
            UpdateFireValue(amount);
        }
        if (type == TypeEnums.ResourceTypes.water)
        {
            UpdateWaterValue(amount);
        }
        
        
    }
    
    public void ShowResourceValues()
    {
      UpdateEarthValue(0);
      UpdateFireValue(0);
      UpdateWaterValue(0);
    }

}
