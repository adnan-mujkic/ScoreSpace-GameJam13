using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAi: MonoBehaviour
{
   public ElementType Type;
   public GameObject BonePrefab;
   public Transform ProjectileTransform;
   public int Level;
   public int HP;
   public bool canThrow;
   public bool firing;
   public LineRenderer LR;

   float ThrowFrequency;
   Player player;
   bool facingRight;
   private void OnEnable() {
      player = FindObjectOfType<Player>();
      ThrowFrequency = Random.Range(4f, 5f) - (GameManager.GM.Wave < 30 ? (float)GameManager.GM.Wave / 10f : 3f );
      StartCoroutine(ThrowBones());
      HP = 1 * Level;
      StartCoroutine(CheckIfInsideCamera());
   }


   IEnumerator CheckIfInsideCamera() {
      while(true) {
         if(!firing) {
            if(player.transform.position.x > transform.position.x && !facingRight) {
               Flip();
            } else if(player.transform.position.x < transform.position.x && facingRight) {
               Flip();
            }
         }

         yield return new WaitForSeconds(0.1f);
      }
   }

   IEnumerator ThrowBones() {
      yield return new WaitForSeconds(1f);
      LR.SetPosition(0, transform.position);
      while(true) {
         if(!firing) {
            LR.gameObject.SetActive(true);
            firing = true;
            Vector2 CastOffset = new Vector2(facingRight ? 1 : -1, 0);
            CastOffset.y = Random.Range(-2f, 3f);
            while(CastOffset.y > -0.05f && CastOffset.y < 0.05f) { CastOffset.y = Random.Range(-2f, 3f); }
            RaycastHit2D hit = Physics2D.Raycast(transform.position, CastOffset, 100f);
            if(hit.collider != null) {
               LR.SetPosition(1, hit.point);
               yield return new WaitForSeconds(2f);
               LR.gameObject.SetActive(false);
               firing = false;
               var bone = Instantiate(BonePrefab);
               bone.GetComponent<BoneAi>().element = Type;
               bone.GetComponent<BoneAi>().ChangeElement();
               bone.transform.position = ProjectileTransform.transform.position;
               Vector2 Destination = hit.point - new Vector2(bone.transform.position.x, bone.transform.position.y);
               bone.GetComponent<Rigidbody2D>().AddForce(Destination.normalized * 600f);
            }
         }
         yield return new WaitForSeconds(ThrowFrequency);
      }
   }

   private void OnTriggerEnter2D(Collider2D other) {
      if(other.tag == "Enemy" && !other.GetComponent<BoneAi>().SkeletonOwner) {
         HP--;
         Destroy(other.gameObject);
         if(HP <= 0) {
            player.AddScore(10 * Level);
            Destroy(gameObject);
         }
      }
   }

   public void ChangeElement(ElementType type) {
      Type = type;
      GetComponent<SpriteRenderer>().color = GameManager.GM.ElementColors[(int)type];
   }
   private void Flip() {
      facingRight = !facingRight;
      Vector3 scaleNew = transform.localScale;
      scaleNew.x *= -1;
      transform.localScale = scaleNew;
   }
}
