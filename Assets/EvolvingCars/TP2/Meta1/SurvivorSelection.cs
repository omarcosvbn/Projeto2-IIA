using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Populations;
using GeneticSharp.Domain.Reinsertions;


public class SurvivorSelection : ReinsertionBase
{
    protected int eliteSize = 0;
    public SurvivorSelection(int eliteSize) : base(false, false)
    {
        this.eliteSize = eliteSize;
    }
    
    protected override IList<IChromosome> PerformSelectChromosomes(IPopulation population, IList<IChromosome> offspring, IList<IChromosome> parents)
    {

        var bestParents = population.CurrentGeneration.Chromosomes.OrderByDescending(p => p.Fitness).Take(eliteSize).ToList();

        for (int i = 0; i < bestParents.Count; i++)
        {
            offspring[i] = bestParents[i];
        }
        

        return offspring;
    }
    
}
