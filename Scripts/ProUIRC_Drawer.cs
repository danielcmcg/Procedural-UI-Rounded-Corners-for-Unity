/// Daniel C Menezes
/// Procedural UI Rounded Corners - https://danielcmcg.github.io/
/// 
/// Based on CiaccoDavide's Unity-UI-Polygon
/// Sourced from - https://github.com/CiaccoDavide/Unity-UI-Polygon

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ProUIRC_Drawer : MaskableGraphic
{
    public ProUIRC_Layout layout;

    [HideInInspector] [SerializeField] Texture m_Texture;

    float rotation = 0;

    public override Texture mainTexture
    {
        get => m_Texture == null ? s_WhiteTexture : m_Texture;
    }
    public Texture texture
    {
        get => m_Texture;
        set
        {
            if (m_Texture == value) return;
            m_Texture = value;
            SetVerticesDirty();
            SetMaterialDirty();
        }
    }

    protected override void OnValidate()
    {
        if (GetComponent<Image>())
            Destroy(GetComponent<Image>());

        layout = GetComponent<ProUIRC_Layout>() ? GetComponent<ProUIRC_Layout>() : gameObject.AddComponent<ProUIRC_Layout>();
    }

    protected UIVertex[] SetVbo(Vector2[] vertices, Vector2[] uvs)
    {
        UIVertex[] vbo = new UIVertex[4];
        for (int i = 0; i < vertices.Length; i++)
        {
            var vert = UIVertex.simpleVert;
            vert.color = color;
            vert.position = vertices[i];
            vert.uv0 = uvs[i];
            vbo[i] = vert;
        }
        return vbo;
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        ProUIRC_LayoutVertice[] layoutVerticeArray = layout.layoutVerticeArray;

        int i = 0;
        foreach (ProUIRC_LayoutVertice vertice in layoutVerticeArray)
        {
            DrawCorner(vh, vertice.corner, vertice.radius, layoutVerticeArray[i].sides);
            int j = i + 1;
            if (j >= layoutVerticeArray.Length)
                j = 0;

            DrawSide(vh, vertice.corner, layoutVerticeArray[i].radius, layoutVerticeArray[j].radius);
            i++;
        }

        if (layout.fill)
            DrawFill(vh, layoutVerticeArray);
    }

    void DrawCorner(VertexHelper vh, Vector2 corner, float cornerRadius, float cornerSides)
    {
        Vector2 offset = new Vector2((-rectTransform.rect.width / 2 + cornerRadius) * corner.x, (-rectTransform.rect.height / 2 + cornerRadius) * corner.y);
        Vector2 prevX = Vector2.zero + offset;
        Vector2 prevY = Vector2.zero + offset;
        Vector2 uv0 = new Vector2(0, 0);
        Vector2 uv1 = new Vector2(0, 1);
        Vector2 uv2 = new Vector2(1, 1);
        Vector2 uv3 = new Vector2(1, 0);
        Vector2 pos0;
        Vector2 pos1;
        Vector2 pos2;
        Vector2 pos3;

        float degrees = 90f / layout.cornerSides;
        if (corner == new Vector2(1, 1))
        {
            rotation = 0;
        }
        if (corner == new Vector2(-1, 1))
        {
            rotation = 90;
        }
        if (corner == new Vector2(-1, -1))
        {
            rotation = 180;
        }
        if (corner == new Vector2(1, -1))
        {
            rotation = 270;
        }

        int vertices = layout.cornerSides + 1;

        for (int i = 0; i < vertices; i++)
        {
            float outer = -rectTransform.pivot.x * cornerRadius * 2;
            float inner = -rectTransform.pivot.x * cornerRadius * 2 + layout.thickness;
            float rad = Mathf.Deg2Rad * (i * degrees + rotation);
            float c = Mathf.Cos(rad);
            float s = Mathf.Sin(rad);
            uv0 = new Vector2(0, 1);
            uv1 = new Vector2(1, 1);
            uv2 = new Vector2(1, 0);
            uv3 = new Vector2(0, 0);

            pos0 = prevX;
            pos1 = new Vector2(outer * c, outer * s) + offset;

            if (layout.fill)
            {
                pos2 = Vector2.zero + offset;
                pos3 = Vector2.zero + offset;
            }
            else
            {
                pos2 = new Vector2(inner * c, inner * s) + offset;
                pos3 = prevY;
            }
            prevX = pos1;
            prevY = pos2;

            prevX = pos1;
            prevY = pos2;
            vh.AddUIVertexQuad(SetVbo(new[] { pos0, pos1, pos2, pos3 }, new[] { uv0, uv1, uv2, uv3 }));
        }
    }

    void DrawFill(VertexHelper vh, ProUIRC_LayoutVertice[] layoutVerticeArray)
    {
        Vector2 uv0 = new Vector2(0, 0);
        Vector2 uv1 = new Vector2(0, 1);
        Vector2 uv2 = new Vector2(1, 1);
        Vector2 uv3 = new Vector2(1, 0);

        uv0 = new Vector2(0, 1);
        uv1 = new Vector2(1, 1);
        uv2 = new Vector2(1, 0);
        uv3 = new Vector2(0, 0);

        Vector2[] posArr = new Vector2[4];
        for (int i = 0; i < posArr.Length; i++)
        {
            posArr[i] = new Vector2((-rectTransform.rect.width / 2 + layoutVerticeArray[i].radius) * layoutVerticeArray[i].corner.x, 
                                    (-rectTransform.rect.height / 2 + layoutVerticeArray[i].radius) * layoutVerticeArray[i].corner.y);
        }

        vh.AddUIVertexQuad(SetVbo(new[] { posArr[0], posArr[1], posArr[2], posArr[3] }, new[] { uv0, uv1, uv2, uv3 }));

    }

    void DrawSide(VertexHelper vh, Vector2 corner, float sideRadius1, float sideRadius2)
    {
        Vector2 uv0 = new Vector2(0, 0);
        Vector2 uv1 = new Vector2(0, 1);
        Vector2 uv2 = new Vector2(1, 1);
        Vector2 uv3 = new Vector2(1, 0);
        Vector2 pos0;
        Vector2 pos1;
        Vector2 pos2;
        Vector2 pos3;

        uv0 = new Vector2(0, 1);
        uv1 = new Vector2(1, 1);
        uv2 = new Vector2(1, 0);
        uv3 = new Vector2(0, 0);

        if (corner.x == corner.y)
        {
            pos0 = new Vector2((-rectTransform.rect.width / 2) * corner.x, (-rectTransform.rect.height / 2 + sideRadius1) * corner.y);
            pos1 = new Vector2((-rectTransform.rect.width / 2) * corner.x, (-rectTransform.rect.height / 2 + sideRadius2) * -corner.y);
            if (layout.fill)
            {
                pos2 = new Vector2((-rectTransform.rect.width / 2 + sideRadius2) * corner.x, (-rectTransform.rect.height / 2 + sideRadius2) * -corner.y);
                pos3 = new Vector2((-rectTransform.rect.width / 2 + sideRadius1) * corner.x, (-rectTransform.rect.height / 2 + sideRadius1) * corner.y);
            }
            else
            {
                pos2 = new Vector2((-rectTransform.rect.width / 2 + layout.thickness) * corner.x, (-rectTransform.rect.height / 2 + sideRadius2) * -corner.y);
                pos3 = new Vector2((-rectTransform.rect.width / 2 + layout.thickness) * corner.x, (-rectTransform.rect.height / 2 + sideRadius1) * corner.y);
            }
        }
        else
        {
            pos0 = new Vector2((-rectTransform.rect.width / 2 + sideRadius1) * corner.x, (-rectTransform.rect.height / 2) * corner.y);
            pos1 = new Vector2((-rectTransform.rect.width / 2 + sideRadius2) * -corner.x, (-rectTransform.rect.height / 2) * corner.y);

            if (layout.fill)
            {
                pos2 = new Vector2((-rectTransform.rect.width / 2 + sideRadius2) * -corner.x, (-rectTransform.rect.height / 2 + sideRadius2) * corner.y);
                pos3 = new Vector2((-rectTransform.rect.width / 2 + sideRadius1) * corner.x, (-rectTransform.rect.height / 2 + sideRadius1) * corner.y);
            }
            else
            {
                pos2 = new Vector2((-rectTransform.rect.width / 2 + sideRadius2) * -corner.x, (-rectTransform.rect.height / 2 + layout.thickness) * corner.y);
                pos3 = new Vector2((-rectTransform.rect.width / 2 + sideRadius1) * corner.x, (-rectTransform.rect.height / 2 + layout.thickness) * corner.y);
            }
        }

        vh.AddUIVertexQuad(SetVbo(new[] { pos0, pos1, pos2, pos3 }, new[] { uv0, uv1, uv2, uv3 }));
    }

}
