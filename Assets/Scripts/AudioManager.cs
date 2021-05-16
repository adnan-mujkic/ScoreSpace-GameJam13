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
   public float TopMusicVolume;

   private void Awake() {
      if(AM == null) {
         AM = this;
      } else {
         Destroy(this.gameObject);
      }
      PlayMusic();
   }

   public void PlayMusic() {
      MusicSource.volume = 0f;
      System.Random ran = new System.Random();
      MusicSource.clip = MusicToPlay[ran.Next(0, MusicToPlay.Length)];
      MusicSource.Play();
      StopAllCoroutines();
      StartCoroutine(SmoothOutMusic());
   }
   public void LowerVolume() {
      StartCoroutine(VolumeAnimation(0.1f, 0.02f));
   }
   public void HighVolume() {
      StartCoroutine(VolumeAnimation(0.05f, 0.1f));
   }
   private IEnumerator SmoothOutMusic() {
      float seconds = 0f;
      MusicSource.volume = 0f;
      while(seconds < 2f) {
         seconds += Time.deltaTime;
         MusicSource.volume = Mathf.Lerp(0f, TopMusicVolume, seconds / 2f);
         yield return new WaitForEndOfFrame();
      }
      MusicSource.volume = TopMusicVolume;
   }
   private IEnumerator VolumeAnimation(float from, float to) {
      float seconds = 0f;
      MusicSource.volume = 0f;
      MusicSource.Play();
      while(seconds < 2f) {
         seconds += Time.deltaTime;
         MusicSource.volume = Mathf.Lerp(from, to, seconds / 2f);
         yield return new WaitForEndOfFrame();
      }
      MusicSource.volume = to;
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
