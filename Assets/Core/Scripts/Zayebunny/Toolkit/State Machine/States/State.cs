using System;
using System.Collections;
using Nocci.Zayebunny.Extensions;

namespace Nocci.Zayebunny.State_Machine.States
{
    /// <summary>
    ///     A generic empty state
    /// </summary>
    public class State : AbstractState
    {
        private readonly Action m_OnExecute;

        /// <param name="onExecute">An event that is invoked when the state is executed</param>
        public State(Action onExecute)
        {
            m_OnExecute = onExecute;
        }

        public override void Enter()
        {
            base.Enter();
            Execute().Run();
        }

        public override IEnumerator Execute()
        {
            yield return null;
            m_OnExecute?.Invoke();
        }
    }
}