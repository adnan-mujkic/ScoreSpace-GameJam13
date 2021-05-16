using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSoundEffect: MonoBehaviour
{
   public AudioClip SingleClip;
   public List<AudioClip> ListOfClips;
   private AudioManager Manager;

   private void OnEnable() {
      Manager = FindObjectOfType<AudioManager>();
   }
   public void PlaySFX() {
      if(Manager == null)
         Manager = FindObjectOfType<AudioManager>();
      if(SingleClip != null) {
         Manager.PlaySFX(SingleClip);
      }else if(ListOfClips != null && ListOfClips.Count > 0) {
         System.Random rn = new System.Random();
         int indexOfSound = rn.Next(0, ListOfClips.Count);
         Manager.PlaySFX(ListOfClips[indexOfSound]);
      }
   }
}
