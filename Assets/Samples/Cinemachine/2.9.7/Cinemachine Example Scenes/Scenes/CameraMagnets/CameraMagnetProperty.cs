﻿using UnityEngine;

namespace Samples.Cinemachine._2._9._7.Cinemachine_Example_Scenes.Scenes.CameraMagnets
{
    [ExecuteInEditMode]
    public class CameraMagnetProperty : MonoBehaviour
    {
        [Range(0.1f, 50.0f)]
        public float MagnetStrength = 5.0f;

        [Range(0.1f, 50.0f)]
        public float Proximity = 5.0f;

        public Transform ProximityVisualization;

        [HideInInspector]
        public Transform myTransform;

        void Start()
        {
            myTransform = transform;
        }

        void Update()
        {
            if (ProximityVisualization != null)
                ProximityVisualization.localScale = new Vector3(Proximity * 2.0f, Proximity * 2.0f, 1);
        }
    }
}
