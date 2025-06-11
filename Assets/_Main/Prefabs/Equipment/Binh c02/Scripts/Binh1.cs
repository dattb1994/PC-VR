using UnityEngine;

[RequireComponent(typeof(HoseParticle))]
public class Binh1 : FireEquipment
{
    public TypeBinh type;
    public Transform tracker;
    public Transform target;
    public AutoGrab autoGrab;
    public ChotBinh chotBinh;
    private ShakingChecker _shakeChecker;
    public ShakingChecker shakingChecker
    {
        get
        {
            if (_shakeChecker == null)
                _shakeChecker = GetComponent<ShakingChecker>();
            return _shakeChecker;
        }
    }

    [SerializeField] float angle;
    // Update is called once per frame
    public override void Start()
    {
        base.Start();
        Hose.IsOn = false;
    }
    void Update()
    {
        angle = Angle();
        if (tracker != null)
            SetTransformTarget();

        if (autoGrab != null)
        { 
            Hose.Spinkler = autoGrab.spinkler;
        }
        if (!Hose.IsOn)
        {
            if (!autoGrab.chotBinh)
                Hose.IsOn = true;
        }
    }
    public void SetTransformTarget()
    {
        target.transform.position = tracker.transform.position;
        target.transform.rotation = tracker.transform.rotation;
    }
    public float Angle()
    {
        return 90 - Vector3.Angle(transform.up, Vector3.up);
    }
}
public enum TypeBinh
{
    co2, water, flour
}
