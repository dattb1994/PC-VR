using DG.Tweening;
using TigerForge;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Outline))]
public class UiElementVR : MonoBehaviour, IClickable
{
    public virtual void Start()
    {
        Outline outline = GetComponent<Outline>();
        BoxCollider collider = GetComponent<BoxCollider>();
        RectTransform rectTransform = GetComponent<RectTransform>();
        collider.size = new Vector3(rectTransform.rect.width, rectTransform.rect.height, .01f);

        if(outline != null)
        {
            outline.effectColor = Color.white;
            outline.effectDistance = new Vector2(2, -2);
        }
        

        EventManager.StartListening(EventKey.OnOverButton.ToString(), () =>
        {
            var c = EventManager.GetData(EventKey.OnOverButton.ToString()) as IClickable;

            if (outline != null)
                outline.enabled = (this == c);
        });
    }
    public virtual void OnClick()
    {
        transform.DOScale(Vector3.one * .8f, .1f).onComplete += () =>
        {
            transform.DOScale(Vector3.one, 1).onComplete += () => DoOnClick();
        };
    }
    public virtual void DoOnClick()
    {
        EventManager.EmitEventData(EventKey.OnUIClick.ToString(), this as IClickable);
    }
    public void OnExit()
    {

    }

    public void OnOver()
    {

    }
}