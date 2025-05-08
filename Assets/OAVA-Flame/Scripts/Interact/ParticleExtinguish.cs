//#if OAVA_IGNIS
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ignis
{
    public class ParticleExtinguish : MonoBehaviour
    {
        public int count = 0;

        [Tooltip("How large area can one particle extinquish")]
        public float particleExtinquishRadius = 0.1f;

        [Tooltip("How much the area is incremented if new area is not hit. (simulates water puddling/sliding on ground)")]
        public float incrementalPower = 0.0005f;

        private ParticleSystem _part;
        private ParticleSystem part
        {
            get
            { 
                if(_part == null)
                {
                    _part = GetComponent<ParticleSystem>();
                    if (_part == null)
                    {
                        Debug.LogError("ParticleExtinguish: No ParticleSystem found on this GameObject.");
                        return null;
                    }
                }
                return _part;
            }
        }
        private List<ParticleCollisionEvent> collisionEvents;

        public bool available = false;

        IEnumerator Start()
        {
            collisionEvents = new List<ParticleCollisionEvent>();
            yield return new WaitForSeconds(5);
            available = true;
        }

        void OnParticleCollision(GameObject other)
        {
            if (other == null) return;

            int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);
            count = numCollisionEvents;

            int i = 0;

            Ignis.FlammableObject flamObj = other.GetComponentInParent<Ignis.FlammableObject>();
            if (flamObj)
            {
                while (i < numCollisionEvents)
                {
                    Vector3 pos = collisionEvents[i].intersection;
                    flamObj.IncrementalExtinguish(pos, particleExtinquishRadius, incrementalPower);
                    i++;
                }
            }

        }
    }
}
//#endif
