using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
   public ShopViewModel shopViewModel;
   public CanvasGroup itemCanvasGroup;
   public int itemPrice;
   public DagableItem item;
   public TextMeshProUGUI priceText;
   
   private void Start()
   {
      priceText.text = itemPrice.ToString();
   }

   private void OnEnable()
   {
      item.OnItemDropped += PurchaseOrSellItem;
      shopViewModel.avaliableTokenAmount.OnValueChanged += CheckIfItemCanBePurchased;
      CheckIfItemCanBePurchased(shopViewModel.avaliableTokenAmount.Value);
   }

   private void OnDisable()
   {
      item.OnItemDropped -= PurchaseOrSellItem;
      shopViewModel.avaliableTokenAmount.OnValueChanged -= CheckIfItemCanBePurchased;
   }

   public void PurchaseOrSellItem(bool itemIsPurchsed)
   {
      if (itemIsPurchsed)
      {
         shopViewModel.ChangeTokenAmount(- itemPrice);
      }
      else
      {
         shopViewModel.ChangeTokenAmount(itemPrice);
      }
   }
   public void CheckIfItemCanBePurchased(int avaliableTokens)
   {
      if (itemPrice > avaliableTokens && !item.inPlace)
      {
         itemCanvasGroup.alpha = 0.5f;
         itemCanvasGroup.blocksRaycasts = false;
      }
      else
      {
         itemCanvasGroup.alpha = 1;
         itemCanvasGroup.blocksRaycasts = true;
      }
   }
}
