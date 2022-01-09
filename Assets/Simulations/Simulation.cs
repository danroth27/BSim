using BSim.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace BSim.Simulations
{
    public class Simulation
    {
        public string Name { get; set; }
        public SimulationOptions Options { get; set; }
        public List<SimulationObject> Objects { get; set; } = new List<SimulationObject>();
    }

    public abstract class SimulationObject
    {
        public Vector2 Position { get; set; } = Vector2.zero;
        public float Rotation { get; set; }
    }

    public class Robot : SimulationObject
    {
        public List<IBehavior> Behaviors { get; set; } = new List<IBehavior>();
    }

    public class Puck : SimulationObject { }

    public class LightSource : SimulationObject { }

    public class Wall : SimulationObject
    {
        public Vector2 EndPosition { get; set; }
    }

    public class SimulationOptions
    {
        public bool FantasyMode { get; set; } = true;

        public float Latency { get; set; } = 0f;
    }

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
                            new Gizmo()
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
                    }
                },
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
