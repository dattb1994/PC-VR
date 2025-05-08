using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScene : MonoBehaviour
{
    public Image progressBar;
    public Text progressText;
    public string sceneToLoad = "GameScene";

    public RectTransform a, b, img;
    private IEnumerator Start()
    {
        Resources.UnloadUnusedAssets();
        sceneToLoad = GameSetting.sceneToLoad;
        SwitchSceneBlurEffect.Instance.ForceShowBlur();
        SwitchSceneBlurEffect.Instance.HideBlur();
        yield return new WaitForSeconds(1f);
        StartCoroutine(LoadSceneRoutine());
    }
    private void Update()
    {
        img.position = Vector3.Lerp(a.position, b.position, Mathf.PingPong((float)fakeProgress / 100, 1));
    }
    public float fakeProgress = 0f;
    IEnumerator LoadSceneRoutine()
    {
        SwitchSceneBlurEffect.Instance.ForceHideBlur();
        

        // Giai đoạn 1: Fake từ 1% → 60%
        while (fakeProgress < 60f)
        {
            fakeProgress += Time.deltaTime * 40f; // Tốc độ tăng
            UpdateUI(fakeProgress);
            yield return null;
        }

        // Giai đoạn 2: Load scene thật
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad);
        operation.allowSceneActivation = false;

        while (operation.progress < 0.9f)
        {
            float targetProgress = 60f + (operation.progress / 0.9f) * 10f; // 60% → 70%
            fakeProgress = Mathf.MoveTowards(fakeProgress, targetProgress, Time.deltaTime * 30f);
            UpdateUI(fakeProgress);
            yield return null;
        }

        // Giai đoạn 3: Fake từ 70% → 100%
        while (fakeProgress < 100f)
        {
            fakeProgress = Mathf.MoveTowards(fakeProgress, 100f, Time.deltaTime * 30f);
            UpdateUI(fakeProgress);
            yield return null;
        }

        // Chờ 1s rồi chuyển scene  
        yield return new WaitForSeconds(1f);
        SwitchSceneBlurEffect.Instance.ShowBlur();
        yield return new WaitForSeconds(1f);
        operation.allowSceneActivation = true;
    }

    void UpdateUI(float value)
    {
        progressBar.fillAmount = value / 100f;
        progressText.text = $"{Mathf.FloorToInt(value)}%";
    }
}
