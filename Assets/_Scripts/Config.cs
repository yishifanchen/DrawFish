using UnityEngine;
using System.Collections;

public class Config  {

    public static Rect m_rectLD = new Rect(0, 0, Screen.width / 2, Screen.height / 2);
    public static Rect m_rectRD = new Rect(Screen.width / 2, 0, Screen.width / 2, Screen.height / 2);
    public static Rect m_rectLU = new Rect(0, Screen.height / 2, Screen.width / 2, Screen.height / 2);
    public static Rect m_rectRU = new Rect(Screen.width / 2, Screen.height / 2, Screen.width / 2, Screen.height / 2);

    public static Vector3 m_showFishPos_LD = new Vector3(-480, -270, -1000);
    public static Vector3 m_showFishPos_RD = new Vector3(480, -270, -1000);
    public static Vector3 m_showFishPos_LU = new Vector3(-480, 270, -1000);
    public static Vector3 m_showFishPos_RU = new Vector3(480, 270, -1000);

    public enum InputType
    {
        MouseInput,
        TouchInput
    }
    public static InputType m_curInputType = InputType.TouchInput;
}
