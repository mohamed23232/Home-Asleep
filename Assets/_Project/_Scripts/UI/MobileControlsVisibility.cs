#if !UNITY_EDITOR && UNITY_WEBGL
using System.Runtime.InteropServices;
#endif
using UnityEngine;

public class MobileControlsVisibility : MonoBehaviour
{
#if !UNITY_EDITOR && UNITY_WEBGL
    [DllImport("__Internal")]
    private static extern int IsMobileInput();
#endif

    void Awake()
    {
#if UNITY_EDITOR
        gameObject.SetActive(false);
#elif UNITY_WEBGL
        gameObject.SetActive(IsMobileInput() == 1);
#else
        gameObject.SetActive(SystemInfo.deviceType == DeviceType.Handheld);
#endif
    }
}
