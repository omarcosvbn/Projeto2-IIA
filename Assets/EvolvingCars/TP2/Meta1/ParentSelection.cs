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

public class ParentSelection : SelectionBase
{
    public ParentSelection() : base(2)
    {
    }




    protected override IList<IChromosome> PerformSelectChromosomes(int number, Generation generation)
    {

        IList<CarChromosome> population = generation.Chromosomes.Cast<CarChromosome>().ToList();
        IList<IChromosome> parents = new List<IChromosome>();
        
        /* YOUR CODE HERE */
        /*REPLACE THESE LINES BY YOUR PARENT SELECTION IMPLEMENTATION*/

        // Initialize tournament size
        int tournamentSize = 2;

        // Continue selecting parents until the desired number is reached
        while (parents.Count < number){

            // Randomly select tournament participants from the population
            var tournamentParticipantsIndexes = RandomizationProvider.Current.GetUniqueInts(tournamentSize, 0, population.Count);

            // Initialize the winner of the tournament
            CarChromosome winner = null;

            // Iterate over tournament participants
            foreach (var index in tournamentParticipantsIndexes){
                var participant = population[index];

                // Determine the winner of the tournament based on fitness
                if (winner == null || participant.Fitness > winner.Fitness){
                    winner = participant;
                }
            }

            // Add the winner of the tournament to the selected parents
            parents.Add(winner);
        }
        /*END OF YOUR CODE*/

        return parents;
    }
}
