using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAi : MonoBehaviour
{
   public GameObject BonePrefab;
   public Transform ProjectileTransform;
   public int Level;
   public int HP;
   public bool canThrow;


   float ThrowFrequency;
   Player player;
   bool facingRight;
   private void OnEnable()
   {
      player = FindObjectOfType<Player>();
      ThrowFrequency = 0.3f * Level;
      StartCoroutine(ThrowBones());
      HP = 1 * Level;
      StartCoroutine(CheckIfInsideCamera());
   }


   IEnumerator CheckIfInsideCamera()
   {
      while (true)
      {
         if (player.transform.position.x > transform.position.x && !facingRight)
         {
            Flip();
         }
         else if (player.transform.position.x < transform.position.x && facingRight)
         {
            Flip();
         }
         yield return new WaitForSeconds(0.1f);
      }
   }

   IEnumerator ThrowBones()
   {
      while (true)
      {
         if (canThrow)
         {
            var bone = Instantiate(BonePrefab);
            bone.transform.position = ProjectileTransform.position;
            bone.GetComponent<BoneAi>().SkeletonFacingRight = facingRight;
            if (facingRight)
            {
               bone.GetComponent<Rigidbody2D>().velocity = new Vector2(10, 0);
            }
            else
            {
               bone.GetComponent<Rigidbody2D>().velocity = new Vector2(-10, 0);
            }
         }
         yield return new WaitForSeconds(1f / ThrowFrequency);
      }
   }

   private void OnTriggerEnter2D(Collider2D other)
   {
      if (other.tag == "Enemy" && !other.GetComponent<BoneAi>().SkeletonOwner)
      {
         HP--;
         Destroy(other.gameObject);
         if (HP <= 0)
         {
            player.AddScore(10 * Level);
            Destroy(gameObject);
         }
      }
   }

   private void Flip()
   {
      facingRight = !facingRight;
      Vector3 scaleNew = transform.localScale;
      scaleNew.x *= -1;
      transform.localScale = scaleNew;
   }
}
