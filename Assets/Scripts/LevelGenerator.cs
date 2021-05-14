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
   public GameObject[] GroundPrefabs;

   List<GameObject> Skeletons;
   List<GameObject> Grounds;


   // Start is called before the first frame update
   void Start() {
      Skeletons = new List<GameObject>();
      Grounds = new List<GameObject>();
      GenerateSkeletons();
   }

   public void GenerateSkeletons() {
      int numberOfSkeletons = (int)difficulty;
      for(int i = 0; i < numberOfSkeletons; i++) {
         var skele = Instantiate(SkeletonPrefab);
         float xPos = Random.Range(LeftEdge, RightEdge);
         float yPos = Random.Range(BottomEdge, TopEdge);
         if(i != 0) {
            while(InRangeOfAnotherSkele(xPos, yPos)) {
               xPos = Random.Range(LeftEdge, RightEdge);
               yPos = Random.Range(BottomEdge, TopEdge);
            }
         }
         skele.transform.position = new Vector3(xPos, yPos, 0f);
         Skeletons.Add(skele.gameObject);
         var ground = Instantiate(GroundPrefabs[0]);
         ground.transform.position = skele.transform.position - new Vector3(0, 0.5f, 0);
         Grounds.Add(ground.gameObject);
      }
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
