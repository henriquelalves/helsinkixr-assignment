using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static void FadeCanvasGroup(MonoBehaviour mb, CanvasGroup cg, float endAlpha, float duration)
    {
        IEnumerator FadeOutCoroutine()
        {
            var startAlpha = cg.alpha;
            var w = 0f;
            while (w < 1f)
            {
                cg.alpha = Mathf.Lerp(startAlpha, endAlpha, w);
                w += Time.deltaTime / duration;
                yield return 0;
            }
        }

        mb.StartCoroutine(FadeOutCoroutine());
    }
}
