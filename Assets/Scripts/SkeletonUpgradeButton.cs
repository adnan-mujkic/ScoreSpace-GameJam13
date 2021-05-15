using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkeletonUpgradeButton : MonoBehaviour
{
   public Image SkeletonSprite;
   public TMPro.TextMeshProUGUI Cost;
   public Sprite NormalSprite, SelectedSprite;
   public bool selected;


   public void SelectSkeletonType(int type){
      if(selected){
         GetComponent<Image>().sprite = NormalSprite;
         selected = false;
      }
      else{
         GetComponent<Image>().sprite = SelectedSprite;
         selected = true;
      }
   }
}
