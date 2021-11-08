using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceUI : MonoBehaviour
{
   public ResourceManager manager;
   public ResourseSO resource;


   public void TryPurchaseResource()
   {
      manager.BuyResource(resource.typeOfResource,resource.resourcePrice);
      Debug.Log("pressed");
   }
}
