using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAi: MonoBehaviour
{
   public Player player;
   public bool facingRight;

   private void OnEnable() {
      player = FindObjectOfType<Player>();
      StartCoroutine
      (PeriodicallyCheckAi());
      FacePlayer();
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
         yield return new WaitForSeconds(Random.Range(2f, 5f));
      }
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
