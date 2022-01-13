using BSim.Behaviors;
using System.Collections.Generic;
using UnityEngine;

namespace BSim.Simulations
{
    public static class Simulations
    {
        public static IList<Simulation> PrebuiltSimulations => new Simulation[]
        {
            Empty(), Gizmo(), London(), BallPit()
        };

        public static Simulation Empty() => new Simulation { Name = "Empty" };

        public static Simulation Gizmo()
        {
            return new Simulation
            {
                Name = "Gizmo",
                Objects =
                {
                    new Robot
                    {
                        Position = new Vector2(-3, 0),
                        Behaviors =
                        {
                            new Gizmo() { Gain = 0.25f }
                        }
                    },
                    new LightSource
                    {
                        Position = new Vector2(3, 0)
                    }
                },
                Options = new SimulationOptions
                {
                    Latency = 1
                }
            };
        }

        public static Simulation London()
        {
            return new Simulation
            {
                Name = "London",
                Objects =
                {
                    new Robot
                    {
                        Position = new Vector2(-4, -3.6f),
                        Behaviors =
                        {
                            new London()
                        }
                    },
                    new Wall {
                        Position = new Vector2(0, -2.5f),
                        Length = 6
                    },
                    new Wall
                    {
                        Position = new Vector2(3, 0.5f),
                        Rotation = 90,
                        Length = 6
                    },
                    new Wall
                    {
                        Position = new Vector2(0, 3.5f),
                        Length = 6
                    },
                    new Wall
                    {
                        Position = new Vector2(-3, 0.5f),
                        Rotation = 90,
                        Length = 6
                    }
                }
            };
        }

        public static Simulation BallPit()
        {
            var ballPit = new Simulation
            {
                Name = "Ball Pit",
                Objects =
                {
                    new Robot()
                    {
                        Position = new Vector2(-4, 0),
                        Behaviors =
                        {
                            new Cruise(),
                            new Escape()
                        }
                    }

                }
            };

            for (int i = 0; i < 1000; i++)
            {
                //var randomPosition = Quaternion.Euler(0, 0, UnityEngine.Random.Range(-180f, 180f)) * Vector2.right * UnityEngine.Random.Range(0, 4f);
                var randomPosition = new Vector2(UnityEngine.Random.Range(-4.9f, 4.9f), UnityEngine.Random.Range(-4f, 5f));
                ballPit.Objects.Add(new Puck { Position = randomPosition });
            }
            return ballPit;
        }
    }
}
