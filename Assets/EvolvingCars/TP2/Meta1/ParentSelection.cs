using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Populations;
using GeneticSharp.Domain.Randomizations;
using GeneticSharp.Domain.Selections;
using GeneticSharp.Infrastructure.Framework.Texts;
using GeneticSharp.Runner.UnityApp.Car;
using UnityEngine;

public class ParentSelection : SelectionBase
{
    public ParentSelection() : base(2)
    {
    }




    protected override IList<IChromosome> PerformSelectChromosomes(int number, Generation generation)
    {

        IList<CarChromosome> population = generation.Chromosomes.Cast<CarChromosome>().ToList();
        IList<IChromosome> parents = new List<IChromosome>();

        int[] randomIndexes = RandomizationProvider.Current.GetUniqueInts(number, 0, population.Count);
        while (parents.Count < number)
        {
            parents.Add(population[randomIndexes[parents.Count]]);
        }

        return parents;
    }
}
