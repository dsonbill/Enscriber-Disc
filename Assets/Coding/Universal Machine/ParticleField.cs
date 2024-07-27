using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Misc;
using System.Linq;
using System.Runtime.Remoting.Messaging;

namespace UniversalMachine
{
    public class ParticleField : MonoBehaviour
    {
        List<Particle> Simulands;

        List<int> PreviousParticles;
        int[] CurrentParticles;

        public int ParticlesPerUpdate;

        public System.Random r = new System.Random();

        public static ParticleField Instance;

        public SimpleShacle Shackle;

        public ForceExchange ForceExchanger;

        public void Attach(Particle particle)
        {
            Simulands.Add(particle);
        }

        public void Detach(Particle particle)
        {
            Simulands.Remove(particle);
        }

        // Start is called before the first frame update
        void Start()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            Simulands = new List<Particle>();
            CurrentParticles = new int[0];
            PreviousParticles = new List<int>();
        }

        int Target()
        {
            int x = Simulands.Count > 0 ? r.Next(0, Simulands.Count - 1) : -1;
            if (PreviousParticles.Contains(x))
                return Target();

            return x;
        }

        // Update is called once per frame
        void Update()
        {
            CurrentParticles = new int[ParticlesPerUpdate];

            int y;
            for (int i = 0; i < ParticlesPerUpdate; i++)
            {
                y = Target();

                if (y == -1)
                    return;

                if (CurrentParticles.Contains(y))
                    continue;

                Simulands[y].ParticleUpdate();

                Shackle.Bind(Simulands[y]);

                CurrentParticles[i] = y;
            }

            PreviousParticles = new List<int>(CurrentParticles);

            List<Particle> simulatedParticles = new List<Particle>();
            foreach (int i in CurrentParticles) { simulatedParticles.Add(Simulands[i]); }
            ForceExchanger.Exchange(simulatedParticles);
        }

        private void FixedUpdate()
        {
            foreach (Particle particle in Simulands)
            {
                particle.Define(Time.deltaTime);
            }
        }
    }
}