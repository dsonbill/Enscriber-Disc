using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Misc;

namespace UniversalMachine
{
    public class PathMarker : MonoBehaviour
    {
        public int Limit;
        public class Mark
        {
            public Vector3 Position;
            public Vector3 Direction;
            public float Length;
            public Vector3 Energy;

            public Vector3 Proceed(Vector3 position, Vector3 destination, Vector3 energy)
            {
                Vector3 a = destination - position;

                Vector3 y = Direction;

                float distance = Vector3.Distance(a, y);

                //Debug.Log("A: " + a);
                //Debug.Log("Y: " + y);

                //Debug.Log("Angle: " + angle);

                //float attack = 1 / 180 / distance;

                //Debug.Log("Attack: " + attack);

                float force = y.magnitude / distance;

                //Debug.Log("Force: " + force);

                //Vector3 warpDirection = y;
                //Vector3 warpEnergy = Energy / Length;
                //Vector3 pathEnergy = energy * a.magnitude;

                //Vector3 warp = new Vector3((warpEnergy.x - pathEnergy.x) * warpDirection.x,
                //    (warpEnergy.y - pathEnergy.y) * warpDirection.y,
                //    (warpEnergy.z - pathEnergy.z) * warpDirection.z);

                Vector3 warp = y * force * Energy.magnitude;

                return warp;
            }
        }

        public List<Mark> Path = new List<Mark>();

        public Vector3 Project(Vector3 start, Vector3 end, Vector3 energy)
        {
            Vector3 final = Vector3.zero;

            foreach (Mark marker in Path)
            {
                final += marker.Proceed(start, end, energy);
                //Debug.Log("Proceed: " + marker.Proceed(start, end, energy));
            }

            Move(start, end, energy);

            return final;
        }

        public void Move(Vector3 start, Vector3 end, Vector3 energy)
        {
            Mark path = new Mark();

            path.Position = start;
            path.Direction = (end - start);
            path.Length = (end - start).magnitude;
            path.Energy = energy;

            if (Path.Count > Limit)
            {
                Path.RemoveAt(Path.Count - 1);
            }

            Path.Add(path);
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