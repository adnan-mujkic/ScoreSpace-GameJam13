using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneAi: MonoBehaviour
{
   public ElementType element;
   public Vector3 posInCamera;
   public bool SkeletonFacingRight;
   public bool SkeletonOwner;
   public int CollisionCount;
   BossAi Boss;

   private void OnEnable() {
      SkeletonOwner = true;
      CollisionCount = 0;
      GetComponent<SpriteRenderer>().color = GameManager.GM.ElementColors[(int)element];
      Boss = FindObjectOfType<BossAi>();
   }
   private void OnCollisionEnter2D(Collision2D other) {
      if(other.transform.tag == "Collisions") {
         CollisionCount++;
         if(CollisionCount > 10)
            Destroy(this.gameObject);
      }
   }
   private void OnTriggerEnter2D(Collider2D other) {
      if(other.gameObject != null && other.transform.tag == "Enemy"){
         Debug.Log("Boss takes damage");
         Destroy(this.gameObject);
         var boss = other.gameObject.GetComponent<BossAi>();
         if(boss != null){
            boss.DecreaseHp(element);
         }
      }
   }
}
