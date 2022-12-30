# ReinforcementLearning_PhysicsCar
  Reinforcement learning approach of training physics based car which learns to avoid obstacles. Project was made in Unity, using MLAgents, reinfercement learning and C# scripts. The car recieves positive reward for driving into the goal and negative reward for driving into the walls, obstacles or for circling around.
#Overview
This is first project I've seen that takes on using Reinforcement learning for physics based car.
Car controller is based on wheel collider physics therefore it was tricky to train such car.
The main goal is for the car to reach certain point on the map while avoiding objects, obstacles and other cars.
![Безымянный](https://user-images.githubusercontent.com/114245364/210117518-d5981fa9-86f1-45f7-9542-05027fa2252b.png)
Model was set to train on static obstacles for 5 millions steps. After succesfully training, it was time to move on to the randomly generated positions of the obstacles which took over 60 million steps to train perfectly   

[INFO] MoveToGoal. Step: 4800000. Time Elapsed: 5777.964 s. Mean Reward: 97.196. Std of Reward: 20.315. Training.
[INFO] MoveToGoal. Step: 5050000. Time Elapsed: 5837.711 s. Mean Reward: 100.000. Std of Reward: 0.000. Training.  

Final goal was to train model with dynamic obstacles (red cars). This process took over 200 million steps.
![Image2](https://user-images.githubusercontent.com/114245364/210117744-b2201abc-449c-42ad-89e6-2ac92217c3d7.jpg)
# Observations
At each frame Neural Network was getting information about Vector3 coordinates of the car itself and the posotion of the goal area.
Also lidar was used in form of collision rays (Ray Perception sensor 3D) with such parametres:
![car](https://user-images.githubusercontent.com/114245364/210118113-9bec3b65-7081-4c39-91b7-0fb4d7a06772.PNG)
# Behaviour parameters
![car2](https://user-images.githubusercontent.com/114245364/210118174-3c25cdca-90c7-433d-a239-4ed6eb47cd67.PNG)
Stacked vectors should be set to 1 in case of static obstacles and 4-6 in case of dynamic obstacles. The latter amount allows for NN to look at multiple states at the same time therefore seeing and predicting moves along with training progress.
# YouTube Video
Here's a [video](https://youtu.be/M0hJMj7G018) of model running on inference mode with fully trained model!
