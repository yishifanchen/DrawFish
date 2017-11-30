using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class SelectFishRU : MonoBehaviour {
    GameControllerRU m_gameController;
    public GameObject[] m_fish;
    public Sprite[] m_fishSprites;
    string m_fishMark;
    
    private int m_curFishGroup = 1;
    private int m_allFishGroup;
    private int m_remainderFishCount;
    Ray m_ray;
    RaycastHit m_hitObject;
	void Start()
    {
        m_gameController = GameObject.Find("GameController_RU").GetComponent<GameControllerRU>();
        m_allFishGroup = m_fishSprites.Length / 3;
        m_remainderFishCount = m_fishSprites.Length % 3;
        if (m_remainderFishCount > 0)
        {
            m_allFishGroup += 1;
        }
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
    public void f_LeftArrowOnClick()//左箭头
    {
        if (m_curFishGroup < m_allFishGroup)
        {
            m_curFishGroup++;
        }
        else
        {
            m_curFishGroup = 1;
        }
        for (int i = 0; i < 3; i++)
        {
            if (i + (m_curFishGroup - 1) * 3 > m_fishSprites.Length - 1)
            {
                m_fish[i].GetComponent<SpriteRenderer>().sprite = m_fishSprites[i - m_remainderFishCount];
            }
            else
            {
                m_fish[i].GetComponent<SpriteRenderer>().sprite = m_fishSprites[i + (m_curFishGroup - 1) * 3];
            }
        }
    }
    public void f_RightArrowOnClick()//右箭头
    {
        if (m_curFishGroup > 1)
        {
            m_curFishGroup--;
        }
        else
        {
            m_curFishGroup = m_allFishGroup;
        }
        for (int i = 0; i < 3; i++)
        {
            if(i + (m_curFishGroup - 1) * 3 > m_fishSprites.Length-1)
            {
                m_fish[i].GetComponent<SpriteRenderer>().sprite = m_fishSprites[i-m_remainderFishCount];
            }
            else
            {
                m_fish[i].GetComponent<SpriteRenderer>().sprite = m_fishSprites[i + (m_curFishGroup - 1) * 3];
            }
        }
    }
    void MouseInput()
    {
        if (Input.GetMouseButtonDown(0) && Config.m_rectRU.Contains(Input.mousePosition))
        {
            m_ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(m_ray, out m_hitObject))
            {
                m_fishMark = m_hitObject.collider.gameObject.GetComponent<SpriteRenderer>().sprite.name.ToString();
                this.gameObject.SendMessage("f_JudgeFishTypeRU", m_fishMark);
                m_gameController.m_image_MainSelect.gameObject.SetActive(false);
                m_gameController.m_image_Draw.gameObject.SetActive(true);
            }
        }
    }
    void TouchInput()
    {
        for (int i = 0; i < Input.touchCount; i++)//触控
        {
            if (Input.GetTouch(i).phase == TouchPhase.Began && Config.m_rectRU.Contains(Input.GetTouch(i).position))
            {
                m_ray = Camera.main.ScreenPointToRay(Input.GetTouch(i).position);
                if (Physics.Raycast(m_ray, out m_hitObject))
                {
                    m_fishMark = m_hitObject.collider.gameObject.GetComponent<SpriteRenderer>().sprite.name.ToString();
                    this.gameObject.SendMessage("f_JudgeFishTypeRU", m_fishMark);
                    m_gameController.m_image_MainSelect.gameObject.SetActive(false);
                    m_gameController.m_image_Draw.gameObject.SetActive(true);
                }
            }
        }
    }
}
