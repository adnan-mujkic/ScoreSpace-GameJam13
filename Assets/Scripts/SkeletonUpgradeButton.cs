using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkeletonUpgradeButton : MonoBehaviour
{
   public Image SkeletonSprite;
   public TMPro.TextMeshProUGUI Cost;
   public Sprite NormalSprite, SelectedSprite;


   public void SelectSkeletonType(bool select){
      if(!select){
         GetComponent<Image>().sprite = NormalSprite;
      }
      else{
         GetComponent<Image>().sprite = SelectedSprite;
      }
   }
}
