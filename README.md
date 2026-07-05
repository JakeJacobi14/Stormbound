# Stormbound

A physics-based puzzle game built around electrostatic interactions, using Coulomb's Law to simulate realistic forces between charged objects. Built as a submission for a programming competition.

🎮 [Play the demo](https://jakejacobi.itch.io/stormbound)

## Overview

Stormbound challenges players to solve puzzles by manipulating charged objects, using real electrostatic physics rather than arbitrary game logic. Positive and negative charges attract and repel according to Coulomb's Law, and players must use these forces strategically to navigate obstacles and reach each level's goal.

## Features

- **Physics simulation grounded in Coulomb's Law**, calculating attraction/repulsion forces between charged objects based on charge magnitude and distance
- **Level progression system** with increasing puzzle complexity
- **Interactive obstacles** that respond to and influence the electrostatic simulation
- **Reusable puzzle mechanics** built with object-oriented design, allowing new puzzle elements to be added without reworking core systems
- **Unity Cloud integration** for build management and deployment

## How It Works

Each charged object in the game exerts a force on every other charged object nearby, calculated using:

```
F = k * (q1 * q2) / r^2
```

where `q1` and `q2` are the charges of the two objects, `r` is the distance between them, and `k` is Coulomb's constant (scaled for gameplay balance). These forces are applied every physics step, causing charged objects to naturally attract or repel as players adjust charge values, positions, or introduce new objects into the scene.

Levels are built around this core mechanic — using walls, gates, and movable charged objects to create puzzles that require the player to understand and predict how these forces will interact.

## Tech Stack

- **C#** — gameplay logic, physics calculations, puzzle systems
- **Unity** — game engine, rendering, level design
- **Unity Cloud** — build services and publishing pipeline

## Author

Jake Jacobi — [GitHub](https://github.com/JakeJacobi14) · [LinkedIn](https://www.linkedin.com/in/jakejacobi/)
