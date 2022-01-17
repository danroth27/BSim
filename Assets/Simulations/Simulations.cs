using BSim.Behaviors;
using System.Collections.Generic;
using UnityEngine;

namespace BSim.Simulations
{
    public static class Simulations
    {
        public static IList<Simulation> PrebuiltSimulations => new Simulation[]
        {
            Empty(), Collection(), Gizmo(), London(), BallPit()
        };

        public static Simulation Empty() => new Simulation { Name = "Empty", Objects = { new Robot() } };

        public static Simulation Collection()
        {
            var collection = new Simulation
            {
                Name = "Collection",
                Objects =
                {
                    new Robot
                    {
                        Position = new Vector2(-3, 0),
                        Behaviors =
                        {
                            new Cruise(),
                            new Home(),
                            new Avoid { Gain = -1 },
                            new AntiMoth(),
                            new DarkPush(),
                            new Escape()
                        }
                    },
                    new LightSource { Position = new Vector2(0, 0.5f) }
                }
            };

            for (int i = 0; i < 10; i++)
            {
                var randomPosition = Quaternion.Euler(0, 0, UnityEngine.Random.Range(-180f, 180f)) * Vector2.right * UnityEngine.Random.Range(1.5f, 3.5f) + new Vector3(0, 0.5f, 0);
                //var randomPosition = new Vector2(UnityEngine.Random.Range(-4.9f, 4.9f), UnityEngine.Random.Range(-4f, 5f));
                collection.Objects.Add(new Puck { Position = randomPosition });
            }

            return collection;
        }

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
                        Behaviors = { new Gizmo() }
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
