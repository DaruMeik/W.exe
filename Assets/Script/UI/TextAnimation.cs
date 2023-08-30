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
    private int lineCounter = 0;

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
            if (_textMeshPro.textInfo.lineCount > 4)
            {
                _textMeshPro.text = _textMeshPro.text.Remove(0, _textMeshPro.textInfo.lineInfo[0].characterCount * (_textMeshPro.textInfo.lineCount - 4));
            }
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
        int totalVisibleLines = _textMeshPro.textInfo.lineCount;
        counter = 0;
        lineCounter = 0;
        int previousLineCharCount = 0;

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
            if(counter > previousLineCharCount + _textMeshPro.textInfo.lineInfo[lineCounter].characterCount)
            {
                previousLineCharCount += _textMeshPro.textInfo.lineInfo[lineCounter].characterCount;
                lineCounter++;
                if(lineCounter >= 4)
                {
                    lineCounter--;
                    counter -= _textMeshPro.textInfo.lineInfo[0].characterCount;
                    _textMeshPro.text = _textMeshPro.text.Remove(0, _textMeshPro.textInfo.lineInfo[0].characterCount);
                }
            }
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