using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

namespace GeneticSharp.Runner.UnityApp.Car
{
    public class CarController : MonoBehaviour
    {
        private static Rect? s_lastCameraRect;
        private PolygonCollider2D m_polygon;
        private Rigidbody2D m_rb;
        private TextMesh m_fitnessText;
        public FollowChromosomeCam m_cam;
        private GameObject m_wheels;
        private CarSampleConfig m_config;
        private float m_startTime;
        private float m_currentVelocity;
        private float m_lastDistance;
        private float m_lastTime;

        public UnityEngine.Object WheelPrefab;
        public float VectorMagnitudeMass = 2f;

        public float Distance { get; private set; }
        public float DistanceTime { get; set; }
        public CarChromosome Chromosome { get; private set; }

        private void Awake()
        {
            m_polygon = GetComponent<PolygonCollider2D>();
            m_rb = GetComponent<Rigidbody2D>();
            m_fitnessText = GetComponentInChildren<TextMesh>();
            m_wheels = transform.Find("Wheels").gameObject;
        }

        void Start()
        {
            try
            {
                m_cam = GameObject.Find("SimulationGrid")
                      .GetComponent<SimulationGrid>()
                      .AddChromosome(gameObject);

            }
            catch (System.NullReferenceException ex)
            {
                
                m_cam = GameObject.Find("SimulationGrid")
                      .GetComponent<EvaluationGrid>()
                      .AddChromosome(gameObject);
                
            }
        }

        private void storeMetrics(float vel){
            if(!Double.IsNaN(vel)){
                // Calculate the acceleration based on the previous velocity
                if (Chromosome.Accelerations.Count > 1) Chromosome.Accelerations.Add((vel - Chromosome.Velocities[Chromosome.Velocities.Count - 1]) / Time.deltaTime);
                else Chromosome.Accelerations.Add(vel / Time.deltaTime);

                // Store velocity value
                Chromosome.Velocities.Add(vel);
            }

            // Update the mean values to always be up to date
            Chromosome.SumVelocities = Chromosome.Velocities.Sum();
            Chromosome.SumAccelerations = Chromosome.Accelerations.Sum();

            float wheelsForce = 0f;
            foreach (Transform wheelTransform in m_wheels.transform)
            {
                GameObject wheelGO = wheelTransform.gameObject;
                // Get the collider radius
                float colliderRadius = wheelGO.GetComponent<CircleCollider2D>().radius;

                // Calculate the real radius considering scaling
                float realRadius = wheelTransform.localScale.x * colliderRadius;

                HingeJoint2D hinge = wheelGO.GetComponent<Rigidbody2D>().GetComponent<HingeJoint2D>();
                wheelsForce += Mathf.Abs(hinge.GetMotorTorque(Time.deltaTime) * realRadius);
            }

            Chromosome.Forces.Add(wheelsForce);
            Chromosome.SumForces = Chromosome.Forces.Sum();
        }


      	public IEnumerator CheckTimeout()
        {
            var intervalLastDistance = Distance;
            var intervalLastTime = Time.time;

            yield return new WaitForSeconds(m_config.WarmupTime);
       
            do
            {
                float vel = CalculateVelocity(ref intervalLastDistance, ref intervalLastTime);
                storeMetrics(vel);

                // Check if car as the min velocity expected in the inteval.
                if (DistanceTime > 0 && vel < m_config.MinVelocity)
                {
                    StopEvaluation();
                    break;
                }
                yield return new WaitForSeconds(m_config.MinVelocityCheckTime);
            } while (true);
        }

        private float CalculateVelocity(ref float lastDistance, ref float lastTime)
        {
            var result = (Distance - lastDistance) / (Time.time - lastTime);
            lastDistance = Distance;
            lastTime = Time.time;
            m_currentVelocity = result;
            return result;
        }

        private void StopEvaluation()
        {
            StopCoroutine("CheckTimeout");
            StopCoroutine("AwaitEvaluation");
            
            m_rb.Sleep();
            m_rb.isKinematic = true;

            foreach (var rb in m_wheels.GetComponentsInChildren<Rigidbody2D>())
            {
                rb.Sleep();
                rb.isKinematic = true;
            }

            foreach (var joint in m_wheels.GetComponentsInChildren<HingeJoint2D>())
            {
                joint.useMotor = false;
            }

            Chromosome.Evaluated = true;
            m_cam.StopFollowing();
        }

        void OnDestroy()
        {
            StopEvaluation();
        }

		private void OnCollisionEnter2D(Collision2D collision)
		{
            var other = collision.gameObject;

			if (other.tag == "DeadZone")
            {
                // When reach the road dead end, use the dead end position as max distance.
                if (transform.position.x > m_config.RoadMiddle && other.name.Equals("dead-end"))
                {
                    Chromosome.RoadCompleted = true;
                    Chromosome.Distance = other.transform.position.x;
                    Chromosome.EllapsedTime = Time.time - m_startTime;
                    UpdateFitnessText();
                }
                
                var intervalLastDistance = Distance;
                var intervalLastTime = Time.time;

                float vel = CalculateVelocity(ref intervalLastDistance, ref intervalLastTime);
                storeMetrics(vel);
                StopEvaluation();
            }
		}

     	private void Update()
		{
            CheckMaxDistance();
            UpdateFitnessText();
        }

        private void UpdateFitnessText()
        {
            m_fitnessText.text = FormatFitnessText(Chromosome.Distance, Chromosome.EllapsedTime);
            m_fitnessText.transform.rotation = Quaternion.identity;
            // 2024
            m_fitnessText.fontSize = 80;
            m_fitnessText.anchor = TextAnchor.LowerLeft;

        }

        private string FormatFitnessText(float distance, float time)
        {
            return time > 0
                ? $"Fitness: {Chromosome.Fitness}\n {distance:N2}m v:" +
                $"{CalculateVelocity(ref m_lastDistance, ref m_lastTime):N2}m/s \n time:{time:N2}\n" +
                $"Mass:{Chromosome.CarMass}\n" + "SumForces:"+ Chromosome.SumForces 
                   : "0m - 0m/s";
            // $"Fitness: {Chromosome.Fitness}\n {distance:N2}m - {(distance / time):N2}m/s\n" +
            //$"{CalculateVelocity(ref m_lastDistance, ref m_lastTime):N2}m/s time:{time:N2}\n" +
            //$"Mass:{Chromosome.CarMass}\n Wheels Energy: {Chromosome.MeanWheelsEnergy}"
            //       : "0m - 0m/s";
        }

        private void CheckMaxDistance()
        {
            Distance = transform.position.x;
            DistanceTime = Time.time - m_startTime;

            if (Distance > Chromosome.Distance)
            {
                Chromosome.Distance = Distance;
                Chromosome.EllapsedTime = DistanceTime;
            }
        }

		public void SetChromosome(CarChromosome chro, CarSampleConfig config)
        {
            Chromosome = chro;
            
            Distance = 0;
            DistanceTime = 0;
            m_startTime = Time.time;
            transform.rotation = Quaternion.identity;
           
            m_config = config;
            m_rb.isKinematic = false;
            m_rb.velocity = Vector2.zero;
            m_rb.angularVelocity = 0;

            var phenotypes = Chromosome.GetPhenotypes();
            m_polygon.points = phenotypes.Select(p => p.Vector).ToArray();
            var wheelsMass = 0f;
            var countWheels = 0;
            for (int i = 0; i < phenotypes.Length; i++)
            {
                var p = phenotypes[i];
                PrepareWheel(i, m_polygon.points[p.WheelIndex], p.WheelRadius);
                if(p.WheelRadius > 0)
                {
                    countWheels++;
                }
                wheelsMass += p.WheelRadius;
            }
            Chromosome.NumberOfWheels = countWheels;
            // The car mass should be greater than wheels sum mass, because the WheelJoint2d get crazy otherwise.
            // If we comment the line bellow and enable the car mass should be greater than wheels sum mass, because the WheelJoint2d get crazy otherwise.
            m_rb.mass = 1 +  m_polygon.points.Sum(p => p.magnitude) * VectorMagnitudeMass + wheelsMass;

            Chromosome.CarMass = m_rb.mass;

            if (m_cam != null)
            {
                m_cam.StartFollowing(gameObject);
            }

            // Init the list containing all the velocities, accel. and kinetic energies
            Chromosome.Distance = 0;
            Chromosome.EllapsedTime = 0;
            Chromosome.RoadCompleted = false;
            Chromosome.Evaluated = false;

            Chromosome.Velocities = new List<float>();
            Chromosome.SumVelocities = 0f;

            Chromosome.Accelerations = new List<float>();
            Chromosome.SumAccelerations = 0f;

            Chromosome.Forces = new List<float>();
            Chromosome.SumForces = 0f;
      
            StartCoroutine("CheckTimeout");
            StartCoroutine("AwaitEvaluation");
        }

        public IEnumerator AwaitEvaluation()
        {
            
            yield return new WaitForSeconds(1);
       
            do
            {
                yield return new WaitForSeconds(1);
            } while (Time.time-m_startTime < m_config.EvaluationTimeLimit && !Chromosome.Evaluated);
            //if(Time.time-m_startTime>=m_config.EvaluationTimeLimit) Debug.Log("Evaluation timeout, ellapsed time: " + (Time.time - m_startTime));
            StopEvaluation();
        }


        private GameObject PrepareWheel(int index, Vector2 anchorPosition, float radius)
        {
            GameObject wheel;
            Transform wheelTransform = m_wheels.transform.childCount > index
                                               ? m_wheels.transform.GetChild(index)
                                               : null;

            if (wheelTransform == null)
            {
                wheel = Instantiate(WheelPrefab, Vector3.zero, Quaternion.identity) as GameObject;
                wheel.transform.SetParent(m_wheels.transform, false);
            }
            else
            {
                wheel = wheelTransform.gameObject;    
            }

            var joint = wheel.GetComponent<HingeJoint2D>();

            joint.attachedRigidbody.isKinematic = false;
            joint.useMotor = true;
            joint.connectedBody = m_rb;
            joint.connectedAnchor = anchorPosition;
            //motor speed é a target motor speed, ou seja, a velocidade que o motor tenta atingir
            //maxMotorTorque é a força máxima que o motor pode aplicar
            joint.motor = new JointMotor2D { motorSpeed = m_config.MaxWheelSpeed * radius, maxMotorTorque = joint.motor.maxMotorTorque };
            joint.enabled = true;

            wheel.transform.localScale = new Vector3(radius, radius, 1);
            wheel.transform.localPosition = anchorPosition;
            wheel.SetActive(true);

            return wheel;
        }
	}
}