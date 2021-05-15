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

   private void OnEnable() {
      SkeletonOwner = true;
      CollisionCount = 0;
      GetComponent<SpriteRenderer>().color = GameManager.GM.ElementColors[(int)element];
   }
   private void OnCollisionEnter2D(Collision2D other) {
      if(other.transform.tag == "Collisions") {
         CollisionCount++;
         if(CollisionCount > 10)
            Destroy(this.gameObject);
      }
   }
}
