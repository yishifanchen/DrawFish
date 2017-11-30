using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PaletteRU : MonoBehaviour {
    DrawRU m_draw;
    Texture2D m_paletteTex2D;
    Texture2D m_swatchesTex2D;
    int m_paletteTex2DLength=181;
    int m_swatchesWidth=15;
    int m_swatchesHeight = 181;
    int m_offestY;
    Color[,] m_arrayColor;
    Rect m_selectRect1;
    Rect m_selectRect2;
    Rect m_selectRect1fan;
    Rect m_selectRect2fan;
    Vector2 m_mousePosition;
    public bool m_isShowPalette=false;
    
    void Start()
    {
        m_draw = GetComponent<DrawRU>();
        m_arrayColor = new Color[m_paletteTex2DLength, m_paletteTex2DLength];
        m_paletteTex2D = new Texture2D(m_paletteTex2DLength, m_paletteTex2DLength, TextureFormat.RGB24, true);
        m_swatchesTex2D = new Texture2D(m_swatchesWidth, m_swatchesHeight, TextureFormat.RGB24, true);
        m_selectRect2 = new Rect(((float)Screen.width / 1920 * 1060), ((float)Screen.height / 1080 * 38), m_paletteTex2D.width, m_paletteTex2D.height);
        m_selectRect1 = new Rect(((float)Screen.width / 1920 * 1060) + m_paletteTex2D.width + 5, ((float)Screen.height / 1080 * 38), m_swatchesWidth, m_swatchesHeight);

        m_selectRect2fan = new Rect(((float)Screen.width / 1920 * 1820) - m_paletteTex2D.width, ((float)Screen.height / 1080 * 502) - m_paletteTex2D.height, m_paletteTex2D.width, m_paletteTex2D.height);
        m_selectRect1fan = new Rect(((float)Screen.width / 1920 * 1820) - m_paletteTex2D.width - m_swatchesWidth - 5, ((float)Screen.height / 1080 * 502) - m_swatchesHeight, m_swatchesWidth, m_swatchesHeight);
        f_SetSwatchesPanel();
        f_SetColorPanel(Color.red);
        m_offestY = (int)((float)Screen.height / 1080 * 502);
    }
    void Update()
    {
        switch (Config.m_curInputType)
        {
            case Config.InputType.MouseInput:
                MouseInput();
                break;
            case Config.InputType.TouchInput:
                TouchInput();
                break;
        }
    }
    void OnGUI()
    {
        if (m_isShowPalette)
        {
            GUI.DrawTexture(m_selectRect2fan, m_paletteTex2D);
            GUI.DrawTexture(m_selectRect1fan, m_swatchesTex2D);
            GUI.depth = 0;
        }
    }
    Color[] f_CalcArrayColor(Color endColor)//调色板
    {
        Color value = (endColor - Color.white) / (m_paletteTex2DLength - 1);
        for (int i = 0; i < m_paletteTex2DLength; i++)
        {
            m_arrayColor[i, m_paletteTex2DLength - 1] = Color.white + value * i;
        }
        for (int i = 0; i < m_paletteTex2DLength; i++)
        {
            value = (m_arrayColor[i, m_paletteTex2DLength - 1] - Color.black) / (m_paletteTex2DLength - 1);
            for (int j = 0; j < m_paletteTex2DLength; j++)
            {
                m_arrayColor[i, j] = Color.black + value * j;
            }
        }
        List<Color> listColor = new List<Color>();
        for (int i = 0; i < m_paletteTex2DLength; i++)
        {
            for (int j = 0; j < m_paletteTex2DLength; j++)
            {
                listColor.Add(m_arrayColor[j, i]);
            }
        }
        return listColor.ToArray();
    }
    Color[] f_CalcArrayColorFan(Color endColor)//调色板fan
    {
        Color value = (endColor - Color.white) / (m_paletteTex2DLength - 1);
        for (int i = m_paletteTex2DLength - 1; i > -1; i--)
        {
            m_arrayColor[i, m_paletteTex2DLength - 1] = Color.white + value * i;
        }
        for (int i = m_paletteTex2DLength - 1; i > -1; i--)
        {
            value = (m_arrayColor[i, m_paletteTex2DLength - 1] - Color.black) / (m_paletteTex2DLength - 1);
            for (int j = m_paletteTex2DLength - 1; j > -1; j--)
            {
                m_arrayColor[i, j] = Color.black + value * j;
            }
        }
        List<Color> listColor = new List<Color>();
        for (int i = m_paletteTex2DLength - 1; i > -1; i--)
        {
            for (int j = m_paletteTex2DLength - 1; j > -1; j--)
            {
                listColor.Add(m_arrayColor[j, i]);
            }
        }
        return listColor.ToArray();
    }
    Color[] f_CalcSwatchesColor()//取色板
    {
        int m_addValue = (m_swatchesHeight - 1) / 6;
        for (int i = 0; i < m_swatchesWidth; i++)
        {
            m_arrayColor[i, 0] = Color.red;
            m_arrayColor[i, m_addValue] = Color.yellow;
            m_arrayColor[i, m_addValue * 2] = Color.green;
            m_arrayColor[i, m_addValue * 3] = Color.cyan;
            m_arrayColor[i, m_addValue * 4] = Color.blue;
            m_arrayColor[i, m_addValue * 5] = Color.magenta;
            m_arrayColor[i, m_swatchesHeight - 1] = Color.red;
        }
        Color value = (Color.yellow - Color.red) / m_addValue;
        for (int i = 0; i < m_swatchesWidth; i++)
        {
            for (int j = 0; j < m_addValue; j++)
            {
                m_arrayColor[i, j] = Color.red + value * j;
            }
        }
        value = (Color.green - Color.yellow) / m_addValue;
        for (int i = 0; i < m_swatchesWidth; i++)
        {
            for (int j = m_addValue; j < m_addValue * 2; j++)
            {
                m_arrayColor[i, j] = Color.yellow + value * (j - m_addValue);
            }
        }
        value = (Color.cyan - Color.green) / m_addValue;
        for (int i = 0; i < m_swatchesWidth; i++)
        {
            for (int j = m_addValue * 2; j < m_addValue * 3; j++)
            {
                m_arrayColor[i, j] = Color.green + value * (j - m_addValue * 2);
            }
        }
        value = (Color.blue - Color.cyan) / m_addValue;
        for (int i = 0; i < m_swatchesWidth; i++)
        {
            for (int j = m_addValue * 3; j < m_addValue * 4; j++)
            {
                m_arrayColor[i, j] = Color.cyan + value * (j - m_addValue * 3);
            }
        }
        value = (Color.magenta - Color.blue) / m_addValue;
        for (int i = 0; i < m_swatchesWidth; i++)
        {
            for (int j = m_addValue * 4; j < m_addValue * 5; j++)
            {
                m_arrayColor[i, j] = Color.blue + value * (j - m_addValue * 4);
            }
        }
        value = (Color.red - Color.magenta) / m_addValue;
        for (int i = 0; i < m_swatchesWidth; i++)
        {
            for (int j = m_addValue * 5; j < m_swatchesHeight - 1; j++)
            {
                m_arrayColor[i, j] = Color.magenta + value * (j - m_addValue * 5);
            }
        }
        List<Color> listColor = new List<Color>();
        for (int i = 0; i < m_swatchesHeight; i++)
        {
            for (int j = 0; j < m_swatchesWidth; j++)
            {
                listColor.Add(m_arrayColor[j, i]);
            }
        }
        return listColor.ToArray();
    }
    Color[] f_CalcSwatchesColorFan()//取色板fan
    {
        int m_addValue = (m_swatchesHeight - 1) / 6;
        for (int i = 0; i < m_swatchesWidth; i++)
        {
            m_arrayColor[i, 0] = Color.red;
            m_arrayColor[i, m_addValue] = Color.magenta;
            m_arrayColor[i, m_addValue * 2] = Color.blue;
            m_arrayColor[i, m_addValue * 3] = Color.cyan;
            m_arrayColor[i, m_addValue * 4] = Color.green;
            m_arrayColor[i, m_addValue * 5] = Color.yellow;
            m_arrayColor[i, m_swatchesHeight - 1] = Color.red;
        }
        Color value = (Color.magenta - Color.red) / m_addValue;
        for (int i = 0; i < m_swatchesWidth; i++)
        {
            for (int j = 0; j < m_addValue; j++)
            {
                m_arrayColor[i, j] = Color.red + value * j;
            }
        }
        value = (Color.blue - Color.magenta) / m_addValue;
        for (int i = 0; i < m_swatchesWidth; i++)
        {
            for (int j = m_addValue; j < m_addValue * 2; j++)
            {
                m_arrayColor[i, j] = Color.magenta + value * (j - m_addValue);
            }
        }
        value = (Color.cyan - Color.blue) / m_addValue;
        for (int i = 0; i < m_swatchesWidth; i++)
        {
            for (int j = m_addValue * 2; j < m_addValue * 3; j++)
            {
                m_arrayColor[i, j] = Color.blue + value * (j - m_addValue * 2);
            }
        }
        value = (Color.green - Color.cyan) / m_addValue;
        for (int i = 0; i < m_swatchesWidth; i++)
        {
            for (int j = m_addValue * 3; j < m_addValue * 4; j++)
            {
                m_arrayColor[i, j] = Color.cyan + value * (j - m_addValue * 3);
            }
        }
        value = (Color.yellow - Color.green) / m_addValue;
        for (int i = 0; i < m_swatchesWidth; i++)
        {
            for (int j = m_addValue * 4; j < m_addValue * 5; j++)
            {
                m_arrayColor[i, j] = Color.green + value * (j - m_addValue * 4);
            }
        }
        value = (Color.red - Color.yellow) / m_addValue;
        for (int i = 0; i < m_swatchesWidth; i++)
        {
            for (int j = m_addValue * 5; j < m_swatchesHeight - 1; j++)
            {
                m_arrayColor[i, j] = Color.yellow + value * (j - m_addValue * 5);
            }
        }
        List<Color> listColor = new List<Color>();
        for (int i = 0; i < m_swatchesHeight; i++)
        {
            for (int j = 0; j < m_swatchesWidth; j++)
            {
                listColor.Add(m_arrayColor[j, i]);
            }
        }
        return listColor.ToArray();
    }
    public void f_SetColorPanel(Color endColor)//设置调色板颜色
    {
        Color[] CalcArray = f_CalcArrayColorFan(endColor);
        m_paletteTex2D.SetPixels(CalcArray);
        m_paletteTex2D.Apply();
    }
    public void f_SetSwatchesPanel()//设置取色板颜色
    {
        Color[] m_swatchesArray = f_CalcSwatchesColorFan();
        m_swatchesTex2D.SetPixels(m_swatchesArray);
        m_swatchesTex2D.Apply();
    }
    IEnumerator f_DelayDraw()
    {
        yield return new WaitForSeconds(.1f);
        GetComponent<PaletteRU>().enabled = false;
        m_isShowPalette = false;
        m_draw.m_isCanDraw = true;
        m_draw.f_ButtonColorGroupInteractableYes();
    }
    void MouseInput()//鼠标输入
    {
        m_mousePosition.x = Input.mousePosition.x;
        m_mousePosition.y = Screen.height - Input.mousePosition.y;
        if (m_selectRect1fan.Contains(m_mousePosition))
        {
            if (Input.GetMouseButton(0))
            {
                Color m_selectColor = m_swatchesTex2D.GetPixel((int)(m_mousePosition.x - m_selectRect1fan.x), m_offestY - (int)m_mousePosition.y);
                f_SetColorPanel(m_selectColor);
            }
        }
        if (m_selectRect2fan.Contains(m_mousePosition))
        {
            if (Input.GetMouseButtonDown(0))
            {
                m_draw.m_curColor = m_paletteTex2D.GetPixel((int)(m_mousePosition.x - m_selectRect2fan.x), m_offestY - (int)m_mousePosition.y);
                StartCoroutine(f_DelayDraw());
            }
        }
        if (!m_selectRect1fan.Contains(m_mousePosition) && !m_selectRect2fan.Contains(m_mousePosition) && Config.m_rectRU.Contains(Input.mousePosition))
        {
            if (Input.GetMouseButtonDown(0))
            {
                StartCoroutine(f_DelayDraw());
            }
        }
    }
    void TouchInput()//触控输入
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            m_mousePosition.x = Input.GetTouch(i).position.x;
            m_mousePosition.y = Screen.height - Input.GetTouch(i).position.y;
            if (m_selectRect1fan.Contains(m_mousePosition))
            {
                if (Input.GetTouch(i).phase == TouchPhase.Began || Input.GetTouch(i).phase == TouchPhase.Moved)
                {
                    Color m_selectColor = m_swatchesTex2D.GetPixel((int)(m_mousePosition.x - m_selectRect1fan.x), m_offestY - (int)m_mousePosition.y);
                    f_SetColorPanel(m_selectColor);
                }
            }
            if (m_selectRect2fan.Contains(m_mousePosition))
            {
                if (Input.GetTouch(i).phase == TouchPhase.Began)
                {
                    m_draw.m_curColor = m_paletteTex2D.GetPixel((int)(m_mousePosition.x - m_selectRect2fan.x), m_offestY - (int)m_mousePosition.y);
                    StartCoroutine(f_DelayDraw());
                }
            }
            if (!m_selectRect1fan.Contains(m_mousePosition) && !m_selectRect2fan.Contains(m_mousePosition) && Config.m_rectRU.Contains(Input.GetTouch(i).position))
            {
                if (Input.GetTouch(i).phase == TouchPhase.Began)
                {
                    StartCoroutine(f_DelayDraw());
                }
            }
        }
    }
}
