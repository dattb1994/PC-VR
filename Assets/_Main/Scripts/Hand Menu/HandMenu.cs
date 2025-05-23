using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class HandMenu : MonoBehaviour
{
    public GameObject display;
    [SerializeField] Transform head;
    public float threshold = 0.7f; // Ngưỡng để xác định tay có hướng về đầu hay không
    public float faceDot;

    public Transform hub;
    public float distance = 1;

    public UnityEvent onFace, outFace;

    [SerializeField] bool isOnFace = false;
    public bool IsOnFace
    {
        get
        {
            return isOnFace;
        }
        set
        {
            if (value == isOnFace)
                return;

            isOnFace = value;
            if (isOnFace)
            {
                display.SetActive(true);
                onFace?.Invoke();
            }
            else
            {
                display.SetActive(false);
                outFace?.Invoke();
            }
        }
    }
    private void Start()
    {
        hub.transform.SetParent(null);
        hub.gameObject.SetActive(false);
    }
    void Update()
    {
        Vector3 toHead = (head.position - transform.position).normalized;
        faceDot = Vector3.Dot(transform.up, toHead); // so sánh hướng lòng bàn tay với hướng về đầu

        if (faceDot < threshold)
        {
            IsOnFace = true;
        }
        else
            IsOnFace = false;

        foreach (Transform t in transform)
        {
            if (t.gameObject.layer != LayerMask.NameToLayer("Default"))
                t.gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }
    public void ShowHub()
    {
        print(44444444);
        Vector3 forward = head.forward;
        forward.y = 0f; // bỏ độ nghiêng nhìn lên/xuống
        forward.Normalize();

        Vector3 targetPos = head.position + forward * distance;
        targetPos.y = head.position.y; // đặt ngang tầm mắt

        // 2. Đặt vị trí và xoay canvas
        hub.transform.position = targetPos;
        hub.transform.rotation = Quaternion.LookRotation(-forward); // Hướng về camera

        // 3. Bật Canvas
        hub.gameObject.SetActive(true);
    }
}
