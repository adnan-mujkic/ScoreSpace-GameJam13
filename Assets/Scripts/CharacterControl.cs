using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl: MonoBehaviour
{
   public float speed;
   public float jumpForce;
   public Transform groundCheck;
   public float checkRadius;
   public LayerMask whatIsGround;
   Animator animator;
   Rigidbody2D rb2d;
   SpriteRenderer spriteRenderer;

   float moveInput;
   public bool facingRight;
   bool moving;
   bool isGrounded;
   public static int maxJumps = 1;
   public int jumps = 1;
   public static bool SprintEnabled;
   public bool Sprinting;

   // Start is called before the first frame update
   void Start() {
      animator = GetComponent<Animator>();
      rb2d = GetComponent<Rigidbody2D>();
      spriteRenderer = GetComponent<SpriteRenderer>();
      facingRight = true;
      //SprintEnabled = true;
      //maxJumps = 2;
   }
   // Update is called once per frame
   void FixedUpdate() {
      if(GameManager.GM.Paused)
         return;
      isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
      moveInput = Input.GetAxisRaw("Horizontal");
      if(SprintEnabled && Sprinting) {
         rb2d.velocity = new Vector2(moveInput * speed * 3f, rb2d.velocity.y);
      } else {
         rb2d.velocity = new Vector2(moveInput * speed, rb2d.velocity.y);
      }


      if(!facingRight && moveInput > 0) {
         Flip();
      } else if(facingRight && moveInput < 0) {
         Flip();
      }
   }
   void Update() {
      if(GameManager.GM.Paused)
         return;
      if(isGrounded) {
         jumps = maxJumps;
      }
      if(rb2d.velocity.y != 0) {
         animator.SetBool("Jumping", true);
      } else {
         animator.SetBool("Jumping", false);
      }
      if(Input.GetKeyDown(KeyCode.Space) && jumps > 0) {
         rb2d.velocity = Vector2.up * jumpForce;
         jumps--;
      } else if(Input.GetKeyDown(KeyCode.Space) && jumps == 0 && isGrounded) {
         rb2d.velocity = Vector2.up * jumpForce;
      }
      Sprinting = Input.GetKey(KeyCode.LeftShift);
      if(rb2d.velocity.x != 0) {
         moving = true;
      } else {
         moving = false;
      }
      animator.SetBool("Walking", moving);
   }
   void Flip() {
      facingRight = !facingRight;
      Vector3 Scaler = transform.localScale;
      Scaler.x *= -1;
      transform.localScale = Scaler;
   }
}
