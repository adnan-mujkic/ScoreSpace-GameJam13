using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UpgradePickerWrapper: MonoBehaviour
{
   public TMPro.TextMeshProUGUI PointsText;
   public Button[] SkeletonButtons;
   public Button HpButton, ShieldButton, ShieldUpgradeButton, ShuffleButton;
   public Image SkeletonEnemy;
   public Image BossEnemy;

   public int SkeletonIndexSelected;
   LevelGenerator LG;

   private void Awake() {
      SkeletonIndexSelected = -1;
      LG = FindObjectOfType<LevelGenerator>();
      ShowUpgrades();
   }

   public void ShowUpgrades() {
      foreach(var item in SkeletonButtons) {
         item.interactable = true;
         item.GetComponent<SkeletonUpgradeButton>().SelectSkeletonType(false);
      }
      SkeletonIndexSelected = -1;
      FindObjectOfType<EventSystem>().SetSelectedGameObject(null);
      PointsText.text = GameManager.GM.Points.ToString();
      HpButton.interactable = true;
      ShieldButton.interactable = true;
      DisplaySkeletonBoss();
   }
   public void BuyHp() {
      if(Player.HP == 10 || GameManager.GM.Points <= 20)
         return;
      GameManager.GM.Points -= 20;
      PointsText.text = GameManager.GM.Points.ToString();
      HpButton.interactable = false;
      DisableInteract();
   }

   public void RefillShield() {
      if(Player.Shield || GameManager.GM.Points <= 200)
         return;
      GameManager.GM.Points -= 200;
      PointsText.text = GameManager.GM.Points.ToString();
      ShieldButton.interactable = false;
      DisableInteract();
   }

   public void UpgradeShield() {
      if(GameManager.GM.Points <= 1000)
         return;
      Player.ShieldHP++;
      GameManager.GM.Points -= 1000;
      PointsText.text = GameManager.GM.Points.ToString();
      DisableInteract();
   }

   public void ReshuffleSkeletons(){
      if(GameManager.GM.Points <= 200)
         return;
      GameManager.GM.Points -= 200;
      PointsText.text = GameManager.GM.Points.ToString();
      LG.PrepareWave();
      DisplaySkeletonBoss();
   }

   public void SelectSkeleton(int type) {
      if(GameManager.GM.Points <= 100)
         return;
      if(SkeletonIndexSelected == -1) {
         SkeletonIndexSelected = type;
         foreach(var item in SkeletonButtons) {
            item.interactable = false;
            item.GetComponent<SkeletonUpgradeButton>().SelectSkeletonType(false);
         }
         SkeletonButtons[type].interactable = true;
         SkeletonButtons[type].GetComponent<SkeletonUpgradeButton>().SelectSkeletonType(true);
         GameManager.GM.Points -= 100;
         PointsText.text = GameManager.GM.Points.ToString();
      } else {
         foreach(var item in SkeletonButtons) {
            item.interactable = true;
            item.GetComponent<SkeletonUpgradeButton>().SelectSkeletonType(false);
         }
         SkeletonIndexSelected = -1;
         FindObjectOfType<EventSystem>().SetSelectedGameObject(null);
         GameManager.GM.Points += 100;
         PointsText.text = GameManager.GM.Points.ToString();
      }
   }

   public void ApplyUpgrade() {
      GameManager.GM.PlayGame(SkeletonIndexSelected);
   }

   private void DisplaySkeletonBoss(){
      SkeletonEnemy.color = GameManager.GM.ElementColors[(int)LG.SkeletonElement];
      BossEnemy.color = GameManager.GM.ElementColors[(int)LG.BossElement];
   }

   public void DisableInteract() {
      if(GameManager.GM.Points <= 100)
         ShieldUpgradeButton.interactable = false;
      if(GameManager.GM.Points <= 200){
         ShieldButton.interactable = false;
         ShuffleButton.interactable = false;
      }
      if(GameManager.GM.Points <= 20)
         HpButton.interactable = false;
   }
}
