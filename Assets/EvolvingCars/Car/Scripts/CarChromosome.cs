using System;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Runner.UnityApp.Commons;
using System.Collections.Generic;
namespace GeneticSharp.Runner.UnityApp.Car
{
    [Serializable]
    public class CarChromosome : FloatingPointChromosome<CarVectorFloatPhenotypeEntity>
    {
        private CarSampleConfig m_config;
        public CarChromosome(CarSampleConfig config)
        {
            m_config = config;

            var phenotypeEntities = new CarVectorFloatPhenotypeEntity[config.VectorsCount];

            for (int i = 0; i < phenotypeEntities.Length; i ++)
            {
                phenotypeEntities[i] = new CarVectorFloatPhenotypeEntity(config, i);
            }

            SetPhenotypes(phenotypeEntities);
            CreateGenes();
        
        }

        public CarSampleConfig getConfig()
        {
            return m_config;
        }

        public string ID { get; } = System.Guid.NewGuid().ToString();

        public bool Evaluated { get; set; }
        public float Distance { get; set; }
        public float EllapsedTime { get; set; }
        new public float Fitness { get; set; }
        public float NumberOfWheels { get; set; }
        public float CarMass { get; set; }
        public bool RoadCompleted { get; set; } = false;

        public List<float> Velocities { get; set; }
        public float SumVelocities { get; set; }
        public List<float> Accelerations { get; set; }
        public float SumAccelerations { get; set; }
        public List<float> Forces { get; set; }
        public float SumForces { get; set; }
      
        public override IChromosome CreateNew()
        {
            return new CarChromosome(m_config);
        }

        
    }
}