using System.Collections.Generic;
using UnityEngine;

namespace UniversalMachine
{
    public class LightSource : MonoBehaviour
    {
        // The strength of the light source as it applies a torque to particles
        //public float TorqueStrength = 1.0f;

        // The intensity of the light source and its potential
        public double Intensity;
        public double Potential;

        public Light Light;

        // Shader property names (make sure these match your shader)
        private const string LightTorqueProperty = "_LightTorque";

        // Update is called once per frame
        void Update()
        {
            // Update the light's visual properties
            Light.intensity = (float)Intensity;
            Light.range = (float)Potential;
        }

        public void UpdateParticle(Particle particle)
        {
            //Material particleMaterial = particle.GetComponent<Renderer>().material;

            //particleMaterial.SetFloat(LightTorqueProperty, TorqueStrength);
        }
    }
}