using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ParticlesEvolution : MonoBehaviour
{
    public int chromosomeLength = 16;
    public List<Chromosome> population = new List<Chromosome>();
    public List<GameObject> particles = new List<GameObject>();
    public GameObject[] ratingFields; 
    public float mutationRate = 0.1f;
    public float mutationRange = 0.1f;
    
    public int currentGeneration = 1;
    public TMP_Text textBox;
    void Start()
    {
        textBox.text = "Generation: 1";

        InitializePopulation(particles.Count);
    }

    void InitializePopulation(int size)
    {
        for (int i = 0; i < size; i++)
        {
            population.Add(GetInitialChromosomeValues(particles[i]));
        } 
    }

    private Chromosome GetInitialChromosomeValues(GameObject particleObj)
    {
        ParticleSystem ps = particleObj.GetComponent<ParticleSystem>();
        ParticleSystem.MainModule main = ps.main;
        ParticleSystem.ShapeModule shape = ps.shape;
        ParticleSystem.VelocityOverLifetimeModule vel = ps.velocityOverLifetime;
        ParticleSystem.NoiseModule noise = ps.noise;
        
        List<Gene> genes = new List<Gene>();
        for (int i = 0; i < chromosomeLength; i++)
        {
            switch (i)
            {
                case 0: 
                    genes.Add(new FloatGene(main.duration));
                    break;
                case 1:
                    genes.Add(new FloatGene(main.startSizeX.constant));
                    break;
                case 2: 
                    genes.Add(new FloatGene(main.startSizeY.constant));
                    break;
                case 3: 
                    genes.Add(new FloatGene(main.startSizeZ.constant));
                    break;
                case 4: 
                    genes.Add(new FloatGene(main.maxParticles));
                    break;
                case 5: 
                    genes.Add(new ColorGene(main.startColor.color));
                    break;
                case 6: 
                    genes.Add(new FloatGene(vel.radial.constant));
                    break;
                case 7: 
                    genes.Add(new FloatGene(vel.speedModifier.constant));
                    break;
                case 8: 
                    genes.Add(new FloatGene(noise.remapX.constant));
                    break;
                case 9: 
                    genes.Add(new FloatGene(noise.remapY.constant));
                    break;
                case 10:
                    genes.Add(new FloatGene(noise.remapZ.constant));
                    break;
                case 11: 
                    genes.Add(new FloatGene(noise.frequency));
                    break;
                case 12: 
                    genes.Add(new FloatGene(noise.scrollSpeed.constant));
                    break;
                case 13:
                    genes.Add(new FloatGene(noise.positionAmount.constant));
                    break;
                case 14: 
                    genes.Add((new FloatGene(noise.rotationAmount.constant)));
                    break;
                case 15: 
                    genes.Add(new FloatGene(noise.sizeAmount.constant));
                    break;
            }

        }
        return new Chromosome(genes);
    }

    public void OnEvolveClick()
    {
        List<Chromosome> selectedChromosomes = new List<Chromosome>();
        for (int i = 0; i < ratingFields.Length; i++)
        {
            int rating = ratingFields[i].GetComponent<StarRating>().GetRating();
            population[i].Fitness = CalculateFitness(population[i], rating);
            selectedChromosomes.Add(population[i]);
        }
        population.Sort((a, b) => b.Fitness.CompareTo(a.Fitness));
        List<Chromosome> newPopulation = new List<Chromosome>();

        for (int i = 0; i < selectedChromosomes.Count / 2; i++)
        {
            newPopulation.Add(selectedChromosomes[i]);
        }
        
        while (newPopulation.Count <= particles.Count)
        {
            Chromosome parent1 = SelectParent(selectedChromosomes);
            Chromosome parent2 = SelectParent(selectedChromosomes);
            newPopulation.AddRange(parent1.Crossover(parent2));
        }
        
        foreach (Chromosome chromosome in newPopulation)
        {
            chromosome.Mutate(mutationRate, mutationRange);
        }

        population = newPopulation;
        

        for (int i = 0; i < population.Count - 1; i++)
        {
            UpdateDisplayPopulation(i);
        }
        
        float topFitness = 0;
        foreach (Chromosome chromosome in population)
        {
            if (chromosome.Fitness > topFitness)
            {
                topFitness = chromosome.Fitness;
            }
        }
            
        Debug.Log("Current Generation " + currentGeneration + ". Top Fitness: " + topFitness);

        UpdateGenerationText();

        ResetStars();
    }
    
    public void OnEvolveClick100()
    {
        for (int x = 0; x < 100; x++)
        {
            OnEvolveClick();
        }
    }

    private void UpdateGenerationText()
    {
        currentGeneration++;
        textBox.text = "Generation: " + currentGeneration.ToString();
    }

    private void ResetStars()
    {
        for (int i = 0; i < ratingFields.Length; i++)
        {
            ratingFields[i].GetComponent<StarRating>().SetRating(0);
        }
    }


    private void UpdateDisplayPopulation(int i)
    {
        Chromosome chrom = population[i];
        GameObject particleObj = particles[i];

        ParticleSystem ps = particleObj.GetComponent<ParticleSystem>();
        ParticleSystem.MainModule main = ps.main;
        ParticleSystem.VelocityOverLifetimeModule vel = ps.velocityOverLifetime;
        ParticleSystem.NoiseModule noise = ps.noise;
        
        ps.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);

        main.duration = (chrom.Genes[0] as FloatGene).Value;
        main.startSizeX = (chrom.Genes[1] as FloatGene).Value;
        main.startSizeY = (chrom.Genes[2] as FloatGene).Value;
        main.startSizeZ = (chrom.Genes[3] as FloatGene).Value;
        main.maxParticles = (int) (chrom.Genes[4] as FloatGene).Value;
        main.startColor = (chrom.Genes[5] as ColorGene).Value;
        
        vel.radial = (chrom.Genes[6] as FloatGene).Value;
        vel.speedModifier = (chrom.Genes[7] as FloatGene).Value;

        noise.remapX = (chrom.Genes[8] as FloatGene).Value;
        noise.remapY = (chrom.Genes[9] as FloatGene).Value;
        noise.remapZ = (chrom.Genes[10] as FloatGene).Value;
        noise.frequency = (chrom.Genes[11] as FloatGene).Value;
        noise.scrollSpeed = (chrom.Genes[12] as FloatGene).Value;
        noise.positionAmount = (chrom.Genes[13] as FloatGene).Value;
        noise.rotationAmount = (chrom.Genes[14] as FloatGene).Value;
        noise.sizeAmount = (chrom.Genes[15] as FloatGene).Value;

        ps.Play(false);

    }
    private Chromosome SelectParent(List<Chromosome> pool)
    {
        int index = Random.Range(0, pool.Count);
        return pool[index];
    }
    
    private float CalculateFitness(Chromosome chromosome, int userRating)
    {
        int maxParticles = (int)(chromosome.Genes[4] as FloatGene).Value;

        float fitness = userRating * 10.0f + maxParticles * 2.0f;
        return fitness;
    }
}
