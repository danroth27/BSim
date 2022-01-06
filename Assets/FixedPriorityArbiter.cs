using System.Collections.Generic;

namespace BSim
{
    internal class FixedPriorityArbiter : IArbiter
    {
        private readonly IRobotController robotController;
        private readonly IDictionary<IBehavior, int> behaviorPriorities = new Dictionary<IBehavior, int>();
        private IBehavior executingBehavior;
        private int executingPriority = -1;

        public FixedPriorityArbiter(IRobotController robotController)
        {
            this.robotController = robotController;
        }

        public void SetBehaviorPriority(IBehavior behavior, int priority)
        {
            behaviorPriorities[behavior] = priority;
        }

        public void SetBehaviorPrioritiesInOrder(IEnumerable<IBehavior> behaviors)
        {
            int i = 0;
            foreach (var behavior in behaviors)
            {
                SetBehaviorPriority(behavior, i++);
            }
        }

        public void ExecuteRobotCommand(RobotCommand command, IBehavior behavior)
        {
            var priority = behaviorPriorities[behavior];
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
