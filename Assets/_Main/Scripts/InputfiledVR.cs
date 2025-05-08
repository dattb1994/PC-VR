using DG.Tweening;
using TigerForge;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputfiledVR : UiElementVR
{
    public bool isActive;
    public Button btnClose;

    InputField _inputField;
    public InputField inputField
    {
        get
        {
            if(_inputField == null)
                _inputField = GetComponent<InputField>();
            return _inputField;
        }
    }
    // Use this for initialization
    public override void Start()
    {
        base.Start();
        btnClose.onClick.AddListener(() =>
        {
            EndInput();
        });
        EventManager.StartListening(EventKey.OnUIClick.ToString(), () =>
        {
            IClickable c = EventManager.GetData(EventKey.OnUIClick.ToString()) as IClickable;
            if (c != this as IClickable)
                EndInput();
        });
    }
    void EndInput()
    {
        EventSystem.current.SetSelectedGameObject(null);
        btnClose.gameObject.SetActive(false);
        isActive = false;
    }
    public override void DoOnClick()
    {
        base.DoOnClick();
        isActive = true;
        btnClose.gameObject.SetActive(true);
        
    }
    private void Update()
    {
        if (isActive)
        {
            if (!inputField.isFocused)
            {
                inputField.ActivateInputField();
            }
        }
    }
    public override void OnClick()
    {
        if (isActive)
            return;
        transform.DOScale(Vector3.one * .8f, .1f).onComplete += () =>
        {
            transform.DOScale(Vector3.one, 1).onComplete += () => DoOnClick();
        };
    }
}