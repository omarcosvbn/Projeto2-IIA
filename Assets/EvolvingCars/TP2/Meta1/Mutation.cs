using System.Diagnostics;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Mutations;
using GeneticSharp.Runner.UnityApp.Car;
using System;

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

        var carChromosome = (CarChromosome)chromosome;

        var genes = carChromosome.GetGenes();
        var rnd = new Random();

        for (int i = 0; i < genes.Length; i++){
            if (rnd.NextDouble() < probability){
                CarChromosome newChromosome = new CarChromosome(carChromosome.getConfig());
                genes[i] = newChromosome.GetGene(i);
            }
        }

        carChromosome.ReplaceGenes(0, genes);
        /*END OF YOUR CODE*/
    }

}
