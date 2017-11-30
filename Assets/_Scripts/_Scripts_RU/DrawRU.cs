using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DrawRU : MonoBehaviour {
    PaletteRU m_palette;//调色板
    public enum ToolType//工具类型
    {
        Pencil,
        Brush,
        Rubber
    }
    public enum BrushType//笔刷类型
    {
        BrushStar,
        BrushHeart,
        BrushPetal
    }
    public ToolType m_curTool;//当前工具
    public BrushType m_curBrush;//当前笔刷
    public Color m_curColor = Color.red;//当前颜色
    int m_markerSize=1;
    #region 持有图片、按钮、滑动条引用
    public Image m_image_Pencil;
    public Image m_image_Brush;
    public Image m_image_Rubber;
    public Image m_image_CurColor;
    public Button[] m_buttonGroup;
    public Button[] m_buttonColorGroup;
    public Slider m_sliderPencil;
    public Slider m_sliderBrush;
    public Slider m_sliderRubber;
    public Image m_imagePencilFill;
    public Image m_imageBrushFill;
    public Image m_imageRubberFill;
    public Image m_fishBg_0;
    #endregion
    public Sprite[] m_fishTemplateBgPic2D;//鱼背景图组
    public Texture2D[] m_fishTemplateFgPic2D;//鱼前景图组
    public Texture2D m_fishLineFramePic2D;
    public Texture2D m_foreGroundBgPic2D;//前景图
    public Texture2D[] m_brushPatternPic2D;//笔刷图案图片数组
    public Texture2D m_pencilPic2D;//铅笔图片
    public Texture2D m_rubberPic2D;//橡皮图片
    private Texture2D m_curBrushPatternPic2D;//当前笔刷图案图片
    private Texture2D m_curToolPic2D;//当前工具图片
    public int m_fishTemplateCount=0;
    public float m_fishLineFrameBlackValue=1;

    //[HideInInspector]
    //public float i, j, k, l;//调整显示图片的区域
    private float m_widthRatio;//宽比
    private float m_heightRatio;//高比
    private float m_left;//左边距
    private float m_down;//下边距
    private float m_textureWidth;
    private float m_textureHeight;
    private Rect m_drawRect;//画图区域
    private Vector2 m_startPos;
    private Vector2 m_finishPos;
    private bool m_isCanStart=true;//是否开始按下鼠标左键
    public bool m_isCanDraw = false;//是否可以画图
    public bool m_isShowTool = false;//是否显示工具图片

    private Vector2 m_touchPos;
    void Start()
    {
        m_palette = GetComponent<PaletteRU>();
        m_curTool = ToolType.Pencil;
        m_curBrush = BrushType.BrushHeart;
        m_buttonGroup[0].GetComponent<Image>().color = Color.yellow;
        m_buttonGroup[3].GetComponent<Image>().color = Color.red;
        m_left = ((float)Screen.width / 1920 * 1238);//(float)(Screen.width / i);
        m_down = ((float)Screen.height / 1080 * 654);//(float)(Screen.height / j);
        m_textureWidth = ((float)Screen.width / 1920 * 404);
        m_textureHeight = ((float)Screen.height / 1080 * 312);
        m_widthRatio = (m_textureWidth / m_foreGroundBgPic2D.width);
        m_heightRatio = (m_textureHeight / m_foreGroundBgPic2D.height);
        m_drawRect = new Rect(m_left, m_down, m_textureWidth, m_textureHeight);//new Rect(Screen.width / i, Screen.height / j, Screen.width / k, Screen.height / l);
        m_imagePencilFill.fillAmount = m_sliderPencil.value / 10;
        m_imageBrushFill.fillAmount = m_sliderBrush.value / 5;
        m_imageRubberFill.fillAmount = m_sliderRubber.value / 6;
        
    }
    void Update()
    {
        m_image_CurColor.color = m_curColor;
        m_fishBg_0.sprite = m_fishTemplateBgPic2D[m_fishTemplateCount];
        if (m_isCanDraw)
        {
            f_RunToolType();
        }
    }
    void OnGUI()
    {
        //GUI.DrawTexture(new Rect(Screen.width / i, Screen.height / j, Screen.width / k, Screen.height / l), m_foreGroundBgPic2D);
        GUI.DrawTexture(new Rect(m_left, Screen.height - m_textureHeight - m_down, m_textureWidth, m_textureHeight), m_foreGroundBgPic2D);

        //GUI.DrawTexture(new Rect(Screen.width / i, Screen.height / j, Screen.width / k, Screen.height / l), m_fishLineFramePic2D);
        GUI.DrawTexture(new Rect(m_left, Screen.height - m_textureHeight - m_down, m_textureWidth, m_textureHeight), m_fishLineFramePic2D);
        GUI.depth = 2;
        if (m_isShowTool)
        {
            switch (Config.m_curInputType)
            {
                case Config.InputType.MouseInput:
                    switch (m_curTool)
                    {
                        case ToolType.Pencil:
                            GUI.DrawTexture(new Rect(
                                (int)Input.mousePosition.x - (m_curToolPic2D.width / (int)m_sliderPencil.maxValue * (int)m_sliderPencil.value) / 2,
                                (int)(Screen.height - (int)Input.mousePosition.y - (m_curToolPic2D.height / (int)m_sliderPencil.maxValue * (int)m_sliderPencil.value) / 2),
                                m_curToolPic2D.width / (int)m_sliderPencil.maxValue * (int)m_sliderPencil.value,
                                m_curToolPic2D.height / (int)m_sliderPencil.maxValue * (int)m_sliderPencil.value),
                                m_curToolPic2D);
                            //GUI.DrawTexture(new Rect((int)Input.mousePosition.x, (int)(Screen.height - Input.mousePosition.y - m_curToolPic2D.height), m_curToolPic2D.width, m_curToolPic2D.height), m_curToolPic2D);
                            break;
                        case ToolType.Brush:
                            GUIUtility.RotateAroundPivot(180, new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y));
                            GUI.DrawTexture(new Rect((int)(Input.mousePosition.x - m_curToolPic2D.width / 2), (int)(Screen.height - Input.mousePosition.y - m_curToolPic2D.height / 2), m_curToolPic2D.width, m_curToolPic2D.height), m_curToolPic2D);

                            break;
                        case ToolType.Rubber:
                            GUI.DrawTexture(new Rect(
                                (int)Input.mousePosition.x - (m_curToolPic2D.width / (int)m_sliderRubber.maxValue * (int)m_sliderRubber.value) / 2,
                                (int)(Screen.height - (int)Input.mousePosition.y - (m_curToolPic2D.height / (int)m_sliderRubber.maxValue * (int)m_sliderRubber.value) / 2),
                                m_curToolPic2D.width / (int)m_sliderRubber.maxValue * (int)m_sliderRubber.value,
                                m_curToolPic2D.height / (int)m_sliderRubber.maxValue * (int)m_sliderRubber.value),
                                m_curToolPic2D);
                            break;
                    }
                    break;
                case Config.InputType.TouchInput:
                    switch (m_curTool)
                    {
                        case ToolType.Pencil:
                            GUI.DrawTexture(new Rect(
                                (int)m_touchPos.x - (m_curToolPic2D.width / (int)m_sliderPencil.maxValue * (int)m_sliderPencil.value) / 2,
                                (int)(Screen.height - (int)m_touchPos.y - (m_curToolPic2D.height / (int)m_sliderPencil.maxValue * (int)m_sliderPencil.value) / 2),
                                m_curToolPic2D.width / (int)m_sliderPencil.maxValue * (int)m_sliderPencil.value,
                                m_curToolPic2D.height / (int)m_sliderPencil.maxValue * (int)m_sliderPencil.value),
                                m_curToolPic2D);
                            //GUI.DrawTexture(new Rect((int)m_touchPos.x, (int)(Screen.height - m_touchPos.y - m_curToolPic2D.height), m_curToolPic2D.width, m_curToolPic2D.height), m_curToolPic2D);
                            break;
                        case ToolType.Brush:
                            GUIUtility.RotateAroundPivot(180, new Vector2(m_touchPos.x, Screen.height - m_touchPos.y));
                            GUI.DrawTexture(new Rect((int)(m_touchPos.x - m_curToolPic2D.width / 2), (int)(Screen.height - m_touchPos.y - m_curToolPic2D.height / 2), m_curToolPic2D.width, m_curToolPic2D.height), m_curToolPic2D);

                            break;
                        case ToolType.Rubber:
                            GUI.DrawTexture(new Rect(
                                (int)m_touchPos.x - (m_curToolPic2D.width / (int)m_sliderRubber.maxValue * (int)m_sliderRubber.value) / 2,
                                (int)(Screen.height - (int)m_touchPos.y - (m_curToolPic2D.height / (int)m_sliderRubber.maxValue * (int)m_sliderRubber.value) / 2),
                                m_curToolPic2D.width / (int)m_sliderRubber.maxValue * (int)m_sliderRubber.value,
                                m_curToolPic2D.height / (int)m_sliderRubber.maxValue * (int)m_sliderRubber.value),
                                m_curToolPic2D);
                            break;
                    }
                    break;
            }
            
        }
    }
    void f_SetToolType(ToolType tooltype)//切换工具类型
    {
        if (tooltype == m_curTool) return;
        else
            m_curTool = tooltype;
    }
    void f_SetBrushType(BrushType brushtype)//切换笔刷类型
    {
        if (brushtype == m_curBrush) return;
        else
            m_curBrush = brushtype;
    }
    void f_RunToolType()//运行当前工具
    {
        switch (m_curTool)
        {
            case ToolType.Pencil://铅笔
                m_markerSize = (int)m_sliderPencil.value;
                m_imagePencilFill.fillAmount = m_sliderPencil.value / 10;
                m_curToolPic2D = m_pencilPic2D;
                switch (Config.m_curInputType)
                {
                    case Config.InputType.MouseInput:
                        f_PencilAndRubber(m_curColor);
                        break;
                    case Config.InputType.TouchInput:
                        f_PencilAndRubberTouch(m_curColor);
                        break;
                }
                break;
            case ToolType.Brush:
                int number = (int)m_sliderBrush.value;
                m_imageBrushFill.fillAmount = m_sliderBrush.value / 5;
                switch (m_curBrush)
                {
                    case BrushType.BrushHeart:
                        m_curBrushPatternPic2D = m_brushPatternPic2D[number * 3 - 3];
                        break;
                    case BrushType.BrushStar:
                        m_curBrushPatternPic2D = m_brushPatternPic2D[number * 3 - 2];
                        break;
                    case BrushType.BrushPetal:
                        m_curBrushPatternPic2D = m_brushPatternPic2D[number * 3 - 1];
                        break;
                }
                m_curToolPic2D = m_curBrushPatternPic2D;
                switch (Config.m_curInputType)
                {
                    case Config.InputType.MouseInput:
                        f_DrawPattern(m_curBrushPatternPic2D);
                        break;
                    case Config.InputType.TouchInput:
                        f_DrawPatternTouch(m_curBrushPatternPic2D);
                        break;
                }
                break;
            case ToolType.Rubber://橡皮擦
                m_markerSize = (int)m_sliderRubber.value * 4;
                m_imageRubberFill.fillAmount = m_sliderRubber.value / 6;
                m_curToolPic2D = m_rubberPic2D;
                switch (Config.m_curInputType)
                {
                    case Config.InputType.MouseInput:
                        f_PencilAndRubber(Color.clear);
                        break;
                    case Config.InputType.TouchInput:
                        f_PencilAndRubberTouch(Color.clear);
                        break;
                }
                break;
        }
    }
    public void f_JudgeFishTypeRU(string str)//判断鱼底图的类型
    {
        switch (str)
        {
            case "天使鱼":
                m_fishTemplateCount = 0;
                break;
            case "泰坦炮弹":
                m_fishTemplateCount = 1;
                break;
            case "盘丽鱼":
                m_fishTemplateCount = 2;
                break;
        }
        f_ClearFishLineFrame(m_fishLineFramePic2D);
        //f_FishLineFrame(m_fishLineFramePic2D);
        f_FishLineFrameMask(m_fishLineFramePic2D);
    }
    private void f_DrawLine(int x1, int y1, int x2, int y2, Color color)//画线
    {
        int dx = x2 - x1;
        int dy = y2 - y1;
        int ux = (dx > 0) ? 1 : -1;
        int uy = (dy > 0) ? 1 : -1;
        int x = x1, y = y1, eps;
        x2 += ux; y2 += uy;
        eps = 0; dx = Mathf.Abs(dx); dy = Mathf.Abs(dy);
        if (dx > dy)
        {
            for (x = x1; x != x2; x += ux)
            {
                f_DrawPoint(x, y, color);
                eps += dy;
                if ((eps << 1) >= dx)
                {
                    y += uy;
                    eps -= dx;
                }
            }
        }
        if (dx <= dy)
        {
            for (y = y1; y != y2; y += uy)
            {
                f_DrawPoint(x, y, color);
                eps += dx;
                if ((eps << 1) >= dy)
                {
                    x += ux; eps -= dy;
                }
            }
        }
    }
    private void f_DrawPoint(int x, int y, Color color)//画点
    {
        for (int i = x - m_markerSize; i <= x + m_markerSize-1; i++)
        {
            for (int j = y - m_markerSize; j <= y + m_markerSize-1; j++)
            {
                if(Vector2.Distance(new Vector2(x,y),new Vector2(i, j))<m_markerSize)
                {
                    m_foreGroundBgPic2D.SetPixel(i, j, color);
                }
            }
        }
    }
    private void f_DrawPattern(Texture2D texture2d)//画图案
    {
        if (m_drawRect.Contains(Input.mousePosition))
        {
            m_isShowTool = true;
            Cursor.visible = false;
            if (Input.GetMouseButton(0))
            {
                m_startPos = (Vector2)Input.mousePosition;
                Color[] m_patternColor = texture2d.GetPixels(0, 0, texture2d.width, texture2d.height);
                int x = 0;
                //for (int i = 0; i < texture2d.width; i++)
                //{
                //    for (int j = 0; j < texture2d.height; j++)
                //    {
                //        if (m_patternColor[x].a > 0.1f)
                //        {
                //            m_foreGroundBgPic2D.SetPixel((int)((m_startPos.x - m_left) / m_widthRatio - texture2d.width / 2 + j), (int)((m_startPos.y - m_down) / m_heightRatio - texture2d.height / 2 + i), m_curColor);
                //        }
                //        x++;
                //    }
                //}
                for (int i = texture2d.width - 1; i >= 0; i--)//反
                {
                    for (int j = texture2d.height - 1; j >= 0; j--)
                    {
                        if (m_patternColor[x].a > 0.1f)
                        {
                            m_foreGroundBgPic2D.SetPixel((int)((m_startPos.x - m_left) / m_widthRatio - texture2d.width / 2 + j), (int)((m_startPos.y - m_down) / m_heightRatio - texture2d.height / 2 + i), m_curColor);
                        }
                        x++;
                    }
                }
                m_foreGroundBgPic2D.Apply();
            }
        }
        else
        {
            m_isShowTool = false;
            Cursor.visible = true;
        }
    }
    private void f_PencilAndRubber(Color color)//铅笔或者橡皮擦
    {
        if (m_drawRect.Contains(Input.mousePosition))
        {
            m_isShowTool = true;
            Cursor.visible = false;
            if (Input.GetMouseButton(0))
            {
                if (m_isCanStart)
                {
                    m_startPos = (Vector2)Input.mousePosition;
                    m_isCanStart = false;
                }
                m_finishPos = (Vector2)Input.mousePosition;
                f_DrawLine((int)((m_startPos.x - m_left) / m_widthRatio), (int)((m_startPos.y - m_down) / m_heightRatio), (int)((m_finishPos.x - m_left) / m_widthRatio), (int)((m_finishPos.y - m_down) / m_heightRatio), color);
                m_foreGroundBgPic2D.Apply();
                m_startPos = (Vector2)Input.mousePosition;
            }
            if (Input.GetMouseButtonUp(0))
            {
                m_isCanStart = true;
            }
        }
        else
        {
            m_isShowTool = false;
            Cursor.visible = true;
        }
    }
    private void f_DrawPatternTouch(Texture2D texture2d)//画图案touch
    {
        for (int t = 0; t < Input.touchCount; t++)
        {
            if (m_drawRect.Contains(Input.GetTouch(t).position))
            {
                m_touchPos = Input.GetTouch(t).position;
                m_isShowTool = true;
                Cursor.visible = false;
                if (Input.GetTouch(t).phase == TouchPhase.Began || Input.GetTouch(t).phase == TouchPhase.Moved || Input.GetTouch(t).phase == TouchPhase.Stationary)
                {
                    m_startPos = Input.GetTouch(t).position;
                    Color[] m_patternColor = texture2d.GetPixels(0, 0, texture2d.width, texture2d.height);
                    int x = 0;
                    //for (int i = 0; i < texture2d.width; i++)
                    //{
                    //    for (int j = 0; j < texture2d.height; j++)
                    //    {
                    //        if (m_patternColor[x].a > 0.1f)
                    //        {
                    //            m_foreGroundBgPic2D.SetPixel((int)((m_startPos.x - m_left) / m_widthRatio - texture2d.width / 2 + j), (int)((m_startPos.y - m_down) / m_heightRatio - texture2d.height / 2 + i), m_curColor);
                    //        }
                    //        x++;
                    //    }
                    //}
                    for (int i = texture2d.width - 1; i >= 0; i--)//反
                    {
                        for (int j = texture2d.height - 1; j >= 0; j--)
                        {
                            if (m_patternColor[x].a > 0.1f)
                            {
                                m_foreGroundBgPic2D.SetPixel((int)((m_startPos.x - m_left) / m_widthRatio - texture2d.width / 2 + j), (int)((m_startPos.y - m_down) / m_heightRatio - texture2d.height / 2 + i), m_curColor);
                            }
                            x++;
                        }
                    }
                    m_foreGroundBgPic2D.Apply();
                }
            }
            if (Input.GetTouch(t).phase == TouchPhase.Ended)
            {
                m_isShowTool = false;
                Cursor.visible = true;
            }
        }
    }
    private void f_PencilAndRubberTouch(Color color)//铅笔或者橡皮擦touch
    {
        for (int t = 0; t < Input.touchCount; t++)
        {
            if (m_drawRect.Contains(Input.GetTouch(t).position))
            {
                m_touchPos = Input.GetTouch(t).position;
                m_isShowTool = true;
                Cursor.visible = false;
                if (Input.GetTouch(t).phase == TouchPhase.Began || Input.GetTouch(t).phase == TouchPhase.Moved || Input.GetTouch(t).phase == TouchPhase.Stationary)
                {
                    if (m_isCanStart)
                    {
                        m_startPos = Input.GetTouch(t).position;
                        m_isCanStart = false;
                    }
                    m_finishPos = Input.GetTouch(t).position;
                    f_DrawLine((int)((m_startPos.x - m_left) / m_widthRatio), (int)((m_startPos.y - m_down) / m_heightRatio), (int)((m_finishPos.x - m_left) / m_widthRatio), (int)((m_finishPos.y - m_down) / m_heightRatio), color);
                    m_foreGroundBgPic2D.Apply();
                    m_startPos = Input.GetTouch(t).position;
                }
                if (Input.GetTouch(t).phase == TouchPhase.Ended)
                {
                    m_isCanStart = true;
                }
            }
            if (Input.GetTouch(t).phase == TouchPhase.Ended)
            {
                m_isShowTool = false;
                Cursor.visible = true;
            }
        }
    }
    private void f_ClearFishLineFrame(Texture2D texture2d)//清除鱼线框
    {
        int x = 0;
        Color[] m_Bg = m_fishTemplateFgPic2D[m_fishTemplateCount].GetPixels(0, 0, m_fishTemplateFgPic2D[m_fishTemplateCount].width, m_fishTemplateFgPic2D[m_fishTemplateCount].height);
        for (int j = 0; j < m_fishTemplateFgPic2D[m_fishTemplateCount].height; j++)
        {
            for (int i = 0; i < m_fishTemplateFgPic2D[m_fishTemplateCount].width; i++)
            {
                if (m_Bg[x].a > 0)
                {
                    texture2d.SetPixel(i, j, Color.clear);
                }
                x++;
            }
        }
        texture2d.Apply();
    }
    public void f_FishLineFrame(Texture2D texture2d)//画鱼线框
    {
        int x = 0;
        Color[] m_Bg = m_fishTemplateFgPic2D[m_fishTemplateCount].GetPixels(0, 0, m_fishTemplateFgPic2D[m_fishTemplateCount].width, m_fishTemplateFgPic2D[m_fishTemplateCount].height);
        for (int j = 0; j < m_fishTemplateFgPic2D[m_fishTemplateCount].height; j++)
        {
            for (int i = 0; i < m_fishTemplateFgPic2D[m_fishTemplateCount].width; i++)
            {
                if (m_Bg[x].r+ m_Bg[x].g + m_Bg[x].b <m_fishLineFrameBlackValue)
                {
                    texture2d.SetPixel(i, j, m_Bg[x]);
                }
                x++;
            }
        }
        texture2d.Apply();
    }

    public void f_FishLineFrameMask(Texture2D texture2d)//画鱼线框mask
    {
        int x = 0;
        Color[] m_Bg = m_fishTemplateFgPic2D[m_fishTemplateCount].GetPixels(0, 0, m_fishTemplateFgPic2D[m_fishTemplateCount].width, m_fishTemplateFgPic2D[m_fishTemplateCount].height);
        //for (int j = 0; j < m_fishTemplateFgPic2D[m_fishTemplateCount].height; j++)
        //{
        //    for (int i = 0; i < m_fishTemplateFgPic2D[m_fishTemplateCount].width; i++)
        //    {
        //        if (m_Bg[x].r+ m_Bg[x].g + m_Bg[x].b <m_fishLineFrameBlackValue)
        //        {
        //            texture2d.SetPixel(i, j, m_Bg[x]);
        //        }
        //        x++;
        //    }
        //}
        for (int j = m_fishTemplateFgPic2D[m_fishTemplateCount].height; j > 0; j--)//反
        {
            for (int i = m_fishTemplateFgPic2D[m_fishTemplateCount].width; i > 0; i--)
            {
                if (m_Bg[x].r + m_Bg[x].g + m_Bg[x].b <= m_fishLineFrameBlackValue && m_Bg[x].r <= 0.92f && m_Bg[x].g <= 0.92f && m_Bg[x].b <= 0.92f)
                {
                    texture2d.SetPixel(i, j, m_Bg[x]);
                }
                x++;
            }
        }
        texture2d.Apply();
    }

    public void f_FishLineFrameFanMask(Texture2D texture2d)//反画鱼线框
    {
        int x = 0;
        Color[] m_Bg = m_fishTemplateFgPic2D[m_fishTemplateCount].GetPixels(0, 0, m_fishTemplateFgPic2D[m_fishTemplateCount].width, m_fishTemplateFgPic2D[m_fishTemplateCount].height);
        for (int j = 0; j < m_fishTemplateFgPic2D[m_fishTemplateCount].height; j++)
        {
            for (int i = 0; i < m_fishTemplateFgPic2D[m_fishTemplateCount].width; i++)
            {
                if (m_Bg[x].r + m_Bg[x].g + m_Bg[x].b <= m_fishLineFrameBlackValue && m_Bg[x].r <= 0.92f && m_Bg[x].g <= 0.92f && m_Bg[x].b <= 0.92f)
                {
                    texture2d.SetPixel(i, j, m_Bg[x]);
                }
                x++;
            }
        }
        //for (int j = m_fishTemplateFgPic2D[m_fishTemplateCount].height; j > 0; j--)//反
        //{
        //    for (int i = m_fishTemplateFgPic2D[m_fishTemplateCount].width; i > 0; i--)
        //    {
        //        if (m_Bg[x].r + m_Bg[x].g + m_Bg[x].b < m_fishLineFrameBlackValue)
        //        {
        //            texture2d.SetPixel(i, j, m_Bg[x]);
        //        }
        //        x++;
        //    }
        //}
        texture2d.Apply();
    }
    public void f_CleanDrawBoard()//一键清除画板
    {
        for (int i = 0; i < m_foreGroundBgPic2D.width; i++)
        {
            for (int j = 0; j < m_foreGroundBgPic2D.height; j++)
            {
                m_foreGroundBgPic2D.SetPixel(i, j, Color.clear);
            }
        }
        m_foreGroundBgPic2D.Apply();
    }
    public void f_ButtonColorGroupInteractableNo()//颜色按钮不可点击
    {
        for(int i = 0; i < m_buttonColorGroup.Length; i++)
        {
            m_buttonColorGroup[i].GetComponent<Button>().enabled = false;
        }
    }
    public void f_ButtonColorGroupInteractableYes()//颜色按钮可以点击
    {
        for (int i = 0; i < m_buttonColorGroup.Length; i++)
        {
            m_buttonColorGroup[i].GetComponent<Button>().enabled = true;
        }
    }
    #region 按钮点击函数
    void f_HideImage_3()
    {
        m_image_Pencil.gameObject.SetActive(false);
        m_image_Brush.gameObject.SetActive(false);
        m_image_Rubber.gameObject.SetActive(false);
    }
    public void f_ButtonPencilOnClick()//点击铅笔按钮
    {
        f_HideImage_3();
        m_image_Pencil.gameObject.SetActive(true);
        f_RestoreDefaultColor(0,3);
        m_buttonGroup[0].GetComponent<Image>().color = Color.yellow;
        f_SetToolType(ToolType.Pencil);
    }
    public void f_ButtonBrushOnClick()//点击笔刷按钮
    {
        f_HideImage_3();
        m_image_Brush.gameObject.SetActive(true);
        f_RestoreDefaultColor(0, 3);
        m_buttonGroup[1].GetComponent<Image>().color = Color.yellow;
        f_SetToolType(ToolType.Brush);
    }
    public void f_ButtonRubberOnClick()//点击橡皮按钮
    {
        f_HideImage_3();
        m_image_Rubber.gameObject.SetActive(true);
        f_RestoreDefaultColor(0, 3);
        m_buttonGroup[2].GetComponent<Image>().color = Color.yellow;
        f_SetToolType(ToolType.Rubber);
    }
    public void f_ButtonBrushHeart()//点击心形笔刷
    {
        f_SetBrushType(BrushType.BrushHeart);
        f_RestoreDefaultColor(3, 6);
        m_buttonGroup[3].GetComponent<Image>().color = Color.red;
    }
    public void f_ButtonBrushStar()//点击星形笔刷
    {
        f_SetBrushType(BrushType.BrushStar);
        f_RestoreDefaultColor(3, 6);
        m_buttonGroup[4].GetComponent<Image>().color = Color.red;
    }
    public void f_ButtonBrushPetal()//点击花瓣笔刷
    {
        f_SetBrushType(BrushType.BrushPetal);
        f_RestoreDefaultColor(3, 6);
        m_buttonGroup[5].GetComponent<Image>().color = Color.red;
    }
    void f_RestoreDefaultColor(int start,int all)//恢复按钮默认颜色
    {
        for(int i = start; i < all; i++)
        {
            m_buttonGroup[i].GetComponent<Image>().color = Color.white;
        }
    }
    public void f_ButtonColor(Image i)//颜色按钮点击
    {
        m_curColor = i.color;
    }
    public void f_ButtonCustomColor()//自定义颜色按钮
    {
        m_palette.enabled = true;
        m_palette.m_isShowPalette = true;
        m_isCanDraw = false;
        f_ButtonColorGroupInteractableNo();
    }
    #endregion

}
