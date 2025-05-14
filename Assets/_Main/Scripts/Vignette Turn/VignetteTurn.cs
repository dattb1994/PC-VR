using Autohand.Demo;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VignetteTurn : Singleton<VignetteTurn>
{
    public Volume volume;
    Vignette vignette;
    public XRHandControllerLink turnController;

    [SerializeField] bool isActive = false;
    public bool IsActive
    {
        get => isActive;
        set
        {
            if (isActive == value)
                return;
            isActive = value;
            if (isActive)
            {
                Show();
            }
            else
            {
                Hide();
            }
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Awake()
    {
        base.Awake();
        if (volume.profile.TryGet(out vignette) == false)
        {
            Debug.LogError("Color Adjustments not found in Volume Profile!");
        }
    }
    private void Start()
    {
        IsActive = false;
    }
    public void Show()
    {
        vignette.intensity.value = 1;
    }
    public void Hide()
    {
        vignette.intensity.value = 0;
    }
    private void Update()
    {
        IsActive = turnController.GetAxis2D(Common2DAxis.primaryAxis).x != 0;
    }
}
