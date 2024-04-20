using GeneticSharp.Domain.Fitnesses;
using GeneticSharp.Domain.Chromosomes;
using System.Threading;
using UnityEngine;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System;
using System.Linq;

namespace GeneticSharp.Runner.UnityApp.Car
{
    public class CarFitness : IFitness
    {
        public CarFitness()
        {
            ChromosomesToBeginEvaluation = new BlockingCollection<CarChromosome>();
            ChromosomesToEndEvaluation = new BlockingCollection<CarChromosome>();
        }

        public BlockingCollection<CarChromosome> ChromosomesToBeginEvaluation { get; private set; }
        public BlockingCollection<CarChromosome> ChromosomesToEndEvaluation { get; private set; }
        public double Evaluate(IChromosome chromosome)
        {
            var c = chromosome as CarChromosome;
            ChromosomesToBeginEvaluation.Add(c);

            float fitness = 0; 
            do
            {
                Thread.Sleep(1000);

                
                float Distance = c.Distance;
                float EllapsedTime = c.EllapsedTime;
                float NumberOfWheels = c.NumberOfWheels;
                float CarMass = c.CarMass;
                int RoadCompleted = c.RoadCompleted ? 1 : 0;

                List<float> Velocities = c.Velocities;
                float SumVelocities = c.SumVelocities;
                
                List<float> Accelerations = c.Accelerations;
                float SumAccelerations = c.SumAccelerations;

                List<float> Forces = c.Forces;
                float SumTotalForces = c.SumForces;

                /*YOUR CODE HERE*/
                /*Note que é executado ao longo da simulação*/

                fitness = SumVelocities;

                /*END OF YOUR CODE*/

                c.Fitness = fitness;

            } while (!c.Evaluated);

            ChromosomesToEndEvaluation.Add(c);


            do
            {
                Thread.Sleep(1000);
            } while (!c.Evaluated);

            /*O valor da variável fitness é o valor de aptidão do indivíduo*/

            return fitness;
        }

    }
}