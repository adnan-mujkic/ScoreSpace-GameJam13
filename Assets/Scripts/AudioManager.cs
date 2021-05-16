using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SfxType { BONE_THROW, PLAYER_DAMAGE, BONE_BREAK, BOSS_DAMAGE, LEVEL_COMPLETE, WAVE_START, GAME_OVER }
public class AudioManager: MonoBehaviour
{
   public static AudioManager AM;
   public AudioSource SfxSource;
   public AudioSource MusicSource;

   public AudioClip[] MusicToPlay;
   [Header("SFX List")]
   public AudioClip BoneThrowSound;
   public AudioClip PlayerDamageTakeSound;
   public AudioClip BoneBreakSound;
   public AudioClip BossTakeDamageSound;
   public AudioClip LevelCompleteSound;
   public AudioClip WaveStartSound;
   public AudioClip GameOverSound;

   private void Awake() {
      if(AM == null) {
         AM = this;
      } else {
         Destroy(this.gameObject);
      }
   }

   public void PlayMusic() {
      System.Random ran = new System.Random();
      MusicSource.clip = MusicToPlay[ran.Next(0, MusicToPlay.Length)];
      StopAllCoroutines();
   }
   public void PlaySoundEffect(SfxType sfx) {
      switch(sfx) {
         case SfxType.BONE_THROW:
            PlaySFX(BoneThrowSound);
            break;
         case SfxType.PLAYER_DAMAGE:
            PlaySFX(PlayerDamageTakeSound);
            break;
         case SfxType.BONE_BREAK:
            PlaySFX(BoneBreakSound);
            break;
         case SfxType.BOSS_DAMAGE:
            PlaySFX(BossTakeDamageSound);
            break;
         case SfxType.LEVEL_COMPLETE:
            PlaySFX(LevelCompleteSound);
            break;
         case SfxType.WAVE_START:
            PlaySFX(WaveStartSound);
            break;
         case SfxType.GAME_OVER:
            PlaySFX(GameOverSound);
            break;
         default:
            break;
      }
   }
   public void PlaySFX(AudioClip clip) {
      SfxSource.PlayOneShot(clip);
   }
}