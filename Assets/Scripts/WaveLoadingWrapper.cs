using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveLoadingWrapper: MonoBehaviour
{
   public TMPro.TextMeshProUGUI WaveText;
   public Image Background;

   public void DisplayWave() {
      StartCoroutine(FadeWaveScreen());
   }
   IEnumerator FadeWaveScreen() {
      Background.color = new Color(0f, 0f, 0f, 0f);
      WaveText.color = new Color(1f, 1f, 1f, 0f);
      float seconds = 0f;
      while(seconds < 0.5f) {
         Background.color = new Color(0f, 0f, 0f, Mathf.Lerp(0f, 0.9f, seconds * 2f));
         WaveText.color = new Color(1f, 1f, 1f, Mathf.Lerp(0f, 1f, seconds * 2f));
         seconds += Time.deltaTime;
         yield return new WaitForEndOfFrame();
      }
      yield return new WaitForSeconds(1f);
      seconds = 0.5f;
      while(seconds > 0f) {
         Background.color = new Color(0f, 0f, 0f, Mathf.Lerp(0f, 0.9f, seconds * 2f));
         WaveText.color = new Color(1f, 1f, 1f, Mathf.Lerp(0f, 1f, seconds * 2f));
         seconds -= Time.deltaTime;
         yield return new WaitForEndOfFrame();
      }
      FindObjectOfType<GameManager>().StartGame();
      gameObject.SetActive(false);
   }
}
