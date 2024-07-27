using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniversalMachine
{
    public class SimpleShacle : MonoBehaviour
    {
        public GameObject Cylinder;
        public float Diameter;

        public float Binding;

        System.Random r = new System.Random();

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        public void Bind(Particle particle)
        {
            Vector2 localPosition = new Vector2(particle.Assertion.x, particle.Assertion.z) - new Vector2(Cylinder.transform.localPosition.x, Cylinder.transform.localPosition.z);
            Vector2 direction = localPosition.normalized;
            float distance = Mathf.Abs(localPosition.magnitude);

            float reduction = 1 / (Diameter * 2) * distance * Binding;

            //Debug.Log(reduction);

            Vector3 inwardForce = (-particle.Conduct(Time.deltaTime)) * reduction;

            particle.AddForce(-direction * inwardForce, Vector3.zero, Time.deltaTime);
        }
    }
}