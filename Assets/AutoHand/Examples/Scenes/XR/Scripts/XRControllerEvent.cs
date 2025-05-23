using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Autohand.Demo {
    public class XRControllerEvent : MonoBehaviour
    {
        public XRHandControllerLink link;
        public CommonButton button;
        public UnityEvent Pressed;
        public UnityEvent Released;
        bool pressed = false;

        protected virtual void Start() {
            if (link == null) {
                link = GetComponentInParent<XRHandControllerLink>();
                if (link == null)
                    Debug.LogError("No XRHandControllerLink connected - input will not work", this);
            }
        }

        protected virtual void Update()
        {
            if (link == null)
                return;

            if (link.ButtonPressed(button) && !pressed)
            {
                Pressed?.Invoke();
                pressed = true;
            }
            else if (!link.ButtonPressed(button) && pressed)
            {
                Released?.Invoke();
                pressed = false;
            }
        }
    }
}