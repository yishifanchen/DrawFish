using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameControllerLU : MonoBehaviour {
    public Image m_image_MainSelect;
    public Image m_image_Draw;
    public Image m_image_Introduce;
    public Image m_image_Show;
    DrawLU m_draw;
    PaletteLU m_palette;
    public Texture2D m_generateTex2D;
    bool m_isShow;
    bool m_once=true;

    public GameObject[] m_fishType;
    public Material m_fishMat;
    public GameObject m_curFish;
    
    void Start()
    {
        m_draw = GetComponent<DrawLU>();
        m_palette = GetComponent<PaletteLU>();
        
    }
    void Update()
    {
        
        if (m_image_Draw.gameObject.activeSelf&&m_once)
        {
            m_draw.enabled = true;
            StartCoroutine(f_DelayDraw());
            m_once = false;
        }
        if (!m_image_Draw.gameObject.activeSelf)
        {
            m_draw.enabled = false;
            m_draw.m_isCanDraw = false;
            m_once = true;
        }
        if (m_image_Introduce.gameObject.activeSelf)
        {
            switch (Config.m_curInputType)
            {
                case Config.InputType.MouseInput:
                    if (Input.GetMouseButtonDown(0) && Config.m_rectLU.Contains(Input.mousePosition))
                    {
                        m_image_Draw.gameObject.SetActive(true);
                        m_image_Introduce.gameObject.SetActive(false);
                    }
                    break;
                case Config.InputType.TouchInput:
                    for (int i = 0; i < Input.touchCount; i++)//触控
                    {
                        if (Input.GetTouch(i).phase == TouchPhase.Began && Config.m_rectLU.Contains(Input.GetTouch(i).position))
                        {
                            m_image_Draw.gameObject.SetActive(true);
                            m_image_Introduce.gameObject.SetActive(false);
                        }
                    }
                    break;
            }
        }
        if (m_curFish)
        {
            m_curFish.transform.Rotate(Vector3.up*Time.deltaTime*20);
        }
    }
    void OnGUI()
    {
        if (m_isShow)
        {
            //GUI.DrawTexture(new Rect(Screen.width / m_draw.i, Screen.height / m_draw.j, Screen.width / m_draw.k, Screen.height / m_draw.l), m_generateTex2D);
        }
    }
    public void f_GenerateTex2D()//生成2D贴图
    {
        int x = 0;
        Color[] m_bg = m_draw.m_fishTemplateFgPic2D[m_draw.m_fishTemplateCount].GetPixels(0, 0, m_draw.m_fishTemplateFgPic2D[m_draw.m_fishTemplateCount].width, m_draw.m_fishTemplateFgPic2D[m_draw.m_fishTemplateCount].height);
        Color[] m_fg = m_draw.m_foreGroundBgPic2D.GetPixels(0, 0, m_draw.m_foreGroundBgPic2D.width, m_draw.m_foreGroundBgPic2D.height);
        m_generateTex2D.SetPixels(0, 0, m_generateTex2D.width, m_generateTex2D.height, m_bg);
        //for (int j = 0; j < m_draw.m_foreGroundBgPic2D.height; j++)
        //{
        //    for (int i = 0; i < m_draw.m_foreGroundBgPic2D.width; i++)
        //    {
        //        if (m_fg[x].a > 0)
        //        {
        //            m_generateTex2D.SetPixel(i, j, m_fg[x]);
        //        }
        //        x++;
        //    }
        //}
        for (int j = m_draw.m_foreGroundBgPic2D.height; j > 0; j--)//反
        {
            for (int i = m_draw.m_foreGroundBgPic2D.width; i > 0; i--)
            {
                if (m_fg[x].a > 0)
                {
                    m_generateTex2D.SetPixel(i, j, m_fg[x]);
                }
                x++;
            }
        }
        m_draw.f_FishLineFrameFanMask(m_generateTex2D);
        m_generateTex2D.Apply();
    }
    public void f_UnDoGenerateTex2D()//撤销生成2D贴图
    {
        for (int i = 0; i < m_draw.m_foreGroundBgPic2D.width; i++)
        {
            for (int j = 0; j < m_draw.m_foreGroundBgPic2D.height; j++)
            {
                m_generateTex2D.SetPixel(i, j, Color.clear);
            }
        }
        m_generateTex2D.Apply();
    }
    
    IEnumerator f_DelayDraw()
    {
        yield return new WaitForSeconds(.1f);
        m_draw.m_isCanDraw = true;
    }
    #region 按钮点击函数
    public void f_ButtonReturnToMain()//返回到主页面按钮
    {
        m_image_Draw.gameObject.SetActive(false);
        m_image_MainSelect.gameObject.SetActive(true);
        m_palette.enabled = false;
        m_draw.f_CleanDrawBoard();
    }
    public void f_ButtonReturnToDraw()//返回到画图按钮
    {
        m_image_Draw.gameObject.SetActive(true);
        m_image_Show.gameObject.SetActive(false);
        f_UnDoGenerateTex2D();
        m_isShow = false;
        m_curFish.SetActive(false);
        Destroy(m_curFish);
    }
    public void f_ButtonIntroduce()//介绍按钮
    {
        m_image_Draw.gameObject.SetActive(false);
        m_image_Introduce.gameObject.SetActive(true);
        m_palette.enabled = false;
    }
    public void f_ButtonShow()//展示按钮
    {
        m_image_Draw.gameObject.SetActive(false);
        m_image_Show.gameObject.SetActive(true);
        f_GenerateTex2D();
        m_isShow = false;
        m_fishType[m_draw.m_fishTemplateCount].transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material=m_fishMat;
        m_curFish=Instantiate(m_fishType[m_draw.m_fishTemplateCount], Config.m_showFishPos_LU, Quaternion.Euler(m_fishType[m_draw.m_fishTemplateCount].transform.rotation.eulerAngles.x, m_fishType[m_draw.m_fishTemplateCount].transform.rotation.eulerAngles.y-90, m_fishType[m_draw.m_fishTemplateCount].transform.rotation.eulerAngles.z))as GameObject;
        m_curFish.transform.localScale *= 40;
        m_fishMat.mainTexture = m_generateTex2D;
        m_curFish.SetActive(true);
        m_curFish.transform.localEulerAngles = new Vector3(0,0,180);
        m_palette.enabled = false;
    }
    public void f_ButtonSubmit()//提交按钮
    {
        m_image_Show.gameObject.SetActive(false);
        m_image_MainSelect.gameObject.SetActive(true);
        m_isShow = false;
        m_curFish.SetActive(false);
        Destroy(m_curFish);
        m_draw.f_CleanDrawBoard();
    }
    #endregion

}
