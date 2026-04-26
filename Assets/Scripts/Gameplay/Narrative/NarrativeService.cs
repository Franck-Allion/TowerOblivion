using System;
using System.Collections.Generic;
using System.Linq;
using TowerOblivion.Core;

namespace TowerOblivion.Gameplay.Narrative
{
    public sealed class NarrativeService : INarrativeService
    {
        private readonly NarrativeState _state;
        private readonly IEventBus _eventBus;

        public NarrativeService(NarrativeState state, IEventBus eventBus)
        {
            _state = state ?? throw new ArgumentNullException(nameof(state));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        }

        // --- Boolean Flags ---

        public bool GetFlag(NarrativeFlagId flagId)
        {
            return GetFlagFromList(_state.Flags, flagId);
        }

        public void SetFlag(NarrativeFlagId flagId, bool value)
        {
            SetFlagInList(_state.Flags, flagId, value);
        }

        public void ToggleFlag(NarrativeFlagId flagId)
        {
            var flag = _state.Flags.FirstOrDefault(f => f.FlagId.Equals(flagId));
            if (flag == null)
            {
                // Toggle from default (false) to true
                SetFlag(flagId, true);
                return;
            }

            var newValue = !flag.Value;
            flag.Value = newValue;
            _eventBus.Publish(new NarrativeFlagChanged(flagId, newValue));
        }

        // --- Consequences (Persistent Cross-Run Flags) ---

        public bool GetConsequence(NarrativeFlagId flagId)
        {
            return GetFlagFromList(_state.Consequences, flagId);
        }

        public void SetConsequence(NarrativeFlagId flagId, bool value)
        {
            SetFlagInList(_state.Consequences, flagId, value);
        }

        // --- Integer Counters ---

        public int GetCounter(NarrativeFlagId counterId)
        {
            var counter = _state.Counters.FirstOrDefault(c => c.CounterId.Equals(counterId));
            return counter?.Value ?? 0;
        }

        public void SetCounter(NarrativeFlagId counterId, int value)
        {
            var counter = _state.Counters.FirstOrDefault(c => c.CounterId.Equals(counterId));
            if (counter == null)
            {
                counter = new NarrativeCounterState { CounterId = counterId, Value = value };
                _state.Counters.Add(counter);
                _eventBus.Publish(new NarrativeCounterChanged(counterId, value));
                return;
            }

            if (counter.Value != value)
            {
                counter.Value = value;
                _eventBus.Publish(new NarrativeCounterChanged(counterId, value));
            }
        }

        public void AdjustCounter(NarrativeFlagId counterId, int amount)
        {
            var current = GetCounter(counterId);
            // Use long for overflow protection during calculation then clamp back to int range
            long next = (long)current + amount;
            int clamped = (int)Math.Max(int.MinValue, Math.Min(int.MaxValue, next));
            
            SetCounter(counterId, clamped);
        }

        // --- Helpers ---

        private bool GetFlagFromList(List<NarrativeFlagState> list, NarrativeFlagId id)
        {
            var flag = list.FirstOrDefault(f => f.FlagId.Equals(id));
            return flag?.Value ?? false;
        }

        private void SetFlagInList(List<NarrativeFlagState> list, NarrativeFlagId id, bool value)
        {
            var flag = list.FirstOrDefault(f => f.FlagId.Equals(id));
            if (flag == null)
            {
                flag = new NarrativeFlagState { FlagId = id, Value = value };
                list.Add(flag);
                _eventBus.Publish(new NarrativeFlagChanged(id, value));
                return;
            }

            if (flag.Value != value)
            {
                flag.Value = value;
                _eventBus.Publish(new NarrativeFlagChanged(id, value));
            }
        }
    }
}
