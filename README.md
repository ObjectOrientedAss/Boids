# Boids
This is a small sandbox for testing the boids simulation algorithms. Project is setupped in Visual Studio, and is using Fast2D and OpenTK to handle rendering in a window and vectors.

## How does it work?
The boids algorithm is composed by three different algorithms:

- Alignment
- Cohesion
- Separation

Each algorithm takes in account something and gives a different outcome that influences the final movement of the boid.
Each alogrithm has a weight, which allows to increase/decrease its influence on the final movement of the boid.
Each algorithm has a radius, which allows to increase/decrease the number of neighbours taken in account in the algorithms.

##Alignment
The boid takes in account its neighbours direction and orientates toward their average heading.

##Cohesion
The boid takes in account its neighbours average position and moves in that direction.

##Separation
The boid takes in account its neighbours individual position and moves in the opposite direction.
