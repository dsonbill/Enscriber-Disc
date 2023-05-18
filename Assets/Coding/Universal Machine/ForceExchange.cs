using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniversalMachine
{
    public class ForceExchange : MonoBehaviour
    {
        public double ContactDepth;
        public double ExchangerRatio;


        // Start is called before the first frame update
        public void Exchange(List<Particle>Particles)
        {
            foreach (Particle a in Particles)
            {
                foreach (Particle b in Particles)
                {
                    if (a == b) continue;

                    Vector3 pEn = b.Ascribe(Time.deltaTime) + a.Ascribe(Time.deltaTime);

                    Vector3 force = new Vector3(pEn.x * (float)ContactDepth * (float)ExchangerRatio,
                        pEn.y * (float)ContactDepth * (float)ExchangerRatio,
                        pEn.z * (float)ContactDepth * (float)ExchangerRatio);

                    Vector3 dir = (a.Assertion - b.Assertion).normalized;

                    float distance = Vector4.Distance(a.Assertion, b.Assertion);
                    distance = Mathf.Pow(distance, 2);

                    a.AddForce(force / distance, b.Assert(Time.deltaTime) + dir, Time.deltaTime);
                }
            }
        }

        // Update is called once per frame
        void Start()
        {
            
        }
    }
}