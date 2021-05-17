using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAi: MonoBehaviour
{
   public ElementType Type;
   public Player player;
   public bool facingRight;
   public Canvas hpCanvas;
   public int HP;
   public HpBarWrapper HpBar;

   private void OnEnable() {
      player = FindObjectOfType<Player>();
      StartCoroutine
      (PeriodicallyCheckAi());
      FacePlayer();
      hpCanvas.worldCamera = Camera.main;
      hpCanvas.pixelPerfect = true;
      HP = 5;
   }

   private IEnumerator PeriodicallyCheckAi() {
      yield return new WaitForSeconds(2f);
      while (true)
      {
         FacePlayer();
         Vector3 playerPos = transform.position;
         playerPos.x = player.transform.position.x;
         Vector3 startingPos = transform.position;
         float seconds = 0f;
         while (seconds < 1f)
         {
            transform.position = Vector3.Lerp(startingPos, playerPos, seconds);
            seconds += Time.deltaTime;
            yield return new WaitForEndOfFrame();
         }
         float waitFrequency = Random.Range(4f, 6f) - (GameManager.GM.Wave < 30 ? (float)GameManager.GM.Wave / 10f : 3f);
         yield return new WaitForSeconds(waitFrequency);
      }
   }
   public void DecreaseHp(ElementType element){
      AudioManager.AM.PlaySoundEffect(SfxType.BOSS_DAMAGE);
      int amount = GameManager.GM.GetDamageNumberForElement(element, Type);
      GameManager.GM.Points += amount * 10;
      GameManager.GM.UpdatePointsText();
      ChangeElement(Type);
      StartCoroutine(FlashOnDamage(Color.red));
      if(HP <= 0)
         return;
      HP -= amount;
      HpBar.UpdateHp(HP);
      if(HP <= 0){
         HP = 0;
         HpBar.UpdateHp(0);
         Die();
      }
   }
   public void Die(){
      Debug.Log("Stage Complete");
      GameManager.GM.Points += 100;
      GameManager.GM.UpdatePointsText();
      FindObjectOfType<LevelGenerator>().DeleteBoss();
      GameManager.GM.AdvanceToNextStage();
   }

      private IEnumerator FlashOnDamage(Color colorToFlash) {
      colorToFlash.r *= 0.7f;
      colorToFlash.g *= 0.7f;
      colorToFlash.b *= 0.7f;
      GetComponent<SpriteRenderer>().color = colorToFlash;
      yield return new WaitForSeconds(0.2f);
      ChangeElement(Type);
   }

   public void ChangeElement(ElementType type){
      Type = type;
      GetComponent<SpriteRenderer>().color = GameManager.GM.ElementColors[(int)type];
   }
   private void FacePlayer() {
      if(player.transform.position.x > transform.position.x && !facingRight) {
         Flip();
      } else if(player.transform.position.x < transform.position.x && facingRight) {
         Flip();
      }
   }
   private void Flip() {
      facingRight = !facingRight;
      Vector3 scaleNew = transform.localScale;
      scaleNew.x *= -1;
      transform.localScale = scaleNew;
   }
}
