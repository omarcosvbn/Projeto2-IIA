using GeneticSharp.Domain.Chromosomes;
using System;
using System.Linq;
using GeneticSharp.Domain.Randomizations;
using System.Collections.Generic;
using GeneticSharp.Domain.Crossovers;
using System.Drawing.Text;

namespace GeneticSharp.Runner.UnityApp.Commons
{
    public class Crossover : ICrossover
    {
  

        

        public int ParentsNumber { get; private set; }

        public int ChildrenNumber { get; private set; }

        public int MinChromosomeLength { get; private set; }

        public bool IsOrdered { get; private set; } // indicating whether the operator is ordered (if can keep the chromosome order).

        protected float crossoverProbability;


        public Crossover(float crossoverProbability) : this(2, 2, 2, true)
        {
            this.crossoverProbability = crossoverProbability;
        }

        public Crossover(int parentsNumber, int offSpringNumber, int minChromosomeLength, bool isOrdered)
        {
            ParentsNumber = parentsNumber;
            ChildrenNumber = offSpringNumber;
            MinChromosomeLength = minChromosomeLength;
            IsOrdered = isOrdered;
        }

        public IList<IChromosome> Cross(IList<IChromosome> parents)
        {
            IChromosome parent1 = parents[0];
            IChromosome parent2 = parents[1];
            IChromosome offspring1 = parent1.Clone();
            IChromosome offspring2 = parent2.Clone();

            /* YOUR CODE HERE */
            /*REPLACE THESE LINES BY YOUR CROSSOVER IMPLEMENTATION*/
            var genes = offspring1.GetGenes();
            int length = genes.Length;
            var rnd = new Random();
            int end = rnd.Next(0, length - 1);

            // Check if a crossover should occur based on the probability
            if (RandomizationProvider.Current.GetDouble() <= crossoverProbability){
                for (int i = 0; i <= end; i++){
                    // Swap genes between parents to create offspring
                    offspring1.ReplaceGene(i, parent2.GetGene(i));
                    offspring2.ReplaceGene(i, parent1.GetGene(i));
                }
            }

            /*END OF YOUR CODE*/

            return new List<IChromosome> { offspring1, offspring2 };
            
        }
    }
}