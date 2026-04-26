using TowerOblivion.Gameplay.Content;

namespace TowerOblivion.Gameplay.Narrative
{
    public sealed class NarrativeEventOrchestrator
    {
        private readonly INarrativeService _narrativeService;

        public NarrativeEventOrchestrator(INarrativeService narrativeService)
        {
            _narrativeService = narrativeService;
        }

        public bool CanTrigger(NarrativeEventContentDefinition definition)
        {
            if (definition == null)
            {
                return false;
            }

            // Evaluate boolean requirement
            if (!string.IsNullOrEmpty(definition.RequiredFlagId.Value))
            {
                if (_narrativeService.GetFlag(definition.RequiredFlagId) != definition.RequiredFlagValue)
                {
                    return false;
                }
            }

            // Evaluate counter requirement
            if (!string.IsNullOrEmpty(definition.RequiredCounterId.Value))
            {
                if (_narrativeService.GetCounter(definition.RequiredCounterId) < definition.RequiredCounterMinValue)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
