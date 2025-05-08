using TigerForge;
using UnityEngine;

public class HeadsetInput : MonoBehaviour
{
    public Camera headsetCamera; // Camera từ headset
    [SerializeField] GameObject sphere; // Đối tượng Sphere
    public float clickThreshold = 2.0f; // Thời gian cần thiết để giả lập click

    private float hitTime = 0f; // Thời gian đã va chạm

    [SerializeField] float sphereSizeOnScreen = 1;
    [SerializeField] float sphereDistanceDefault = 5;


    IClickable _selecting = null;
    IClickable selecting
    {
        set
        {
            if (_selecting != value)
            {
                EventManager.EmitEventData(EventKey.OnOverButton.ToString(), value);
                _selecting = value;
            }
        }
        get { return _selecting; }
    }

    public GameObject isHit = null;

    Renderer renSpere;

    private void Start()
    {
        SetSpherePosDefault();
        sphere.SetActive(true);
        renSpere = sphere.GetComponent<Renderer>();
    }
    void Update()
    {
        if (RaycastClickable(out RaycastHit hit))
        {
            sphere.transform.position = hit.point;
            SetSizeSphere();
            renSpere.material.SetColor("_EmissionColor", Color.green);
        }
        else
        {
            SetSpherePosDefault();
            hitTime = 0;
            selecting = null;
            renSpere.material.SetColor("_EmissionColor", Color.yellow);
        }

        if (selecting != null)
        {
            if (hitTime >= clickThreshold)
            {
                selecting.OnClick();
                Debug.Log("Clicked on ");

                selecting = null;
                hitTime = 0;
            }
            else
                hitTime += Time.deltaTime;
        }
    }

    public bool RaycastClickable(out RaycastHit hit)
    {
        Ray ray = new Ray(headsetCamera.transform.position, headsetCamera.transform.forward);

        bool result = false;

        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);

        if (Physics.Raycast(ray, out hit, 200))
        {
            isHit = hit.collider.gameObject;

            var clickable = hit.collider.GetComponent<IClickable>();
            if (clickable != null)
            {
                selecting = clickable;
                result = true;
            }
        }
        else
        {
            isHit = null;

            selecting = null;
        } 
            
        return result;
    }

    void SetSizeSphere()
    {
        float distanceToCamera = Vector3.Distance(headsetCamera.transform.position, sphere.transform.position);
        float size = sphereSizeOnScreen * distanceToCamera;
        sphere.transform.localScale = new Vector3(size, size, size);

    }
    void SetSpherePosDefault()
    {
        sphere.transform.position = headsetCamera.transform.position + headsetCamera.transform.forward * sphereDistanceDefault;
        SetSizeSphere();
    }
}

public interface IClickable
{
    void OnClick();
    void OnOver();
    void OnExit();
}
