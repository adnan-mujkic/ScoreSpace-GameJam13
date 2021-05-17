using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Difficulty { Easy = 1, Normal = 2, Hard = 3, Insane = 4 }
public class LevelGenerator: MonoBehaviour
{
   [Header("Options")]
   public Difficulty difficulty;
   public float LeftEdge, RightEdge, BottomEdge, TopEdge;
   [Header("Prefabs")]
   public EnemyAi SkeletonPrefab;
   public BossAi BossPrefab;
   public GameObject[] GroundPrefabs;

   public int AdditionalSkeleton;
   List<GameObject> Skeletons;
   List<GameObject> Grounds;
   BossAi Boss;
   public ElementType SkeletonElement, BossElement;
   Player player;


   // Start is called before the first frame update
   void Start() {
      Skeletons = new List<GameObject>();
      Grounds = new List<GameObject>();
      player = FindObjectOfType<Player>();
   }

   public void PrepareWave() {
      SkeletonElement = (ElementType)Random.Range(1, 3);
      if(GameManager.GM.Wave == 1 || AdditionalSkeleton == -1) {
         BossElement = GetStrongerElement(SkeletonElement);
      } else {
         BossElement = (ElementType)Random.Range(1, 3);
      }
   }

   public void DeleteBoss() {
      foreach(var item in Skeletons) {
         Destroy(item.gameObject);
      }
      Skeletons.Clear();
      Destroy(Boss.gameObject);
      var allBones = FindObjectsOfType<BoneAi>();
      if(allBones != null) {
         foreach(var item in allBones) {
            Destroy(item.gameObject);
         }
      }
      foreach (var item in Grounds)
      {
         Destroy(item.gameObject);
      }
      Grounds.Clear();
      GameManager.GM.Paused = true;
   }

   public void GenerateSkeletons() {
      SpawnSkeleton();
      if(AdditionalSkeleton != -1) {
         SpawnSkeleton();
      }
      Boss = Instantiate(BossPrefab);
      Boss.transform.position = new Vector3(Random.Range(-6f, 6f), -6f, 0f);
      while(CheckIfInPlayerRange(Boss.transform.position.x)){
         Boss.transform.position = new Vector3(Random.Range(-6f, 6f), -6f, 0f);
      }
      Boss.ChangeElement(BossElement);
   }
   public void SpawnSkeleton() {
      var skele = Instantiate(SkeletonPrefab);
      float xPos = Random.Range(LeftEdge, RightEdge);
      float yPos = Random.Range(BottomEdge, TopEdge);
      if(Skeletons.Count > 0) {
         while(InRangeOfAnotherSkele(xPos, yPos)) {
            xPos = Random.Range(LeftEdge, RightEdge);
            yPos = Random.Range(BottomEdge, TopEdge);
         }
      }
      skele.transform.position = new Vector3(xPos, yPos, 0f);
      if(Skeletons.Count == 0)
         skele.gameObject.GetComponentInChildren<SkeletonAi>().ChangeElement(SkeletonElement);
      else
         skele.gameObject.GetComponentInChildren<SkeletonAi>().ChangeElement((ElementType)AdditionalSkeleton);
      Skeletons.Add(skele.gameObject);
      var ground = Instantiate(GroundPrefabs[(Random.value > 0.5f) ? 0 : 1]);
      ground.transform.position = skele.transform.position - new Vector3(0, 0.5f, 0);
      Grounds.Add(ground.gameObject);
   }
   public ElementType GetWeakerElement(ElementType type) {
      ElementType OppositeType;
      switch(type) {
         case ElementType.FIRE:
            OppositeType = ElementType.WATER;
            break;
         case ElementType.WATER:
            OppositeType = (Random.value > 0.5f) ? ElementType.ICE : ElementType.ELECTRO;
            break;
         case ElementType.ICE:
            OppositeType = ElementType.FIRE;
            break;
         case ElementType.ELECTRO:
            OppositeType = (Random.value > 0.5f) ? ElementType.FIRE : ElementType.ICE;
            break;
         default:
            OppositeType = ElementType.FIRE;
            break;
      }
      return OppositeType;
   }
   public ElementType GetStrongerElement(ElementType type) {
      ElementType OppositeType;
      switch(type) {
         case ElementType.FIRE:
            OppositeType = (Random.value > 0.5f) ? ElementType.ICE : ElementType.ELECTRO;
            break;
         case ElementType.WATER:
            OppositeType = ElementType.FIRE;
            break;
         case ElementType.ICE:
            OppositeType = (Random.value > 0.5f) ? ElementType.WATER : ElementType.ELECTRO;
            break;
         case ElementType.ELECTRO:
           OppositeType = ElementType.WATER;
            break;
         default:
            OppositeType = ElementType.FIRE;
            break;
      }
      return OppositeType;
   }

   public bool InRangeOfAnotherSkele(float x, float y) {
      for(int i = 0; i < Skeletons.Count; i++) {
         if(Skeletons[i].transform.position.x - 1f < x && Skeletons[i].transform.position.x + 1f > x) {
            if(Skeletons[i].transform.position.y - 1f < y && Skeletons[i].transform.position.y + 1f > y) {
               return true;
            }
         }
      }
      return false;
   }
   public bool CheckIfInPlayerRange(float x){
      return (x - 1f < player.transform.position.x && x + 1f > player.transform.position.x);
   }
}
