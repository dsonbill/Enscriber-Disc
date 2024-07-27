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

        public List<Transform> CharacterTransforms = new List<Transform>();
        public List<CurvedText> CharacterTexts = new List<CurvedText>();

        public Func<List<Particle>> Quanta;

        System.Random r = new System.Random();

        void Start()
        {
            // Create transforms and text objects for each character
            CreateCharacterObjects();

            // Set initial flow direction
            Flow = Disc.up + Disc.right;
        }

        void CreateCharacterObjects()
        {
            // Clear existing character objects
            foreach (Transform characterTransform in CharacterTransforms)
            {
                Destroy(characterTransform.gameObject);
            }
            CharacterTransforms.Clear();
            CharacterTexts.Clear();

            // Create character objects for each character in the inscription
            char[] characters = Enscription.ToCharArray();
            float characterSpacing = Diameter / characters.Length;
            float currentPosition = -Diameter / 2f;

            for (int i = 0; i < characters.Length; i++)
            {
                // Create a new transform for the character
                GameObject characterObject = new GameObject($"Character_{i}");
                characterObject.transform.parent = Disc;
                characterObject.transform.localPosition = new Vector3(currentPosition, 0f, 0f);
                CharacterTransforms.Add(characterObject.transform);

                // Create a new CurvedText for the character
                CurvedText characterText = characterObject.AddComponent<CurvedText>();
                characterText.text = characters[i].ToString();
                characterText.fontSize = 10f; // Adjust font size as needed
                CharacterTexts.Add(characterText);

                // Update current position for the next character
                currentPosition += characterSpacing;
            }
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
            // Update disc scale and position
            Disc.localScale = new Vector3((float)Diameter * 1.45f, (float)Height * 0.03f, (float)Diameter * 1.45f);
            Disc.localPosition = new Vector3(Disc.localPosition.x, (float)Height, Disc.localPosition.z);

            // Rotate the disc
            Disc.Rotate(new Vector3(0, 1, 0), (float)RotationRate);

            // Update rotation value
            Rotation = (Disc.localRotation.y + 1) / 2;
        }

        void ApplyForce(Particle particle)
        {
            // Calculate energy based on the character's ASCII value
            double energy = r.NextDouble() * Energy() / UniversalMachine.Particle.EnergeticResistance;

            // Apply force to the particle relative to the character's position
            for (int i = 0; i < CharacterTransforms.Count; i++)
            {
                Vector3 characterPosition = CharacterTransforms[i].position;
                Vector3 direction = (characterPosition - particle.transform.position).normalized;
                float distance = Vector3.Distance(characterPosition, particle.transform.position);

                // Calculate force based on ASCII value and distance
                float forceMagnitude = (CharacterTexts[i].text[0] - 65) / distance * (float)energy;

                // Apply force to the particle
                particle.AddForce(direction * forceMagnitude, Vector3.zero, Time.deltaTime);
            }
        }

        void FixedUpdate()
        {
            foreach (Particle unit in Quanta())
            {
                ApplyForce(unit);
            }
        }
    }
}
