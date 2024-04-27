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

        // Cast the chromosome to the appropriate type
        var carChromosome = (CarChromosome)chromosome;

        // Get the genes of the chromosome
        var genes = carChromosome.GetGenes();
        
        // Create a new instance of Random to generate random numbers
        var rnd = new Random();

        // Iterate over each gene in the chromosome
        for (int i = 0; i < genes.Length; i++){

            // Check if mutation should occur based on the probability
            if (rnd.NextDouble() < probability){

                // Perform mutation by creating a new chromosome with the mutated gene at position i
                CarChromosome newChromosome = new CarChromosome(carChromosome.getConfig());
                genes[i] = newChromosome.GetGene(i);
            }
        }

        // Replace the genes of the original chromosome with the mutated genes
        carChromosome.ReplaceGenes(0, genes);
        /*END OF YOUR CODE*/
    }

}
