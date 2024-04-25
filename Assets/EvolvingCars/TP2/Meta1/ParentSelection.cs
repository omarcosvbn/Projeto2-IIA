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
        int tournamentSize = 5; // Tamanho do torneio
        while (parents.Count < number)
        {
            // Selecionar aleatoriamente indivíduos para o torneio
            var tournamentParticipantsIndexes = RandomizationProvider.Current.GetUniqueInts(tournamentSize, 0, population.Count);

            // Encontrar o vencedor do torneio
            CarChromosome winner = null;
            foreach (var index in tournamentParticipantsIndexes)
            {
                var participant = population[index];
                if (winner == null || participant.Fitness > winner.Fitness)
                {
                    winner = participant;
                }
            }

            // Adicionar o vencedor à lista de pais
            parents.Add(winner);
        }
        /*END OF YOUR CODE*/

        return parents;
    }
}
