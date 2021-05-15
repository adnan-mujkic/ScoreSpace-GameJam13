using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ElementType { FIRE, ICE, ELECTRO, WATER }
public class GameManager: MonoBehaviour
{
   public static GameManager GM;
   public Color[] ElementColors;
   public GameObject MainUi, MenuUi;
   LevelGenerator LG;


   private void Awake() {
      if(GM == null) {
         GM = this;
      } else {
         Destroy(this.gameObject);
      }
      LG = FindObjectOfType<LevelGenerator>();
   }

   public void StartGame() {
      MainUi.SetActive(true);
      MenuUi.SetActive(false);
      LG.GenerateSkeletons();
   }

   public void SelectUpgradeSkeleton(int type){

   }
}
