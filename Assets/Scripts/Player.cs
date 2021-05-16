using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Player: MonoBehaviour
{
   public static int Lives;
   public static int HP;
   public static int Shield;
   public static int ShieldHP;
   public static Player Instance;
   public GameObject SucessScreen;
   public bool Shielding;
   CharacterControl characterControl;

   public HpBarWrapper HpBar;
   public TMPro.TextMeshProUGUI ShieldText;
   public static int Score;
   public static int Keys;
   public TMPro.TextMeshProUGUI ScoreText;
   public GameObject GameOverScreen;
   public Button ReplayButton;


   private void Awake() {
      if(Instance == null) {
         Instance = this;
      } else {
         Destroy(this.gameObject);
      }
      ShieldHP = Shield = 1;
      Player.Instance.ShieldText.text = Shield.ToString();
   }

   private void Start() {
      characterControl = GetComponent<CharacterControl>();
      AddScore(0);
      HP = 10;
      HpBar.UpdateHp(HP);
   }

   private void OnTriggerEnter2D(Collider2D other) {
      AudioManager.AM.PlaySoundEffect(SfxType.PLAYER_DAMAGE);
      if(other.transform.tag == "Enemy") {
         if(Shield > 0) {
            Shield--;
            Player.Instance.ShieldText.text = Shield.ToString();
            Destroy(other.transform.parent.gameObject);
         } else {
            HP--;
            HpBar.UpdateHp(HP);
            Destroy(other.transform.parent.gameObject);
            if(HP <= 0) {
               Die(false);
            }
         }
      } else if(other.transform.tag == "Boss") {
         if(Shield > 0) {
            Shield--;
            Player.Instance.ShieldText.text = Shield.ToString();
         } else {
            HP--;
            HpBar.UpdateHp(HP);
            if(HP <= 0) {
               Die(false);
            }
         }
      }
   }
   public static void ShieldRefil() {
      Shield = ShieldHP;
      Player.Instance.ShieldText.text = Shield.ToString();
   }
   public static void ShieldUpgrade() {
      ShieldHP++;
      ShieldRefil();
   }

   public void Die(bool falling) {
      GetComponent<CharacterControl>().enabled = false;
      if(falling) {
         StartCoroutine(WaitAndDisablePhysics());
      }
      GameManager.GM.GameOver();
   }
   IEnumerator WaitAndDisablePhysics() {
      yield return new WaitForSeconds(0.5f);
      GetComponent<Rigidbody2D>().simulated = false;
   }
   public void AddScore(int score) {
      Score += score;
      if(ScoreText != null)
         ScoreText.text = "Score: " + Score.ToString();
   }
   public void ReloadGame() {
      SceneManager.LoadScene(0);
   }

}