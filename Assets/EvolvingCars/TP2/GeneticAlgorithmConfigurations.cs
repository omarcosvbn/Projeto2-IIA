using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using GeneticSharp.Runner.UnityApp.Car;
using GeneticSharp.Runner.UnityApp.Commons;
using UnityEngine;

public static class GeneticAlgorithmConfigurations
{
    /* YOUR CODE HERE:
    * 
    * Configuration of the algorithm: You should change the configurations of the algorithm here
    */

    public static float crossoverProbability = 0.9f; 
    public static float mutationProbability = 0.05f; 
    public static int maximumNumberOfGenerations = 30;
    public static int eliteSize = 2;

    public static int minGeneValue = 1;
    public static int maxGeneValue = 10;

    public static Crossover crossoverOperator = new Crossover(crossoverProbability);
    public static Mutation mutationOperator = new Mutation();
    public static ParentSelection parentSelection = new ParentSelection();
    public static SurvivorSelection survivorSelection = new SurvivorSelection(eliteSize);
    public static GenerationsTermination terminationCondition = new GenerationsTermination(maximumNumberOfGenerations);
}
