using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class TextAnimation : MonoBehaviour
{
    [SerializeField] private PlayerStat playerStat;
    [SerializeField] private TextMeshPro _textMeshPro;
    [SerializeField] private EventBroadcast eventBroadcast;

    public GameObject textBox;
    public TMP_FontAsset[] fontAssets;
    public List<string> textBoxList = new List<string>();
    private bool _isPlaying = false;
    private int counter = 0;

    [SerializeField] float timeBtwnChars = 0.01f;

    int i = 0;

    private void OnEnable()
    {
        i = 0;
        _isPlaying = false;
        EndCheck();
    }

    private void Update()
    {
        if (PlayerControl.Instance.pInput.Player.Interact.WasPressedThisFrame())
        {
            EndCheck();
        }
    }

    public void EndCheck()
    {
        if (_isPlaying)
        {
            counter = _textMeshPro.textInfo.characterCount - 1;
            _isPlaying = false;
        }
        else if (i < textBoxList.Count)
        {
            _textMeshPro.text = textBoxList[i];
            StartCoroutine(TextVisible());
        }
        else
        {
            textBox.SetActive(false);
            eventBroadcast.FinishTextNoti();
        }
    }

    private IEnumerator TextVisible()
    {
        _textMeshPro.ForceMeshUpdate();
        int totalVisibleCharacters = _textMeshPro.textInfo.characterCount;
        counter = 0;

        while (true)
        {
            _isPlaying = true;
            int visibleCount = counter % (totalVisibleCharacters + 1);
            _textMeshPro.maxVisibleCharacters = visibleCount;

            if (visibleCount >= totalVisibleCharacters)
            {
                i += 1;
                _isPlaying = false;
                break;
            }

            counter += 1;
            yield return StartCoroutine(CoroutineUtil.WaitForRealSeconds(timeBtwnChars));


        }
    }
    private static class CoroutineUtil
    {
        public static IEnumerator WaitForRealSeconds(float time)
        {
            float start = Time.realtimeSinceStartup;
            while (Time.realtimeSinceStartup < start + time)
            {
                yield return null;
            }
        }
    }
}