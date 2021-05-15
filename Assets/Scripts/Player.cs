using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
   public static int Lives;
   public static int HP;
   public GameObject SucessScreen;
   public bool Shielding;
   CharacterControl characterControl;

   public HpBarWrapper HpBar;
   public static int Score;
   public static int Keys;
   public TMPro.TextMeshProUGUI ScoreText;
   public GameObject GameOverScreen;
   public Button ReplayButton;



   private void Start()
   {
      HpBar = FindObjectOfType<HpBarWrapper>();
      characterControl = GetComponent<CharacterControl>();
      if (HP == 0)
      {
         HP = 8;
         HpBar.UpdateHp(HP);
      }
   }

   // Update is called once per frame
   void Update()
   {

   }

   private void OnTriggerEnter2D(Collider2D other)
   {
      if (other.tag == "Enemy")
      {
         if ((Shielding && characterControl.facingRight && other.transform.position.x > transform.position.x) ||
         (Shielding && !characterControl.facingRight && other.transform.position.x < transform.position.x))
         {
            if (other.GetComponent<BoneAi>() != null)
            {
               if (other.GetComponent<BoneAi>().SkeletonFacingRight)
                  other.GetComponent<Rigidbody2D>().velocity = new Vector2(-25, 0);
               else
                  other.GetComponent<Rigidbody2D>().velocity = new Vector2(25, 0);
               other.GetComponent<BoneAi>().SkeletonOwner = false;
            }
         }
         else
         {
            HP--;
            HpBar.UpdateHp(HP);
            Destroy(other.gameObject);
            if (HP <= 0)
            {
               Die(false);
            }
         }
      }
      if (other.tag == "DeathZone")
      {
         Die(true);
      }
   }

   public void Die(bool falling)
   {
      GetComponent<CharacterControl>().enabled = false;
      if (falling)
      {
         StartCoroutine(WaitAndDisablePhysics());
      }
      else
      {
         if (!UpdateLives())
            SceneManager.LoadScene(1);
      }

   }
   IEnumerator WaitAndDisablePhysics()
   {
      yield return new WaitForSeconds(0.5f);
      GetComponent<Rigidbody2D>().simulated = false;
      if (!UpdateLives())
            SceneManager.LoadScene(1);
   }
   public void AddScore(int score)
   {
      Score += score;
      ScoreText.text = "Score: " + Score.ToString();
   }
   public void ReloadGame()
   {
      SceneManager.LoadScene(0);
   }
   public bool UpdateLives()
   {
      Lives--;
      if (Lives == 0)
      {
         GameOverScreen.SetActive(true);
         ReplayButton.Select();
         EventSystem.current.SetSelectedGameObject(null);
         EventSystem.current.SetSelectedGameObject(ReplayButton.gameObject);
         return true;
      }
      return false;
   }

   public void AddKey(){
      Keys++;
      if(Keys == 2){
         SucessScreen.SetActive(true);
         GetComponent<BoxCollider2D>().enabled = false;
         GetComponent<Rigidbody2D>().simulated = false;
      }
   }


}

public struct DialogueMap{
   public int index;
   public bool enabled;
}