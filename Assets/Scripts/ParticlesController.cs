using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class ParticlesController : MonoBehaviour
{
    public List<GameObject> particleSystems;

    [Header("Duration")]
    [SerializeField] public float durationMin;
    [SerializeField] public float durationMax;

    [Header("Size")] 
    [SerializeField] public float sizeMin;
    [SerializeField] public float sizeMax;
    
    [Header("Particles Amount")] 
    [SerializeField] public int particlesAmountMin;
    [SerializeField] public int particlesAmountMax;
    
    [Header("Radial")]
    [SerializeField] public int radialMin;
    [SerializeField] public int radialMax;
    
    [Header("Noise Speed Modifier")]
    [SerializeField] public int speedModMin;
    [SerializeField] public int speedModMax;

    
    [Header("Noise Frequency")]
    [SerializeField] public int freqMin;
    [SerializeField] public int freqMax;

    
    [Header("Noise Scroll")]
    [SerializeField] public int scrollMin;
    [SerializeField] public int scrollMax;


    
    void Start()
    {
        RandomizeInitialValues();
    }

    void RandomizeInitialValues()
    {
        foreach(GameObject obj in particleSystems)
        {
            ParticleSystem ps = obj.GetComponent<ParticleSystem>();
            
            ps.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
            ParticleSystem.MainModule main = ps.main;

            main.duration = Random.Range(durationMin, durationMax);
            main.prewarm = true;
            main.startSize3D = true;
            main.startSizeX = Random.Range(sizeMin, sizeMax);
            main.startSizeY = Random.Range(sizeMin, sizeMax);
            main.startSizeZ = Random.Range(sizeMin, sizeMax);
            main.maxParticles = Random.Range(particlesAmountMin, particlesAmountMax);
            main.startColor = RandomColorRGB();

            ParticleSystem.ShapeModule shapeMod = ps.shape;
            //shapeMod.shapeType = RandomShape();
            shapeMod.shapeType = ParticleSystemShapeType.Circle; 
            ParticleSystem.VelocityOverLifetimeModule velocityOverLifetimeModule = ps.velocityOverLifetime;
            velocityOverLifetimeModule.radial = Random.Range(radialMin, radialMax);
            velocityOverLifetimeModule.speedModifier = Random.Range(speedModMin, speedModMax);

            ParticleSystem.NoiseModule noiseModule = ps.noise;
            noiseModule.separateAxes = true;
            noiseModule.remapX = Random.Range(sizeMin, sizeMax);
            noiseModule.remapY = Random.Range(sizeMin, sizeMax);
            noiseModule.remapZ = Random.Range(sizeMin, sizeMax);
            noiseModule.frequency = Random.Range(freqMin, freqMax);
            noiseModule.scrollSpeed = Random.Range(scrollMin, scrollMax);

            noiseModule.positionAmount = Random.Range(freqMin, freqMax);
            noiseModule.rotationAmount = Random.Range(freqMin, freqMax);
            noiseModule.sizeAmount = Random.Range(freqMin, freqMax);
            
            




        }

        PlayParticles();
    }

    void PlayParticles()
    {
        foreach (GameObject obj in particleSystems)
        {
            ParticleSystem ps = obj.GetComponent<ParticleSystem>();
            ps.Play(false);

        }
    }
    private Color RandomColorRGB()
    {
        Color col = new Color(Random.value, Random.value, Random.value);
        return col;
    }

    private bool RandBool()
    {
        return Random.value < 0.5f;
    }

    private ParticleSystemShapeType RandomShape()
    {
        List<ParticleSystemShapeType> possibleShapes = new List<ParticleSystemShapeType>();
        possibleShapes.Add(ParticleSystemShapeType.Box);
        possibleShapes.Add(ParticleSystemShapeType.Circle);
        possibleShapes.Add(ParticleSystemShapeType.Cone);
        possibleShapes.Add(ParticleSystemShapeType.Donut);
        possibleShapes.Add(ParticleSystemShapeType.Hemisphere);
        possibleShapes.Add(ParticleSystemShapeType.Rectangle);
        
        int randElem = Random.Range(0, possibleShapes.Count);
        
        return possibleShapes[randElem];
    }


}
