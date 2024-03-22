using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode][RequireComponent(typeof(Camera))]
public class MaskableGraphicsDisplayer : MonoBehaviour
{
    private MaskableGraphic[] m_graphics;
    public Material mat;
    public Camera UICam;

    private void Start()
    {
        if (mat == null)
        {
            mat = new Material(Shader.Find("Sprites/Default"));
        }

        if (UICam == null)
        {
            UICam = GetComponent<Camera>();
        }
    }

    private void OnPostRender()
    {
        if (mat == null) return;
        if (UICam == null) return;
        
        GL.PushMatrix();
        mat.SetPass(0);
        GL.LoadOrtho();

        // 可自定义 限制遍历范围
        // 收集UI中可点击的对象
        m_graphics = FindObjectsOfType<MaskableGraphic>();
        for (int i = 0; i < m_graphics.Length; i++)
        {
            var mg = m_graphics[i];
            if (mg.gameObject.activeSelf && mg.raycastTarget)
            {
                DrawRectByGL(mg);
            }
        }
        GL.PopMatrix();
    }
    private void DrawRectByGL(MaskableGraphic mg)
    {
        Vector3[] corners = new Vector3[4];
        mg.rectTransform.GetWorldCorners(corners);

        for (int i = 0; i < 4; i++) {
            corners[i] = UICam.WorldToScreenPoint(corners[i]);
        }

        GLDrawLine(corners[0], corners[1]);
        GLDrawLine(corners[1], corners[2]);
        GLDrawLine(corners[2], corners[3]);
        GLDrawLine(corners[3], corners[0]);
    }
    
    private Vector2 GLVector(Vector2 screen) {
        return new Vector2(screen.x / Screen.width, screen.y / Screen.height);
    }

    private void GLDrawLine(Vector2 start, Vector2 end) {
        GL.Begin(GL.LINES);
        GL.Color(Color.blue);
        GL.Vertex(GLVector(start));
        GL.Vertex(GLVector(end));
        GL.End();
    }
}
