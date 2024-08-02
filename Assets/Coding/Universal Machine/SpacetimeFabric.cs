using System.Collections.Generic;
using UnityEngine;

namespace UniversalMachine
{
    public class SpacetimeFabric : MonoBehaviour
    {
        // Warping Vector Structure
        public struct WarpVector
        {
            public Vector3 Position;
            public Vector3 Direction;
            public float Magnitude;

            public WarpVector(Vector3 position, Vector3 direction, float magnitude)
            {
                Position = position;
                Direction = direction;
                Magnitude = magnitude;
            }
        }

        // List to store warping vectors
        private List<WarpVector> WarpVectors = new List<WarpVector>();

        // Consolidation Parameters
        public float ConsolidationRadius = 1.0f;
        public float ConsolidationThreshold = 0.5f;
        public float ConsolidationInterval = 0.5f;

        // Expostulant Affair
        public float ExpostulantAffair = 1f;

        // Spacetime Material
        public List<Renderer> Renderers;

        // Maximum number of warping vectors to send to the shader
        private const int MaxWarpingVectors = 100;

        private float lastConsolidationTime = 0f;

        public Vector3 Mul(Vector3 vector1, Vector3 vector2)
        {
            return new Vector3(
                vector1.x * vector2.x,
                vector1.y * vector2.y,
                vector1.z * vector2.z
                );
        }

        void FixedUpdate()
        {
            // 1. Update Warping Vectors (from particle expostulation)
            //UpdateWarpingVectors();

            // 2. Consolidate Warping Vectors (periodically)
            if (Time.time - lastConsolidationTime >= ConsolidationInterval)
            {
                ConsolidateWarpingVectors();
                lastConsolidationTime = Time.time;
            }

            // 3. Update Shader Properties (for visualization and particle positioning)
            UpdateSpacetimeShaderProperties();
        }

        // Method to add new warping vectors from particles
        public void AddWarpVector(Vector3 position, Vector3 direction, float magnitude)
        {
            WarpVectors.Add(new WarpVector(position, direction, magnitude));
        }

        // Method to update warping vectors based on particle expostulation
        public void UpdateWarpingVectors(Particle particle)
        {
            // TODO: Calculate expostulation value using particle.Expostulate(Time.deltaTime)
            float expostulationValue = particle.Expostulate(Time.deltaTime);

            // TODO: Calculate warping direction and magnitude based on Contact Theory principles
            Vector3 warpDirection = particle.Velocity.normalized * Mathf.Pow(Mul(particle.Velocity.normalized, particle.Velocity).magnitude, Time.deltaTime); // Example - Replace with your logic
            float warpMagnitude = expostulationValue * ExpostulantAffair; // Example - Replace with your logic

            // Add or update the warping vector for this particle
            AddWarpVector(particle.transform.position, warpDirection, warpMagnitude);
        }

        // Method to consolidate nearby warping vectors
        private void ConsolidateWarpingVectors()
        {
            // TODO: Implement your intelligent consolidation algorithm here
            // This is where you'll group and average warping vectors based on:
            // - ConsolidationRadius
            // - ConsolidationThreshold
            // - And your categorization logic to preserve important features

            // Temporary simple consolidation (replace with your algorithm)
            if (WarpVectors.Count > MaxWarpingVectors)
            {
                WarpVectors.RemoveRange(MaxWarpingVectors, WarpVectors.Count - MaxWarpingVectors);
            }
        }

        // Method to update shader properties
        public void UpdateParticleShaderProperties(Material particleMaterial)
        {
            // Prepare warping vector data for shaders
            Vector4[] warpingVectorsArray = new Vector4[MaxWarpingVectors];
            for (int i = 0; i < WarpVectors.Count && i < MaxWarpingVectors; i++)
            {
                warpingVectorsArray[i] = new Vector4(
                    WarpVectors[i].Position.x,
                    WarpVectors[i].Position.y,
                    WarpVectors[i].Position.z,
                    WarpVectors[i].Magnitude
                );
            }

            // Update Particle Shader
            particleMaterial.SetVectorArray("_WarpingVectors", warpingVectorsArray);
            particleMaterial.SetInt("_NumWarpingVectors", warpingVectorsArray.Length);
        }

        private void UpdateSpacetimeShaderProperties()
        {
            // Prepare warping vector data for shaders
            Vector4[] warpingVectorsArray = new Vector4[MaxWarpingVectors];
            for (int i = 0; i < WarpVectors.Count && i < MaxWarpingVectors; i++)
            {
                warpingVectorsArray[i] = new Vector4(
                    WarpVectors[i].Position.x,
                    WarpVectors[i].Position.y,
                    WarpVectors[i].Position.z,
                    WarpVectors[i].Magnitude
                );
            }

            // Update Spacetime Shader
            foreach(Renderer renderer in Renderers)
            {
                renderer.material.SetVectorArray("_WarpingVectors", warpingVectorsArray);
                renderer.material.SetInt("_NumWarpingVectors", warpingVectorsArray.Length);
            }
            
        }
        }
}