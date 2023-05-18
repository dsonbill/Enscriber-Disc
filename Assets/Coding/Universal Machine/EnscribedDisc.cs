using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

namespace UniversalMachine
{
    public class EnscribedDisc : MonoBehaviour
    {
        public Transform Disc;


        public float Diameter;

        public string Enscription;

        public double RotationRate;

        public float Height;

        public float Rotation;

        public Vector3 Flow;

        public CurvedText Text;

        public Func<List<Particle>> Quanta;

        System.Random r = new System.Random();


        void Start()
        {
            Text.text = Enscription;
            Flow = Disc.up + Disc.right;
        }

        public int Number()
        {
            char[] enscr = Enscription.ToCharArray();

            int i = 0;
            foreach (char c in enscr)
            {
                i += c - 65;
            }

            return i;
        }

        public float Energy()
        {
            return Number() / Height * Diameter;
        }

        public Vector3 Offset()
        {
            return Flow * Rotation * Diameter;
        }

        void Update()
        {
            Disc.localScale = new Vector3((float)Diameter * 1.45f, (float)Height * 0.03f, (float)Diameter * 1.45f);
            Disc.localPosition = new Vector3(Disc.localPosition.x, (float)Height, Disc.localPosition.z);

            Disc.Rotate(new Vector3(0, 1, 0), (float)RotationRate);

            Rotation = Disc.localRotation.y;
        }

        void ApplyForce(Particle particle)
        {
            double energy = r.NextDouble() * Energy() / UniversalMachine.Particle.EnergeticResistance;
            Vector3 offset = Offset();

            //offset = new Vector3(offset.x * transform.up.x, offset.y * transform.up.y, offset.z * transform.up.z);

            particle.AddForce(transform.up * (float)energy, offset, Time.deltaTime);
        }

        void FixedUpdate()
        {
            foreach(Particle unit in Quanta())
            {
                ApplyForce(unit);
            }
        }
    }
}