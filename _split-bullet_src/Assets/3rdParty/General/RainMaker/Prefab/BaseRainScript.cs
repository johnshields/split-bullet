﻿using UnityEngine;

//
// Rain Maker (c) 2015 Digital Ruby, LLC
// http://www.digitalruby.com
//
namespace DigitalRuby.RainMaker
{
    public class BaseRainScript : MonoBehaviour
    {
        [Tooltip("Camera the rain should hover over, defaults to main camera")]
        public Camera Camera;

        [Tooltip(
            "Whether rain should follow the camera. If false, rain must be moved manually and will not follow the camera.")]
        public bool FollowCamera = true;

        [Tooltip("Intensity of rain (0-1)")] [Range(0.0f, 1.0f)]
        public float RainIntensity;

        [Tooltip("Rain particle system")] public ParticleSystem RainFallParticleSystem;

        [Tooltip("Particles system for when rain hits something")]
        public ParticleSystem RainExplosionParticleSystem;

        [Tooltip("Particle system to use for rain mist")]
        public ParticleSystem RainMistParticleSystem;

        [Tooltip("The threshold for intensity (0 - 1) at which mist starts to appear")] [Range(0.0f, 1.0f)]
        public float RainMistThreshold = 0.5f;

        [Tooltip("Wind zone that will affect and follow the rain")]
        public WindZone WindZone;

        [Tooltip(
            "X = minimum wind speed. Y = maximum wind speed. Z = sound multiplier. " +
            "Wind speed is divided by Z to get sound multiplier value. " +
            "Set Z to lower than Y to increase wind sound volume, or higher to decrease wind sound volume."
        )]
        public Vector3 WindSpeedRange = new Vector3(50.0f, 500.0f, 500.0f);

        [Tooltip("How often the wind speed and direction changes (minimum and maximum change interval in seconds)")]
        public Vector2 WindChangeInterval = new Vector2(5.0f, 30.0f);

        [Tooltip("Whether wind should be enabled.")]
        public bool EnableWind = true;

        protected Material rainMaterial;
        protected Material rainExplosionMaterial;
        protected Material rainMistMaterial;

        private float lastRainIntensityValue = -1.0f;
        private float nextWindTime;

        private void UpdateWind()
        {
            if (EnableWind && WindZone != null && WindSpeedRange.y > 1.0f)
            {
                WindZone.gameObject.SetActive(true);
                if (FollowCamera) WindZone.transform.position = Camera.transform.position;


                if (!Camera.orthographic) WindZone.transform.Translate(0.0f, WindZone.radius, 0.0f);

                if (nextWindTime < Time.time)
                {
                    WindZone.windMain = Random.Range(WindSpeedRange.x, WindSpeedRange.y);
                    WindZone.windTurbulence = Random.Range(WindSpeedRange.x, WindSpeedRange.y);
                    if (Camera.orthographic)
                    {
                        int val = Random.Range(0, 2);
                        WindZone.transform.rotation = Quaternion.Euler(0.0f, (val == 0 ? 90.0f : -90.0f), 0.0f);
                    }
                    else
                        WindZone.transform.rotation = Quaternion.Euler(Random.Range(-30.0f, 30.0f),
                            Random.Range(0.0f, 360.0f), 0.0f);

                    nextWindTime = Time.time + Random.Range(WindChangeInterval.x, WindChangeInterval.y);
                }
            }
            else
            {
                if (WindZone != null) WindZone.gameObject.SetActive(false);
            }
        }

        private void CheckForRainChange()
        {
            if (lastRainIntensityValue != RainIntensity)
            {
                lastRainIntensityValue = RainIntensity;
                if (RainIntensity <= 0.01f)
                {
                    if (RainFallParticleSystem != null)
                    {
                        ParticleSystem.EmissionModule e = RainFallParticleSystem.emission;
                        e.enabled = false;
                        RainFallParticleSystem.Stop();
                    }

                    if (RainMistParticleSystem != null)
                    {
                        ParticleSystem.EmissionModule e = RainMistParticleSystem.emission;
                        e.enabled = false;
                        RainMistParticleSystem.Stop();
                    }
                }
                else
                {
                    if (RainFallParticleSystem != null)
                    {
                        ParticleSystem.EmissionModule e = RainFallParticleSystem.emission;
                        e.enabled = RainFallParticleSystem.GetComponent<Renderer>().enabled = true;
                        if (!RainFallParticleSystem.isPlaying) RainFallParticleSystem.Play();

                        ParticleSystem.MinMaxCurve rate = e.rateOverTime;
                        rate.mode = ParticleSystemCurveMode.Constant;
                        rate.constantMin = rate.constantMax = RainFallEmissionRate();
                        e.rateOverTime = rate;
                    }

                    if (RainMistParticleSystem != null)
                    {
                        ParticleSystem.EmissionModule e = RainMistParticleSystem.emission;
                        e.enabled = RainMistParticleSystem.GetComponent<Renderer>().enabled = true;
                        if (!RainMistParticleSystem.isPlaying) RainMistParticleSystem.Play();

                        float emissionRate;
                        if (RainIntensity < RainMistThreshold) emissionRate = 0.0f;

                        else
                        {
                            // must have RainMistThreshold or higher rain intensity to start seeing mist
                            emissionRate = MistEmissionRate();
                        }

                        ParticleSystem.MinMaxCurve rate = e.rateOverTime;
                        rate.mode = ParticleSystemCurveMode.Constant;
                        rate.constantMin = rate.constantMax = emissionRate;
                        e.rateOverTime = rate;
                    }
                }
            }
        }

        protected virtual void Start()
        {
#if DEBUG

            if (RainFallParticleSystem == null)
            {
                Debug.LogError("Rain fall particle system must be set to a particle system");
                return;
            }

#endif

            if (Camera == null) Camera = Camera.main;

            if (RainFallParticleSystem != null)
            {
                ParticleSystem.EmissionModule e = RainFallParticleSystem.emission;
                e.enabled = false;
                Renderer rainRenderer = RainFallParticleSystem.GetComponent<Renderer>();
                rainRenderer.enabled = false;
                rainMaterial = new Material(rainRenderer.material);
                rainMaterial.EnableKeyword("SOFTPARTICLES_OFF");
                rainRenderer.material = rainMaterial;
            }

            if (RainExplosionParticleSystem != null)
            {
                ParticleSystem.EmissionModule e = RainExplosionParticleSystem.emission;
                e.enabled = false;
                Renderer rainRenderer = RainExplosionParticleSystem.GetComponent<Renderer>();
                rainExplosionMaterial = new Material(rainRenderer.material);
                rainExplosionMaterial.EnableKeyword("SOFTPARTICLES_OFF");
                rainRenderer.material = rainExplosionMaterial;
            }

            if (RainMistParticleSystem != null)
            {
                ParticleSystem.EmissionModule e = RainMistParticleSystem.emission;
                e.enabled = false;
                Renderer rainRenderer = RainMistParticleSystem.GetComponent<Renderer>();
                rainRenderer.enabled = false;
                rainMistMaterial = new Material(rainRenderer.material);
                if (UseRainMistSoftParticles) rainMistMaterial.EnableKeyword("SOFTPARTICLES_ON");
                else rainMistMaterial.EnableKeyword("SOFTPARTICLES_OFF");

                rainRenderer.material = rainMistMaterial;
            }
        }

        protected virtual void Update()
        {
#if DEBUG

            if (RainFallParticleSystem == null)
            {
                Debug.LogError("Rain fall particle system must be set to a particle system");
                return;
            }

#endif

            CheckForRainChange();
            UpdateWind();
        }

        protected virtual float RainFallEmissionRate()
        {
            return (RainFallParticleSystem.main.maxParticles / RainFallParticleSystem.main.startLifetime.constant) *
                   RainIntensity;
        }

        protected virtual float MistEmissionRate()
        {
            return (RainMistParticleSystem.main.maxParticles / RainMistParticleSystem.main.startLifetime.constant) *
                   RainIntensity * RainIntensity;
        }

        protected virtual bool UseRainMistSoftParticles
        {
            get { return true; }
        }
    }
}