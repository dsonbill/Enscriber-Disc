using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace UniversalMachine
{
    public class Particle : MonoBehaviour
    {
        public const float Nearest = 1.4013e-25f;
        public const float EnergeticResistance = 4f;
        public const float FactualRestraint = 1f;
        public const float KineticEasing = 10f;
        public const float ContactWindow = 1f;

        public class Folding
        {
            public Vector3 Energy;
            public Vector3 Position;
            public Vector3 Force;
            public Vector3 Torque;
            public float Dimensionality;
        }

        public enum EffectorFolding
        {
            Energy,
            Position,
            Force,
            Torque,
            NonSystematic
        }

        // The Energetic, Positional, Spacial, and Mechanical properties of the particle
        // These represent each effect as it is folded into a dimensional axis of an arena
        // They are the total possible discernable properties of the particle, not the actual values
        // The w component of each vector is the temporal component, or how long it will take to discern the property
        // Ascription is the energy of the particle
        // Assertion is the position of the particle
        // Conductance is the force applied to the particle
        // Attunement is the torque applied to the particle
        public Vector4 Ascription = new Vector4(1,1,1, 1);

        public Vector4 Assertion = new Vector4(1,1,1, 1);

        public Vector4 Conductance = new Vector4(1,1,1, 1);

        public Vector4 Attunement = new Vector4(1,1,1, 1);

        //Current Dimensional folding of the particle in each respective axis of effect and affair
        //They represent how the particle is currently discerned in the arena
        //This influences the particle's behavior and interactions with other particles
        //Energetic, Positional, Spacial, Mechanical
        public Vector4 Reach = new Vector4();


        public Vector3 Definition { get; private set; } = Vector3.zero;

        public float Destination { get; private set; } = 0;

        // Contact Depth with other existential entities
        public float Depth;

        //Current DIsplacement delta built up from forces
        public Vector3 Delta = Vector3.zero;

        // The results of the projection of the particle's Delta onto the ascription plane
        public List<Vector3> Projections = new List<Vector3>();


        public int SlamEvents = 0;

        public double SlamConsideration;


        System.Random r = new System.Random();

        public delegate void OnDestroyAction();
        public OnDestroyAction onDestroy;

        // The function that will be used to project the particle's Delta onto the ascription plane
        // This will give us the force that is applied to the particle from other nearby ascriptions
        public Func<Vector3, Vector3, Vector3, Vector3> Project;

        public Vector3 Velocity
        {
            get { return transform.localPosition - lastPosition; }
        }

        Vector3 lastPosition = Vector3.zero;

        public Material material;

        public SpacetimeFabric substrate;

        // This function represents the particle's loss of energy over time to the environment
        public Func<double, double> PrimaryReduction
        {
            get
            {
                return new Func<double, double>((regionalArea) =>
                 {
                     double syphon = 1 / ((Vector3)Ascription).magnitude * (Depth / regionalArea);

                     return syphon * ContactWindow;
                 });
            }
        }

        // This function represents the particle;s loss of positionment over time to the environment
        public Func<double, double> SecondaryReduction
        {
            get
            {
                return new Func<double, double>((regionalArea) =>
                {
                    double syphon = 1 / ((Vector3)Assertion).magnitude * Depth * regionalArea;
                    
                    return syphon * ContactWindow;
                });
            }
        }

        // This function represents the particle's loss of force over time to the environment
        public Func<double, double> TertiaryReduction
        {
            get
            {
                return new Func<double, double>((regionalArea) =>
                {
                    double fAvg = (Conductance.x + Conductance.y + Conductance.z) / 3;
                    double syphon = 1 / fAvg * (Depth / regionalArea);

                    return syphon * ContactWindow;
                });
            }
        }

        // This function represents the particle's loss of torque over time to the environment
        public Func<double, double> QuaternaryReduction
        {
            get
            {
                return new Func<double, double>((regionalArea) =>
                {
                    double fAvg = (Attunement.x + Attunement.y +Attunement.z) / 3;
                    double syphon = 1 / fAvg * (Depth / regionalArea);

                    return syphon * ContactWindow;
                });
            }
        }

        

        public float GetDiscernmentRatio(float discernment, float temporal)
        {
            return discernment * temporal;
        }

        public void AddForce(Vector3 f, Vector3 point, float temporal)
        {
            Vector3 contactForce;
            float energyMagnitude = Ascribe(temporal).magnitude;

            if (energyMagnitude == 0)
            {
                contactForce = f / KineticEasing * Depth;
            }
            else
            {
                contactForce = f / (KineticEasing * (energyMagnitude / EnergeticResistance)) * Depth;
            }
 

            float totalDiscernment = Assertion.w * Ascription.w * Mathf.Pow(Attunement.w, 2) * Mathf.Pow(Conductance.w, 3);
            float discernmentRatio = GetDiscernmentRatio(totalDiscernment, temporal);


            Vector3 pf = Conduct(temporal);
            Conductance = new Vector4(
                pf.x + contactForce.x,
                pf.y + contactForce.y,
                pf.z + contactForce.z,
                temporal);

            Vector3 pt = Attune(temporal);
            Attunement = new Vector4(
                pt.x + point.x,
                pt.y + point.y,
                pt.z + point.z,
                temporal);
        }

        

        public Vector4 Cross(Vector4 vector1, Vector4 vector2)
        {
            return new Vector4(
                vector1.y * vector2.z - vector1.z * vector2.y,
                vector1.z * vector2.x - vector1.x * vector2.z,
                vector1.x * vector2.y - vector1.y * vector2.x,
                vector1.w * vector2.w);
        }

        public Vector3 Mul(Vector3 vector1, Vector3 vector2)
        {
            return new Vector3(
                vector1.x * vector2.x,
                vector1.y * vector2.y,
                vector1.z * vector2.z
                );
        }

        public float GetFactual(Vector4 detail, float temporal)
        {
            float discernment = temporal;

            return discernment / FactualRestraint;
        }

        public Vector3 Ascribe(float temporal)
        {
            float factual = GetFactual(Ascription, temporal);
            Vector3 pEnergy = new Vector3(
                Ascription.x * factual,
                Ascription.y * factual,
                Ascription.z * factual
                );
            return pEnergy;
        }

        public Vector3 Assert(float temporal)
        {
            float factual = GetFactual(Assertion, temporal);
            Vector3 pPosition =  new Vector3(
                Assertion.x * factual,
                Assertion.y * factual,
                Assertion.z * factual
                );
            return pPosition;
        }

        public Vector3 Conduct(float temporal)
        {
            float factual = GetFactual(Conductance, temporal);
            Vector3 pForce = new Vector3(
                Conductance.x * factual,
                Conductance.y * factual,
                Conductance.z * factual
                );
            return pForce;
        }

        public Vector3 Attune(float temporal)
        {
            float factual = GetFactual(Attunement, temporal);
            Vector3 pTorque = new Vector3(
                Attunement.x * factual,
                Attunement.y * factual,
                Attunement.z * factual
                );
            return pTorque;
        }

        public Vector3 Assertability()
        {
            return new Vector3(Assertion.x * Assertion.w, Assertion.y * Assertion.w, Assertion.z * Assertion.w);
        }

        public Vector3 Ascribability()
        {
            return new Vector3(Ascription.x * Ascription.w, Ascription.y * Ascription.w, Ascription.z * Ascription.w);
        }

        public Vector3 Conduction()
        {
            return new Vector3(Conductance.x * Conductance.w, Conductance.y * Conductance.w, Conductance.z * Conductance.w);
        }

        public Vector3 Attuning()
        {
            return new Vector3(Attunement.x * Attunement.w, Attunement.y * Attunement.w, Attunement.z * Attunement.w);
        }
        
        public Vector4 Applicate(Vector4 subject)
        {
            Vector4 adj;
            float x = 0f;
            float y = 0f;
            float z = 0f;
            float w = 0f;

            if (subject.x == float.NegativeInfinity)
            {
                x = -float.MaxValue;
            }
            else if (subject.x == float.PositiveInfinity)
            {
                x = float.MaxValue;
            }
            else if (float.IsNaN(subject.x))
            {
                x = 0f;
            }
            else
            {
                x = subject.x;
            }

            if (subject.y == float.NegativeInfinity)
            {
                y = -float.MaxValue;
            }
            else if (subject.y == float.PositiveInfinity)
            {
                y = float.MaxValue;
            }
            else if (float.IsNaN(subject.y))
            {
                y = 0f;
            }
            else
            {
                y = subject.y;
            }
            
            if (subject.z == float.NegativeInfinity)
            {
                z = -float.MaxValue;
            }
            else if (subject.z == float.PositiveInfinity)
            {
                z = float.MaxValue;
            }
            else if (float.IsNaN(subject.z))
            {
                z = 0f;
            }
            else
            {
                z = subject.z;
            }

            if (subject.w == float.NegativeInfinity)
            {
                w = -float.MaxValue;
            }
            else if (subject.w == float.PositiveInfinity)
            {
                w = float.MaxValue;
            }
            else if (float.IsNaN(subject.w))
            {
                w = 0f;
            }
            else
            {
                w = subject.w;
            }

            adj = new Vector4(x, y, z, w);

            return adj;
        }

        List<Vector3> ApplicateMultiplate(List<Vector3> applicants)
        {
            List<Vector3> limit = new List<Vector3>();
            foreach (Vector3 applicant in applicants) { limit.Add(Applicate(applicant)); }

            return limit;
        }

        public void Simulate(float temporal)
        {
            Vector3 delta = Assert(Time.deltaTime);
            Vector3 vail = Ascribe(Time.deltaTime);
            Vector3 destinate = Conduct(Time.deltaTime);
            Vector3 juncture = Attune(Time.deltaTime);

            Vector3 applicant = new Vector3(
                delta.x * ((destinate.x * juncture.x) / (vail.x / EnergeticResistance)),
                delta.y * ((destinate.y * juncture.y) / (vail.y / EnergeticResistance)),
                delta.z * ((destinate.z * juncture.z) / (vail.z / EnergeticResistance))
                );

            applicant = Applicate(applicant);

            Vector3 projection = Project(delta, applicant, vail);
            projection = Applicate(projection);

            Projections.Add(projection);

            float disruption = temporal / Mathf.Abs(delta.magnitude) * Mathf.Abs(projection.magnitude);

            IndiscernProperty(
                delta,
                new Vector3(applicant.x, applicant.y, applicant.z),
                temporal - disruption,
                (v) => { Assertion = v; },
                () => { return Assertion; });

            IndiscernProperty(
                destinate,
                Vector3.zero,
                temporal,
                (v) => { Conductance = v; },
                () => { return Conductance; });

            IndiscernProperty(
                juncture,
                Vector3.zero,
                temporal,
                (v) => { Attunement = v; },
                () => { return Attunement; });
        }

        public float Indiscern(float range)
        {
            return (r.Next(-10, 10) * (float)r.NextDouble() * range) / 10;
        }

        public void IndiscernProperty(Vector3 discernment, Vector3 resultant, float temporal, Action<Vector4> record, Func<Vector4> describe)
        {
            //Vector3 contained = ((Vector3)describe()) - discernment;

            Vector3 delta = resultant - discernment;

            Vector4 indiscernment = new Vector4(
                resultant.x + Indiscern(delta.magnitude),
                resultant.y + Indiscern(delta.magnitude),
                resultant.z + Indiscern(delta.magnitude),
                -temporal);

            record(describe() - indiscernment);
        }
        
        public void Fold(EffectorFolding target, Vector3 foldingDelta)
        {
            switch (target)
            {
                case EffectorFolding.Energy:
                    Ascription = EnergyEffector(foldingDelta);
                    break;
                case EffectorFolding.Position:
                    Assertion = PositionEffector();
                    break;
                case EffectorFolding.Torque:
                    Attunement = AngularEffector();
                    break;
                case EffectorFolding.Force:
                    Conductance = ForceEffector();
                    break;
                case EffectorFolding.NonSystematic:
                    Expostulate(Time.deltaTime);
                    break;
            }
        }

        public Folding Postulate()
        {
            float Dimensionality = Ascription.w + Assertion.w + Attunement.w + Conductance.w;

            Vector3 ener = new Vector3(Ascription.x * Ascription.w, Ascription.y * Ascription.w, Ascription.z * Ascription.w);
            Ascription = new Vector4();

            Vector3 pos = new Vector3(Assertion.x * Assertion.w, Assertion.y * Assertion.w, Assertion.z * Assertion.w);
            Assertion = new Vector4();

            Vector3 tor = new Vector3(Attunement.x * Attunement.w, Attunement.y * Attunement.w, Attunement.z * Attunement.w);
            Attunement = new Vector4();

            Vector3 four = new Vector3(Conductance.x * Conductance.w, Conductance.y * Conductance.w, Conductance.z * Conductance.w);
            Conductance = new Vector4();

            Folding information = new Folding();
            information.Energy = ener;
            information.Position = pos;
            information.Torque = tor;
            information.Force = four;
            
            return information;
        }

        public Vector4 EnergyEffector(Vector3 effectorDelta)
        {
            float dimensionality = (Assertion.w + Attunement.w + Conductance.w) / 3 * effectorDelta.magnitude;

            Vector3 pos = new Vector3(
                Assertion.x * Assertion.w + effectorDelta.x,
                Assertion.y * Assertion.w + effectorDelta.y,
                Assertion.z * Assertion.w + effectorDelta.z
                );

            Vector3 tor = new Vector3(
                Attunement.x * Attunement.w / effectorDelta.x,
                Attunement.y * Attunement.w / effectorDelta.y,
                Attunement.z * Attunement.w / effectorDelta.z
                );

            Vector3 forc = new Vector3(
                Conductance.x * Conductance.w - effectorDelta.x,
                Conductance.y * Conductance.w - effectorDelta.y,
                Conductance.z * Conductance.w - effectorDelta.z
                );

            float posDisc = Assertion.w / ((Vector3)Assertion).magnitude * pos.magnitude;
            Assertion = Assertion - new Vector4(pos.x, pos.y, pos.z, posDisc);

            float torDisc = Attunement.w / ((Vector3)Attunement).magnitude * tor.magnitude;
            Attunement = Attunement - new Vector4(tor.x, tor.y, tor.z, torDisc);

            float forDisc = Conductance.w / ((Vector3)Conductance).magnitude * forc.magnitude;
            Conductance = Conductance - new Vector4(forc.x, forc.y, forc.z, forDisc);

            Vector4 specifics =  pos + (Mul(tor, forc));
            specifics.w = dimensionality;
            return specifics;
        }

        public Vector4 PositionEffector()
        {
            // 1. Calculate the overall "dimensionality" of the particle
            float dimensionality = Ascription.w + (Attunement.w * Mathf.PI * Conductance.w);

            // 2. Combine Conductance and Attunement to calculate the effect of torque
            Vector3 torqueEffect = Mul(Attunement, Conductance);

            // 3. Scale the torque effect by the particle's energy potential (Ascription)
            torqueEffect /= new Vector3(Ascription.x, Ascription.y, Ascription.z).magnitude; //  Scaling based on Ascription

            // 4. Combine the scaled torque effect with the current position (Assertion)
            //   - This represents how force and torque influence position in spacetime.
            Vector4 newPosition = Assertion - new Vector4(torqueEffect.x, torqueEffect.y, torqueEffect.z, Assertion.w);

            // 5. Adjust the "dimensionality" of the position
            //    - This ensures that the uncertainty in position is correctly represented.
            newPosition.w = dimensionality * 0.5f; // Adjust scaling factor as needed 

            return newPosition;
        }

        public Vector4 AngularEffector()
        {
            // 1. Calculate the overall "dimensionality" of the particle
            float dimensionality = Ascription.w + (Attunement.w * Mathf.PI * Conductance.w);

            // 2. Combine the particle's energy and force to determine the "rotational potential"
            Vector3 rotationalPotential = Mul(Ascription, Conductance);

            // 3. Scale the "rotational potential" based on the particle's position (Assertion)
            rotationalPotential *= new Vector3(Assertion.x, Assertion.y, Assertion.z).magnitude;

            // 4. Update the Attunement vector, incorporating the "rotational potential"
            Vector4 newAttunement = Attunement - new Vector4(rotationalPotential.x, rotationalPotential.y, rotationalPotential.z, Attunement.w);

            // 5. Adjust the "dimensionality" of the attunement
            newAttunement.w = dimensionality * 0.5f;

            return newAttunement;
        }

        public Vector4 ForceEffector()
        {
            // 1. Calculate the overall "dimensionality" of the particle
            float dimensionality = Ascription.w + (Attunement.w * Mathf.PI * Conductance.w);

            // 2. Combine the particle's position and rotational influence to determine the "forcehood" of the position
            Vector3 forcehoodPosition = Mul(Assertion, Attunement);

            // 3. Scale the "forcehood" of the position based on the particle's energy potential
            forcehoodPosition *= new Vector3(Ascription.x, Ascription.y, Ascription.z).magnitude;

            // 4. Update the Conductance vector, incorporating the "forcehood" of the position
            Vector4 newForce = Conductance - new Vector4(forcehoodPosition.x, forcehoodPosition.y, forcehoodPosition.z, Conductance.w);

            // 5. Adjust the "dimensionality" of the force
            newForce.w = dimensionality * 0.5f;

            return newForce;
        }

        //ReinitializationalParamaterlessFunctionalitySystem
        //A.K.A. Role-Playing File System
        //A.K.A. Regional Protection Forwarding Service
        //A.K.A. Relentless Persona Friction State
        //A.K.A. Only William Really Knows What It Does! Till Now :)
        public float Expostulate(float deltaTime)
        {
            float dimensionality = Ascription.w + Assertion.w + Attunement.w + Conductance.w;

            float primordials = Mathf.Pow(Ascription.x, deltaTime);
            primordials += Mathf.Pow(Ascription.y, deltaTime);
            primordials += Mathf.Pow(Ascription.z, deltaTime);
            
            primordials += Mathf.Pow(Assertion.x, deltaTime);
            primordials += Mathf.Pow(Assertion.y, deltaTime);
            primordials += Mathf.Pow(Assertion.z, deltaTime);

            primordials += Mathf.Pow(Attunement.x, deltaTime);
            primordials += Mathf.Pow(Attunement.y, deltaTime);
            primordials += Mathf.Pow(Attunement.z, deltaTime);

            primordials += Mathf.Pow(Conductance.x, deltaTime);
            primordials += Mathf.Pow(Conductance.y, deltaTime);
            primordials += Mathf.Pow(Conductance.z, deltaTime);

            primordials /= Mathf.Pow(deltaTime, 3);
            primordials *= Mathf.Pow(dimensionality, 5);

            primordials *= Mathf.Pow(1, deltaTime);

            return primordials;
        }

        public void Advance(float deltaTime)
        {
            float energySolve = Reach.x * Ascription.w * deltaTime;
            Ascription = new Vector4(Ascription.x * energySolve, Ascription.y * energySolve, Ascription.z * energySolve, Ascription.w - energySolve);

            float positionSolve = Reach.y * Assertion.w * deltaTime;
            Assertion = new Vector4(Assertion.x * positionSolve, Assertion.y * positionSolve, Assertion.z * positionSolve, Assertion.w - positionSolve);

            float forceSolve = Reach.z * Conductance.w * deltaTime;
            Conductance = new Vector4(Conductance.x * forceSolve, Conductance.y * forceSolve, Conductance.z * forceSolve, Conductance.w - forceSolve);

            float torqueSolve = Reach.w * Attunement.w * deltaTime;
            Attunement = new Vector4(Attunement.x * torqueSolve, Attunement.y * torqueSolve, Attunement.z * torqueSolve, Attunement.w - torqueSolve);
        }

        public void Reduce(float delta, Func<double, double, double> algorithmic, Func<Particle> stream)
        {
            //Apply Descriptor Shaping
            //Energetic, Solid, Spacial, Mechanical

        }

        public void SetPosition(Vector3 deltaPosition)
        {
            lastPosition = transform.localPosition;
            transform.localPosition += deltaPosition;
        }

        public void Define(float delta)
        {
            Destination += delta;
        }

        void Start()
        {
            material = GetComponent<Renderer>().material;
            ParticleField.Instance.Attach(this);
            onDestroy += () => { ParticleField.Instance.Detach(this); };
        }

        void Update()
        {
            material.SetFloat("_ParticleDepth", Depth);
            material.SetFloat("_ParticleReach", Reach.magnitude);
            material.SetVector("_ParticleAscription", Ascription);
            material.SetVector("_ParticleDelta", Delta); // New line for Delta
        }

        void FixedUpdate()
        {
            // This is where the magic happens
            Simulate(Destination);
            Destination = 0;

            // Apply projections to the particle's Delta
            Vector3 totalProjection = Vector3.zero;
            foreach (Vector3 projection in Projections)
            {
                totalProjection += projection;
            }
            Projections.Clear(); // Clear the list after applying

            Delta += totalProjection;  // Add the combined projection to Delta
        }

        void OnDestroy()
        {
            onDestroy?.Invoke();
        }
    }
}