using System.Collections.Generic;

namespace BSim
{
    public class FixedPriorityArbiter : IArbiter
    {
        private readonly IRobotController robotController;
        private IBehavior executingBehavior;
        private int executingPriority = -1;

        public FixedPriorityArbiter(IRobotController robotController)
        {
            this.robotController = robotController;
            BehaviorPriorities = new Dictionary<IBehavior, int>();
        }

        public IDictionary<IBehavior, int> BehaviorPriorities { get; }

        public void SetBehaviorPrioritiesInOrder(IEnumerable<IBehavior> behaviors)
        {
            int i = 0;
            foreach (var behavior in behaviors)
            {
                BehaviorPriorities[behavior] = i++;
            }
        }

        public void ExecuteRobotCommand(RobotCommand command, IBehavior behavior)
        {
            BehaviorPriorities.TryGetValue(behavior, out var priority);
            if (priority >= executingPriority && command != null)
            {
                executingPriority = priority;
                executingBehavior = behavior;
                robotController.ExecuteRobotCommand(command);
            }
            else if (command == null && behavior == executingBehavior)
            {
                executingPriority = -1;
                executingBehavior = null;
            }
        }
    }
}
