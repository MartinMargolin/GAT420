using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutonomousAgent : Agent
{
    [SerializeField] Perception perception;
    [SerializeField] Perception flockPerception;
    [SerializeField] ObstaclePerception obstaclePerception;
    [SerializeField] Steering steering;
    [SerializeField] AutonomousAgentData agentData;


    // Update is called once per frame
    void Update()
    {
        
        GameObject[] gameObjects = perception.GetGameObjects();

        if (gameObjects.Length == 0)
        {
            movement.ApplyForce(steering.Wander(this));
        }

        if (gameObjects.Length != 0)
        {
            Debug.DrawLine(transform.position, gameObjects[0].transform.position);

            movement.ApplyForce(steering.Seek(this, gameObjects[0]) * agentData.seekWeight);
            movement.ApplyForce(steering.Flee(this, gameObjects[0]) * agentData.fleeWeight);

           
        }

        if (obstaclePerception.IsObstacleInFront())
        {
            Vector3 direction = obstaclePerception.GetOpenDirection();
            movement.ApplyForce(steering.CalculateSteering(this, direction) * agentData.obstacleWeight);
        }

        gameObjects = flockPerception.GetGameObjects();
        if(gameObjects.Length != 0)
        {
            movement.ApplyForce(steering.Cohesion(this, gameObjects) * agentData.cohesionWeight);
        }

        transform.position = Utilities.Wrap(transform.position, new Vector3(-20, -20, -20), new Vector3(20, 20, 20));

    }
}
 