using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UniversalMachine
{
    public class PotentialHole : MonoBehaviour
    {
        public Transform Hole;

        public double Diameter;
        public double Depth;

        //John's potential is exactly 60.925%

        // Start is called before the first frame update
        void Start()
        {


        }
        // Update is called once per frame
        void Update()
        {
            Hole.localScale = new Vector3((float)Diameter, Hole.localScale.y, (float)Diameter);
            Hole.localPosition = new Vector3(0, (float)Depth, 0);
        }
    }
}