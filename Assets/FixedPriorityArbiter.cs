using System.Collections.Generic;

namespace BSim
{
    public class FixedPriorityArbiter : IArbiter
    {
        private readonly IRobotController robotController;
        private IBehavior executingBehavior;
        private int executingPriority = -1;

        public FixedPriorityArbiter(IRobotController robotController, IEnumerable<IBehavior> behaviors)
        {
            this.robotController = robotController;
            SetBehaviorPrioritiesInOrder(behaviors);
        }

        public IDictionary<IBehavior, int> BehaviorPriorities = new Dictionary<IBehavior, int>();

        public void SetBehaviorPrioritiesInOrder(IEnumerable<IBehavior> behaviors)
        {
            BehaviorPriorities.Clear();
            int i = 0;
            foreach (var behavior in behaviors)
            {
                BehaviorPriorities[behavior] = i++;
                behavior.SetArbiter(this);
            }
        }

        public void ExecuteRobotCommand(RobotCommand command, IBehavior behavior)
        {
            BehaviorPriorities.TryGetValue(behavior, out var priority);
            if (priority >= executingPriority && command != null)
            {
                executingPriority = priority;
                executingBehavior = behavior;
                robotController.ExecuteRobotCommand(command, behavior);
            }
            else if (command == null && behavior == executingBehavior)
            {
                executingPriority = -1;
                executingBehavior = null;
            }
        }
    }
}
