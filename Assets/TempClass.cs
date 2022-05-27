 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;
using System.Linq;
public class TempClass : MonoBehaviour
{
    #region sample
    public TextMeshProUGUI text;
    public Sprite sprite;
    public string target;

    [ContextMenu("Test")]
    private void Test()
    {
        TextToImage(text, target,sprite);
    }
    [ContextMenu("Test2")]
    private void Test2()
    {
        TextToImage(text, target, sprite,0,new Vector2(50,50));
    }
    [ContextMenu("Test3")]
    private void Test3()
    {
        AllTextToImage(text, target, sprite, new Vector2(50, 50));
    }
    #endregion

    public Image TextToImage(TextMeshProUGUI tmp, string targetText, Sprite sprite)
    {
        return TextToImage(tmp, targetText, sprite,0,null);
    }

    public Image TextToImage(TextMeshProUGUI tmp, string targetText, Sprite sprite, int startIndex)
    {
        return TextToImage(tmp, targetText, sprite, startIndex,null);
    }

    public Image TextToImage(TextMeshProUGUI tmp, string targetText, Sprite sprite, Vector2? imageSize = null)
    {
        return TextToImage(tmp, targetText, sprite, 0,imageSize);
    }

    /// <summary>
    /// 해당 텍스트 내용을 이미지로 치환해주는 코드임. 치환된 내용은 색깔만 안보이게 바뀜
    /// </summary>
    /// <param name="tmp">텍스트메쉬프로 넣으삼</param>
    /// <param name="targetText">바꿀 string넣으삼</param>
    /// <param name="sprite">바꿀 sprite 넣으삼</param>
    /// <param name="startIndex">바꾸기 시작할 string index넣으삼</param>
    /// <param name="imageSize">이미지 사이즈 바꾸려면 넣으삼</param>
    /// <returns></returns>
    public Image TextToImage(TextMeshProUGUI tmp, string targetText, Sprite sprite,int startIndex=0 ,Vector2? imageSize = null)
    {
        int charIndex = tmp.text.IndexOf(targetText, startIndex);
        if (charIndex<0)
        {
            return null;
        }
        TMP_TextInfo textInfo = tmp.textInfo;


        int charLastIndex = charIndex + targetText.Length - 1;

        TMP_CharacterInfo charInfo = textInfo.characterInfo[charIndex];
        TMP_CharacterInfo charLastInfo = textInfo.characterInfo[charLastIndex];

        Vector2 textSize;

        float textSizeX = charLastInfo.topRight.x - charInfo.bottomLeft.x;
        float textSizeY = 0;
        for (int i = 0; i < targetText.Length; i++)
        {
            TMP_CharacterInfo curCharInfo = textInfo.characterInfo[charIndex + i];
            Vector2 charSize = curCharInfo.topRight - curCharInfo.bottomLeft;
            if (textSizeY < charSize.y) { textSizeY = charSize.y; }

            SetColorSingleWord(tmp, curCharInfo);
        }
        textSize = new Vector2(textSizeX, textSizeY);

        Image image = new GameObject("TextToImage").AddComponent<Image>();
        RectTransform imageRect = image.GetComponent<RectTransform>();
        imageRect.SetParent(tmp.transform);
        imageRect.sizeDelta = imageSize ?? textSize;

        Vector2 textPos = (Vector2)charInfo.bottomLeft + (textSize) * 0.5f; ;
        imageRect.anchoredPosition = textPos;

        image.sprite = sprite;

        return image;
    }

    /// <summary>
    /// 모든 text 바꾸려면 넣으삼
    /// </summary>
    /// <param name="tmp"></param>
    /// <param name="targetText"></param>
    /// <param name="sprite"></param>
    /// <param name="imageSize"></param>
    /// <returns></returns>
    public List<Image> AllTextToImage(TextMeshProUGUI tmp, string targetText, Sprite sprite, Vector2? imageSize = null)
    {
        if (!tmp.text.Contains(targetText))
        {
            return null;
        }
        List<Image> images = new List<Image>();
        int startIndex = 0;
        while (true)
        {
            startIndex = tmp.text.IndexOf(targetText, startIndex+1);
            if (startIndex < 0)
                break;
            var image = TextToImage(tmp, targetText, sprite, startIndex, imageSize);
            images.Add(image);
        }
        return images;
    }

    /// <summary>
    /// 색바까주는코드
    /// </summary>
    /// <param name="tmp"></param>
    /// <param name="charInfo"></param>
    public void SetColorSingleWord(TextMeshProUGUI tmp , TMP_CharacterInfo charInfo)
    {
        var textInfo = tmp.textInfo;
        int meshIndex = charInfo.materialReferenceIndex;
        int vertexIndex = charInfo.vertexIndex;

        Color32[] vertexColors = textInfo.meshInfo[meshIndex].colors32;
        vertexColors[vertexIndex + 0] = Color.clear;
        vertexColors[vertexIndex + 1] = Color.clear;
        vertexColors[vertexIndex + 2] = Color.clear;
        vertexColors[vertexIndex + 3] = Color.clear;

        tmp.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
    }
}
