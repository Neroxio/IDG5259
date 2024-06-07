using UnityEngine;
using System.Collections.Generic;
public class Chromosome
{
    public List<Gene> Genes { get; set; }
    public float Fitness { get; set; }

    public Chromosome(List<Gene> genes)
    {
        Genes = genes;
    }

    public List<Chromosome> Crossover(Chromosome other)
    {
        List<Chromosome> offspring = new List<Chromosome>();
        List<Gene> child1Genes = new List<Gene>();
        List<Gene> child2Genes = new List<Gene>();

        for (int i = 0; i < Genes.Count; i++)
        {
            if (Random.Range(0f, 1f) < 0.5f)
            {
                child1Genes.Add(Genes[i].Crossover(other.Genes[i]));
                child2Genes.Add(other.Genes[i].Crossover(Genes[i]));
            }
            else
            {
                child1Genes.Add(other.Genes[i].Crossover(Genes[i]));
                child2Genes.Add(Genes[i].Crossover(other.Genes[i]));
            }
        }

        offspring.Add(new Chromosome(child1Genes));
        offspring.Add(new Chromosome(child2Genes));
        return offspring;
    }

    public void Mutate(float mutationRate, float mutationRange)
    {
        foreach (Gene gene in Genes)
        {
            gene.Mutate(mutationRate, mutationRange);
        }
    }
}