/// Daniel C Menezes
/// Procedural UI Rounded Corners - https://danielcmcg.github.io/
/// 
/// Based on CiaccoDavide's Unity-UI-Polygon
/// Sourced from - https://github.com/CiaccoDavide/Unity-UI-Polygon

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
[ExecuteInEditMode]
public class ProUIRC_Layout : MonoBehaviour
{
    public ProUIRC_Drawer drawer;

    [SerializeField] public ProUIRC_LayoutVertice[] layoutVerticeArray = new ProUIRC_LayoutVertice[4];
    [SerializeField] public bool fill = true;
    [SerializeField] public float thickness = 5;
    [SerializeField] public int cornerSides = 20;

    public RectTransform rectTransform;

    private void OnEnable()
    {
        if (GetComponent<Graphic>())
            DestroyImmediate(GetComponent<Graphic>());

        drawer = GetComponent<ProUIRC_Drawer>() ? GetComponent<ProUIRC_Drawer>() : gameObject.AddComponent<ProUIRC_Drawer>();
        drawer.layout = this;
        rectTransform = GetComponent<RectTransform>();

        Init();
    }

    void OnValidate()
    {
        drawer.SetVerticesDirty();
    }

    public void Init()
    {
        for (int i = 0; i < layoutVerticeArray.Length; i++)
        {
            if (layoutVerticeArray[i] == null)
            {
                layoutVerticeArray[i] = new ProUIRC_LayoutVertice();
            }

            switch (i)
            {
                case 0:
                    layoutVerticeArray[i].corner = new Vector2(1, 1);
                    break;
                case 1:
                    layoutVerticeArray[i].corner = new Vector2(1, -1);
                    break;
                case 2:
                    layoutVerticeArray[i].corner = new Vector2(-1, -1);
                    break;
                case 3:
                    layoutVerticeArray[i].corner = new Vector2(-1, 1);
                    break;
                default:
                    break;
            }
        }
    }
}

[System.Serializable]
public class ProUIRC_LayoutVertice
{
    public Vector2 corner;
    [SerializeField] public float sides;
    [SerializeField] public float radius;

    public Vector2Int NeighborCornerHV
    {
        get
        {
            Vector2Int r = new Vector2Int(0, 0);
            if (corner == new Vector2(1, 1))
            {
                r = new Vector2Int(3, 1);
            }
            if (corner == new Vector2(1, -1))
            {
                r = new Vector2Int(2, 0);
            }
            if (corner == new Vector2(-1, -1))
            {
                r = new Vector2Int(1, 3);
            }
            if (corner == new Vector2(-1, 1))
            {
                r = new Vector2Int(0, 2);
            }
            return r;
        }
    }
}