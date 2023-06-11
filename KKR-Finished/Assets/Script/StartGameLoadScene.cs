using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGameLoadScene : MonoBehaviour
{
    public GameObject LoadingScreen;
    public Slider LoadingBarfill;

    public string gameSceneName = "MainMenu"; // ���ͫչ�ͧ������ͧ�����Ŵ

    private void Start()
    {
        // �ʴ����ͫ�͹ GameObject �������Ǣ�ͧ�Ѻ˹�Ҩ���Ŵ
        LoadingScreen.SetActive(true);
        StartCoroutine(LoadGameSceneAsync());
    }
    private IEnumerator LoadGameSceneAsync()
    {
        // ��Ŵ�չ��Ẻ�����͡
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(gameSceneName);
        asyncOperation.allowSceneActivation = false;

        // ���Թ�����Ŵ����з���ʴ�ʶҹС����Ŵ

        while (!asyncOperation.isDone)
        {
            float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f); // �ӹǳ�����׺˹�Ңͧ�����Ŵ
            LoadingBarfill.value = progress;
            // ����觷��س��ͧ��������ʴ�ʶҹС����Ŵ (���Ѿവᶺ��Ŵ���͢�ͤ���)

            if (progress >= 0.9f)
            {
                // �ҡ�����׺˹���Թ 90% (0.9) �������ʶҹС����Ŵ
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
        LoadingScreen.SetActive(false);
    }
}
