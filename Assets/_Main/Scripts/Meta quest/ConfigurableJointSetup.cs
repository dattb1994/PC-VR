using UnityEngine;

public class ConfigurableJointSetup : MonoBehaviour
{
    void Start()
    {
        ConfigurableJoint joint = GetComponent<ConfigurableJoint>();

        if (joint == null)
        {
            Debug.LogError("Thiếu Configurable Joint trên GameObject này!");
            return;
        }

        // Chỉ cho phép xoay quanh trục X
        joint.angularXMotion = ConfigurableJointMotion.Limited;
        joint.angularYMotion = ConfigurableJointMotion.Locked;
        joint.angularZMotion = ConfigurableJointMotion.Locked;

        // Thiết lập giới hạn góc X từ -5 đến 73 độ
        SoftJointLimit lowLimit = new SoftJointLimit();
        lowLimit.limit = -5f;
        joint.lowAngularXLimit = lowLimit;

        SoftJointLimit highLimit = new SoftJointLimit();
        highLimit.limit = 73f;
        joint.highAngularXLimit = highLimit;
    }
}
