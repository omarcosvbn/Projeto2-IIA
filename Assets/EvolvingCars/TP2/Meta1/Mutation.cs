using System.Diagnostics;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Mutations;
using GeneticSharp.Runner.UnityApp.Car;

public class Mutation : IMutation
{
    public bool IsOrdered { get; private set; } // indicating whether the operator is ordered (if can keep the chromosome order).

    public Mutation()
    {
        IsOrdered = true;
    }

    public void Mutate(IChromosome chromosome, float probability)
    {
        
        /* YOUR CODE HERE */
        /*REPLACE THESE LINES BY YOUR MUTATION IMPLEMENTATION*/
        CarChromosome newChromosome = new CarChromosome(((CarChromosome)chromosome).getConfig());
        chromosome.ReplaceGenes(0, newChromosome.GetGenes());
        /*END OF YOUR CODE*/
    }

}
