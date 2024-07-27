using System;
using System.Collections.Generic;
using UnityEngine;

namespace UniversalMachine
{
    public class ExistenceGradient : MonoBehaviour
    {
        //11:49 PM - 12/30/2021
        // Overwhelimg /blank mind/ -< -shift-  sense of repeating pattern
        //12:12 12/31/2021

        public Transform Primary;
        public Transform Secondary;

        public MeshRenderer PrimaryMesh;
        public MeshRenderer SecondaryMesh;

        public double Diameter;
        public double Distance;

        public Vector2 Position;

        public Vector3 Bifurcation;

        Vector3 PrimaryPosition;
        Vector3 PrimaryScale;

        Vector3 SecondaryPosition;
        Vector3 SecondaryScale;

        Quaternion Precession = Quaternion.identity;
        float precessionDelta = 0f;

        // x: Diameter, y: Height, z: Angular, w: Precessional
        public Vector4 Contact;

        public Gradient ColorGradient;
        public Gradient SecondaryColorGradient;

        public float Substantiation;

        public Func<List<Particle>> Quanta;

        public Func<Vector2> WellDistance;
        public Func<Vector3, Vector3> WellDirection;

        float pTime;
        float sTime;

        // Start is called before the first frame update
        void Start()
        {

            sTime = 1 ;
        }

        public Vector3 Mul(Vector3 vector1, Vector3 vector2)
        {
            return new Vector3(
                vector1.x * vector2.x,
                vector1.y * vector2.y,
                vector1.z * vector2.z
                );
        }

        void CalculateEnergy()
        {
            float positionOffset = Vector3.Distance(Primary.localPosition, Secondary.localPosition);
            float rotationalOffset = Quaternion.Angle(Primary.localRotation, Secondary.localRotation);

            Color offsetColor = PrimaryMesh.material.color - SecondaryMesh.material.color;
            float stateOffset = (offsetColor.r + offsetColor.g + offsetColor.b) / 3;

            Substantiation = (1 + positionOffset) * (1 / 360 * rotationalOffset);
        }


        void FixedUpdate()
        {
            pTime += Time.deltaTime;
            sTime -= Time.deltaTime;
            if (pTime >= 1)
            {
                pTime = 0;
            }
            if (sTime <= 0)
            {
                sTime = 1;
            }

            precessionDelta += Time.deltaTime;
            if (precessionDelta >= 1f)
            {
                precessionDelta = 0f;
            }


            CalculateEnergy();
            CalculateArena();

            Manifest();

            Friction();
        }

        void CalculateArena()
        {
            PrimaryPosition = new Vector3(Position.x, (float)(Distance / 2), Position.y);
            PrimaryScale = new Vector3((float)Diameter, (float)(Distance), (float)Diameter);

            SecondaryPosition = Mul(PrimaryPosition, new Vector3(1, Contact.y, 1));
            SecondaryScale = Mul(PrimaryScale, new Vector3(Contact.x, Contact.y, Contact.x));

            Precession = Quaternion.AngleAxis(1 / 360 * (Contact.z * precessionDelta), Bifurcation);
        }

        void Manifest()
        {
            Primary.localPosition = PrimaryPosition;
            Primary.localScale = PrimaryScale;

            Secondary.localPosition = SecondaryPosition;
            Secondary.localScale = SecondaryScale;
            Secondary.localRotation = Precession;

            PrimaryMesh.material.color = ColorGradient.Evaluate(pTime);
            SecondaryMesh.material.color = SecondaryColorGradient.Evaluate(sTime);
        }

        Vector3 Force(Particle particle)
        {
            Vector2 wellDistance = WellDistance();

            Vector3 deltaA = wellDistance.x * WellDirection(Primary.transform.position);
            Vector3 deltaB = wellDistance.y * WellDirection(Secondary.transform.position);

            Vector3 pPos = particle.Assert(Time.deltaTime);
            
            Vector3 adjPosA = pPos - deltaA;
            Vector3 adjPosB = pPos - deltaB;

            Vector3 adjPosBTrans = Secondary.TransformPoint(adjPosB);
            Vector3 adjPosBInvTrans = Primary.InverseTransformPoint(adjPosBTrans);

            Vector3 f = Mul((adjPosA - adjPosBInvTrans), new Vector3(0.5f, 0.5f, 0.5f));

            return f;
        }

        public void Friction()
        {
            foreach (Particle particle in Quanta())
            {
                Vector3 offset = particle.transform.position - Primary.position;
                float distance = offset.magnitude;
                Vector3 direction = offset.normalized;

                Vector3 torque = Bifurcation * distance * Substantiation;

                particle.AddForce(Force(particle), torque, Time.deltaTime);

                //Vector3 rot = new Vector3(Secondary.localRotation.x / 360, Secondary.localRotation.y / 360, Secondary.localRotation.z / 360);
                //Vector3 onset = distance +
                //    (Primary.localPosition + (new Vector3(Primary.localScale.x, Primary.localScale.y, Primary.localScale.z) / 2));

                //particle.AddForce(-onset.normalized * onset.magnitude, Vector3.zero, Time.deltaTime);

                //Left by that fucking bastard - I guess I was right after all!!
            }



        }
    }
}