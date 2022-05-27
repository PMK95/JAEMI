using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShadowSprite : MonoBehaviour
{
    public SpriteRenderer shadowImage;
    public Transform targetTranform;
    public SpriteRenderer targetImage;

    public Transform targetRoot;
    public void Init(Transform root,SpriteRenderer targetImage,Color color)
    {
        targetRoot = root;
        this.targetImage = targetImage;
        targetTranform = targetImage.transform;
        shadowImage = gameObject.AddComponent<SpriteRenderer>();

        shadowImage.sprite = targetImage.sprite;
        shadowImage.sortingOrder = targetImage.sortingOrder;

        shadowImage.color = color;
    }
    public void UpdateShadowPose(Vector3 offset,Vector3 scaleMultiple,Vector3 offsetRot)
    {
        var pos = (targetRoot.position + targetTranform.position) + offset;
        transform.localPosition = new Vector3(pos.x, pos.y, 0);
        transform.localRotation = targetRoot.rotation* targetTranform.rotation * Quaternion.Euler(offsetRot);
        transform.localScale = new Vector3(targetTranform.lossyScale.x * scaleMultiple.x, targetTranform.lossyScale.y * scaleMultiple.y, targetTranform.lossyScale.z * scaleMultiple.z);
    }

    public void UpdateShadowSprite(Color color)
    {
        shadowImage.sprite = targetImage.sprite;
        shadowImage.sortingOrder = targetImage.sortingOrder;
        shadowImage.color = color;
    }
    public void SetSorting(int index)
    {
        shadowImage.sortingOrder = index;
    }
}
