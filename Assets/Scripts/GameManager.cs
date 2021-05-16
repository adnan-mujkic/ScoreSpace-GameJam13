using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public enum ElementType { FIRE, ICE, ELECTRO, WATER }
public class GameManager: MonoBehaviour
{
   public DamageNumbersSO DamageNumbers;
   public static GameManager GM;
   public Color[] ElementColors;
   public GameObject MainUi, MenuUi, RulesPageUi, UpgradesUi, HighScoreScreen;
   public TMPro.TextMeshProUGUI ScoreText;
   public Button PlayButton;
   LevelGenerator LG;
   WaveLoadingWrapper WLW;
   public int Wave;
   public int Points;
   public bool Paused;
   public TMPro.TextMeshProUGUI HighscoreText;


   private void Awake() {
      if(GM == null) {
         GM = this;
      } else {
         Destroy(this.gameObject);
      }
      LG = FindObjectOfType<LevelGenerator>();
      OptionsManager.OnGameLoaded += GameLoaded;
      Paused = true;
      Wave = 1;
   }
   public void GameLoaded(){
      if(OptionsManager.SaveFile.FirstTime){
         Debug.Log("Loaded");
         PlayButton.interactable = false;
      }
   }
   public void OpenRules(){
      RulesPageUi.SetActive(true);
   }
   public void ReadRules(){
      PlayButton.interactable = false;
      RulesPageUi.SetActive(false);
      OptionsManager.SaveFile.FirstTime = false;
      OptionsManager.SaveGame();
   }

   public void PlayGame(int skeletonIndex){
      LG.AdditionalSkeleton = skeletonIndex;
      FindObjectOfType<UpgradePickerWrapper>(true).gameObject.SetActive(false);
      MenuUi.SetActive(false);
      WLW = FindObjectOfType<WaveLoadingWrapper>(true);
      WLW.gameObject.SetActive(true);
      WLW.WaveText.text = "Wave: " + Wave.ToString();
      WLW.DisplayWave();
      Paused = false;
   }
   public void StartGame() {
      MainUi.SetActive(true);
      LG.PrepareWave();
      LG.GenerateSkeletons();
      UpdatePointsText();
   }

   public void AdvanceToNextStage(){
      MainUi.SetActive(false);
      UpgradesUi.SetActive(true);
      LG.PrepareWave();
      Wave++;
      UpgradesUi.GetComponent<UpgradePickerWrapper>().ShowUpgrades();
   }
   public int GetDamageNumberForElement(ElementType Atacker, ElementType Reciever){
      return DamageNumbers.DamageArray[(int)Atacker].DamageNumbers[(int)Reciever];
   }

   public void UpdatePointsText(){
      ScoreText.text = Points.ToString();
   }

   public void GameOver(){
      if(OptionsManager.SaveFile.Wave < Wave){
         OptionsManager.SaveFile.Wave = Wave;
         OptionsManager.SaveGame();
      }
      HighScoreScreen.SetActive(true);
      HighscoreText.text = OptionsManager.SaveFile.Wave.ToString();
   }

   public void PlayAgain(){
      UnityEngine.SceneManagement.SceneManager.LoadScene(0);
   }

}