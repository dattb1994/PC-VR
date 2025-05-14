using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class XRRecenterSystem : Singleton<XRRecenterSystem>
{
    //void Update()
    //{
    //    // Kiểm tra nút menu trên controller trái hoặc phải
    //    if (IsMenuButtonPressed(XRNode.LeftHand) || IsMenuButtonPressed(XRNode.RightHand))
    //    {
    //        RecenterView();
    //    }
    //}

    //bool IsMenuButtonPressed(XRNode node)
    //{
    //    var device = InputDevices.GetDeviceAtXRNode(node);
    //    if (device.TryGetFeatureValue(CommonUsages.menuButton, out bool pressed))
    //        return pressed;
    //    return false;
    //}

    public void RecenterView()
    {
        var subsystems = new List<XRInputSubsystem>();
        SubsystemManager.GetSubsystems(subsystems);
        //SubsystemManager.GetInstances(subsystems);

        foreach (var subsystem in subsystems)
        {
            if (subsystem != null && subsystem.TryRecenter())
            {
                Debug.Log("Recenter thành công");
                return;
            }
        }

        Debug.LogWarning("Không thể recenter - sử dụng phương pháp thủ công");
        ManualRecenterFallback();
    }

    void ManualRecenterFallback()
    {
        // Triển khai phương pháp thủ công nếu hệ thống không hỗ trợ
        // Xem cách 2 bên dưới
    }
}