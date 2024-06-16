using System;
using UnityEngine;

namespace config {
    [Serializable]
    public class AnimationSpeedConfig {
        [SerializeField]
        [Tooltip("Animation speed in degrees per second")]
        public float rotation = 60f;

        [SerializeField]
        public float position = 500f;
        
        [SerializeField]
        public float releasePosition = 2000f;

        [SerializeField]
        public float zoom = 0.3f;

        private float referenceWidth = 1920f;
        private float referenceHeight = 1080f;

        public void AdjustForScreenResolution()
        {
            float screenWidth = Screen.width;
            float screenHeight = Screen.height;

            float widthScale = screenWidth / referenceWidth;
            float heightScale = screenHeight / referenceHeight;

            float scaleFactor = (widthScale + heightScale) / 2;

            rotation *= scaleFactor;
            position *= scaleFactor;
            releasePosition *= scaleFactor;
            zoom *= scaleFactor;
        }
    }
}
