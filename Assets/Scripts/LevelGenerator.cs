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


   // Start is called before the first frame update
   void Start() {
      Skeletons = new List<GameObject>();
      Grounds = new List<GameObject>();
   }

   public void PrepareWave() {
      SkeletonElement = (ElementType)Random.Range(1, 3);
      BossElement = (ElementType)Random.Range(1, 3);
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
      GameManager.GM.Paused = true;
   }

   public void GenerateSkeletons() {
      SpawnSkeleton();
      if(AdditionalSkeleton != -1){
         SpawnSkeleton();
      }
      Boss = Instantiate(BossPrefab);
      Boss.transform.position = new Vector3(Random.Range(-6f, 6f), -6f, 0f);
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
      skele.gameObject.GetComponentInChildren<SkeletonAi>().ChangeElement(SkeletonElement);
      Skeletons.Add(skele.gameObject);
      var ground = Instantiate(GroundPrefabs[(Random.value > 0.5f) ? 0 : 1]);
      ground.transform.position = skele.transform.position - new Vector3(0, 0.5f, 0);
      Grounds.Add(ground.gameObject);
   }
   public ElementType GetOppositeElement(ElementType type) {
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

   public bool InRangeOfAnotherSkele(float x, float y) {
      for(int i = 0; i < Skeletons.Count; i++) {
         if(Skeletons[i].transform.position.x - 0.5f < x && Skeletons[i].transform.position.x + 0.5f > x) {
            if(Skeletons[i].transform.position.y - 0.5f < y && Skeletons[i].transform.position.y + 0.5f > y) {
               return true;
            }
         }
      }
      return false;
   }
}
