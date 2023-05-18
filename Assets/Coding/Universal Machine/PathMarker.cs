using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Misc;

namespace UniversalMachine
{
    public class PathMarker : MonoBehaviour
    {
        public class Mark : ScriptableObject
        {
            public Vector3 Position;
            public Vector3 Direction;
            public float Length;
            public Vector3 Energy;

            public Vector3 Proceed(Vector3 position, Vector3 destination, Vector3 energy)
            {
                Vector3 a = destination - position;

                Vector3 x = a.normalized;
                Vector3 y = Direction;

                float angle = Vector3.Angle(x, y);

                float attack = 1 / 180 * angle;

                float distance = (Position + (Direction * (Length / 2))).magnitude
                    - (position + (x * (a.magnitude / 2))).magnitude;

                Vector3 warpDirection = new Vector3(y.x, y.y, y.z);
                Vector3 warpEnergy = Energy / Length;
                Vector3 pathEnergy = energy * a.magnitude;

                //Vector3 warp = new Vector3((warpEnergy.x - pathEnergy.x) * warpDirection.x,
                //    (warpEnergy.y - pathEnergy.y) * warpDirection.y,
                //    (warpEnergy.z - pathEnergy.z) * warpDirection.z);

                Vector3 warp = warpDirection *
                    (warpEnergy.magnitude / attack - pathEnergy.magnitude * attack) /
                    distance;

                return warp;
            }
        }

        public KeyedList<Mark> Path = new KeyedList<Mark>();

        public Dictionary<int,Mark> Striations = new Dictionary<int,Mark>();

        public Vector3 Project(Vector3 start, Vector3 end, Vector3 energy)
        {
            Vector3 final = Vector3.zero;

            foreach (Mark marker in Path)
            {
                final += marker.Proceed(start, final, energy);
                Debug.Log("Proceed: " + marker.Proceed(start, final, energy));
            }

            Move(start, end, energy);

            return (final + end) / 2;
        }

        public void Move(Vector3 start, Vector3 end, Vector3 energy)
        {
            Mark path = ScriptableObject.CreateInstance<Mark>();

            path.Position = start;
            path.Direction = (end - start).normalized;
            path.Length = (end - start).magnitude;
            path.Energy = energy;

            Path.Add(path);

            Striations.Add(Striations.Count, path);
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}