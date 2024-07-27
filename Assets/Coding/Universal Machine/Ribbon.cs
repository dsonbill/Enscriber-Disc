using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniversalMachine
{
    public class Ribbon : MonoBehaviour
    {

        public double CylinderHeight;
        public double CylinderDiameter;


        public double Height;
        public double Deflection;
        public double Mean;


        //Vector3();
        //Vector4(Matrix4x4, Yellow,ZoneDefinition, while,)

        // Update is called once per frame
        void Update()
        {

        }

        // Start is called before the first frame update
        void Start()
        {

        }

        void Curve()
        {
            // Transform the vertex coordinates from model space into world space
            //float4 vv = mul(_Object2World, v.vertex);

            // Now adjust the coordinates to be relative to the camera position
            //vv.xyz -= _WorldSpaceCameraPos.xyz;

            // Reduce the y coordinate (i.e. lower the "height") of each vertex based
            // on the square of the distance from the camera in the z axis, multiplied
            // by the chosen curvature factor
            //vv = float4(0.0f, (vv.z * vv.z) * -_Curvature, 0.0f, 0.0f);

            // Now apply the offset back to the vertices in model space
            //v.vertex += mul(_World2Object, vv);
        }
    }
}