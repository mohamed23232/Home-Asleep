using UnityEngine;

public class MobileControlsVisibility : MonoBehaviour
{
    void Awake()
    {
        bool isMobile = SystemInfo.deviceType == DeviceType.Handheld
                     || Input.touchSupported;
        gameObject.SetActive(isMobile);
    }
}
