using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gene
{
    public abstract Gene Crossover(Gene other);
    public abstract void Mutate(float mutationRate, float mutationRange);
    
}

public class FloatGene : Gene
{
    public float Value { get; set; }

    public FloatGene(float value)
    {
        Value = value;
    }

    public override Gene Crossover(Gene other)
    {
        FloatGene otherGene = other as FloatGene;
        float newValue = (Value + otherGene.Value) / 2;
        return new FloatGene(newValue);
    }

    public override void Mutate(float mutationRate, float mutationRange)
    {
        if (Random.value < mutationRate)
        {
            Value += Random.Range(-mutationRange, mutationRange);
        }
    }
}


public class ColorGene : Gene
{
    public Color Value { get; set; }

    public ColorGene(Color value)
    {
        Value = value;
    }

    public override Gene Crossover(Gene other)
    {
        ColorGene otherGene = other as ColorGene;
        Color newValue = Color.Lerp(Value, otherGene.Value, 0.5f);
        return new ColorGene(newValue);
    }

    public override void Mutate(float mutationRate, float mutationRange)
    {
        if (Random.value < mutationRate)
        {
            var value = Value;
            value.r = Random.Range(-mutationRange, mutationRange);
            value.g = Random.Range(-mutationRange, mutationRange);
            value.b += Random.Range(-mutationRange, mutationRange);
            Value = value;
        }
    }
}


