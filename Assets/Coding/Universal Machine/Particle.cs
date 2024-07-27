using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace UniversalMachine
{
    public class Particle : MonoBehaviour
    {
        public const float Nearest = 1.4013e-25f;
        public const float EnergeticResistance = 1f;
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

        public enum StateFocus
        {
            Energy = 1,
            Position = -1
        }

        public enum EffectorFolding
        {
            Energy,
            Position,
            Force,
            Torque,
            NonSystematic
        }


        public Vector4 Ascription = new Vector4(1,1,1, 1);

        public Vector4 Assertion = new Vector4(1,1,1, 1);

        public Vector4 Conductance = new Vector4(1,1,1, 1);

        public Vector4 Attunement = new Vector4(1,1,1, 1);

        //Energetic, Positional, Spacial, Mechanical
        public Vector4 Descriptor = new Vector4();


        public Vector3 Definition { get; private set; } = Vector3.zero;

        public float Destination { get; private set; } = 0;

        public float Depth;


        public Vector3 Delta = Vector3.zero;


        public int SlamEvents = 0;

        public double SlamConsideration;


        System.Random r = new System.Random();

        public delegate void OnDestroyAction();
        public OnDestroyAction onDestroy;


        public Func<Vector3, Vector3, Vector3, Vector3> Project;

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

        public Func<double, double> QuaternaryReduction
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

        //List<Vector3> simulantVectors;

        public void Simulate(float temporal)
        {
            //Debug.Log("A: " + Position + " | " + Energy + " | " + Force + " | " + Torque);
            //float forceOffset = 1 / Force.w * deltaTime;
            //float torqueOffset = 1 / Torque.w  * deltaTime;

            Vector3 delta = Assert(Time.deltaTime);
            Vector3 vail = Ascribe(Time.deltaTime);
            Vector3 destinate = Conduct(Time.deltaTime);
            Vector3 juncture = Attune(Time.deltaTime);

            //Debug.Log("Delta: " + delta);
            //Debug.Log("Vail: " + vail);
            //Debug.Log("Destinate: " + destinate);
            //Debug.Log("Juncture: " + juncture);

            //simulantVectors = new List<Vector3>();

            //simulantVectors.Add(delta);
            //simulantVectors.Add(vail);
            //simulantVectors.Add(destinate);
            //simulantVectors.Add(juncture);

            //simulantVectors = ApplicateMultiplate(simulantVectors);

            //delta = simulantVectors[0];
            //vail = simulantVectors[1];
            //destinate = simulantVectors[2];
            //juncture = simulantVectors[3];

            
            Vector3 effect = new Vector3(
                delta.x + ((destinate.x * juncture.x) / vail.x),
                delta.y + ((destinate.y * juncture.y) / vail.y),
                delta.z + ((destinate.z * juncture.z) / vail.z)
                );

            effect = Applicate(effect);

            //Debug.Log("Effect: " + effect);

            Vector3 projection = Project(delta, effect, vail);

            //Debug.Log("Delta: " + delta);
            //Debug.Log("d*j :" + (destinate.x * juncture.x));
            //Debug.Log("Vail: " + vail);
            //Debug.Log("Effect: " + effect);
            //Debug.Log("Vail: " + vail);
            //Debug.Log("Projection: " + projection);

            projection = Applicate(projection);

            float disruption = temporal / Mathf.Abs(delta.magnitude) * Mathf.Abs(projection.magnitude);

            //Debug.Log("Projection: " + projection);
            //Debug.Log("Disruption: " + disruption);


            IndiscernProperty(
                delta,
                new Vector3(effect.x, effect.y, effect.z),
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



            Delta += effect + projection;
        }

        float Indiscern(float range)
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
            float dimensionality = Ascription.w + Attunement.w + Conductance.w;

            Vector3 ener = new Vector3(Ascription.x * Ascription.w, Ascription.y * Ascription.w, Ascription.z * Ascription.w);
            Ascription = new Vector4();

            Vector3 tor = new Vector3(Attunement.x * Attunement.w, Attunement.y * Attunement.w, Attunement.z * Attunement.w);
            Attunement = new Vector4();
            
            Vector3 four = new Vector3(Conductance.x * Conductance.w, Conductance.y * Conductance.w, Conductance.z * Conductance.w);
            Conductance = new Vector4();

            Vector4 specifics = Mul(tor, four);
            specifics = new Vector4(specifics.x * ener.x, specifics.y * ener.y, specifics.z * ener.z);
            specifics.w = dimensionality;
            return specifics;
        }

        public Vector4 AngularEffector()
        {
            float dimensionality = Ascription.w + Assertion.w + Conductance.w;

            Vector3 ener = new Vector3(Ascription.x * Ascription.w, Ascription.y * Ascription.w, Ascription.z * Ascription.w);
            Ascription = new Vector4();

            Vector3 pos = new Vector3(Assertion.x * Assertion.w, Assertion.y * Assertion.w, Assertion.z * Assertion.w);
            Assertion = new Vector4();

            Vector3 four = new Vector3(Conductance.x * Conductance.w, Conductance.y * Conductance.w, Conductance.z * Conductance.w);
            Conductance = new Vector4();

            Vector4 specifics = new Vector3(ener.x * four.x, ener.y * four.y, ener.z * four.z) + pos;
            specifics.w = dimensionality;
            return specifics;
        }

        public Vector4 ForceEffector()
        {
            float dimensionality = Ascription.w + Assertion.w + Attunement.w;

            Vector3 ener = new Vector3(Ascription.x * Ascription.w, Ascription.y * Ascription.w, Ascription.z * Ascription.w);
            Ascription = new Vector4();

            Vector3 pos = new Vector3(Assertion.x * Assertion.w, Assertion.y * Assertion.w, Assertion.z * Assertion.w);
            Assertion = new Vector4();

            Vector3 tor = new Vector3(Attunement.x * Attunement.w, Attunement.y * Attunement.w, Attunement.z * Attunement.w);
            Attunement = new Vector4();

            Vector4 specifics = new Vector3(ener.x * tor.x, ener.y * tor.y, ener.z * tor.z) + pos;
            specifics.w = dimensionality;
            return specifics;
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
            float energySolve = Descriptor.x * Ascription.w * deltaTime;
            Ascription = new Vector4(Ascription.x * energySolve, Ascription.y * energySolve, Ascription.z * energySolve, Ascription.w - energySolve);

            float positionSolve = Descriptor.y * Assertion.w * deltaTime;
            Assertion = new Vector4(Assertion.x * positionSolve, Assertion.y * positionSolve, Assertion.z * positionSolve, Assertion.w - positionSolve);

            float forceSolve = Descriptor.z * Conductance.w * deltaTime;
            Conductance = new Vector4(Conductance.x * forceSolve, Conductance.y * forceSolve, Conductance.z * forceSolve, Conductance.w - forceSolve);

            float torqueSolve = Descriptor.w * Attunement.w * deltaTime;
            Attunement = new Vector4(Attunement.x * torqueSolve, Attunement.y * torqueSolve, Attunement.z * torqueSolve, Attunement.w - torqueSolve);
        }

        public void Reduce(float delta, Func<double, double, double> algorithmic, Func<Particle> stream)
        {
            //Apply Descriptor Shaping
            //Energetic, Solid, Spacial, Mechanical

        }

        public void SetPosition()
        {
            transform.localPosition += Delta;
            Delta = Vector3.zero;
        }

        float pause;

        public void Define(float delta)
        {
            Destination += delta;
        }

        public void ParticleUpdate()
        {
            //if (pause < 1f)
            //{
            //    pause += Time.deltaTime;
            //    return;
            //}
            //pause = 0f;

            Simulate(Destination);
            
            //Clear Destination after simulation
            Destination = 0;
            //Advance(Time.deltaTime);

            

            //transform.localPosition = (Vector3)Move(Time.deltaTime);
        }

        void Start()
        {
            ParticleField.Instance.Attach(this);
            onDestroy += () => { ParticleField.Instance.Detach(this); };
        }

        void FixedUpdate()
        {
            SetPosition();
        }

        void OnDestroy()
        {
            onDestroy?.Invoke();
        }
    }
}