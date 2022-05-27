using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ShadowObject : MonoBehaviour
{
    public List<ShadowSprite> shadowImages = new List<ShadowSprite>();
    public Transform target;
    public SortingGroup sortingGroup;
    public bool isInit;
    public Vector3 offset = new Vector3(0, -1f, 0);
    public Vector3 offsetRot = new Vector3(0, -90f, 0);
    public Vector3 scaleMultiple = new Vector3(0.8f, 0.5f, 0.8f);

    public bool updatePose;
    public bool updateSprite;
    [ColorUsage(true)]
    public Color color;
    [ContextMenu("lnit")]
    public void Init()
    {
        var targetImagers = target.GetComponentsInChildren<SpriteRenderer>();
        transform.SetParent(target);
        transform.position = target.position;
        sortingGroup = gameObject.AddComponent<SortingGroup>();
        for (int i = 0; i < targetImagers.Length; i++)
        {
            GameObject shadowImage = new GameObject("shadowImage");
            shadowImage.transform.SetParent(transform);
            shadowImages.Add(shadowImage.AddComponent<ShadowSprite>());
            shadowImages[i].Init(target, targetImagers[i], color);
        }
        isInit = true;


    }
    public void SetSorting(int index)
    {
        sortingGroup.sortingOrder = index;
        //shadowImages.ForEach((x)=>x.SetSorting(index));
    }
    void Update()
    {
        if (!isInit) return;

        if (updatePose)
        {
            shadowImages.ForEach((x) => x.UpdateShadowPose(offset,scaleMultiple,offsetRot));
        }

        if (updateSprite)
        {
            shadowImages.ForEach((x) => x.UpdateShadowSprite(color));
        }
    }

}
