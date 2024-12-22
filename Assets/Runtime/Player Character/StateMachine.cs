/* Made by Oliver Beebe 2023 */

using System;
using System.Collections.Generic;

namespace Attrition.PlayerCharacter
{
    [Serializable]
    public class StateMachine 
    {
        #if UNITY_EDITOR
        [UnityEngine.SerializeField, UnityEngine.TextArea(3, 3)] private string debug;
        #endif

        public event Action<IState, IState> OnTransition;

        private readonly IState firstState;
        private readonly TransitionDictionary transitions;

        public IState currentState  { get; private set; }
        public IState previousState { get; private set; }
        public float stateDuration  { get; private set; }

        public StateMachine(IState firstState, TransitionDictionary transitions) 
        {
            this.firstState = firstState;
            this.transitions = transitions;

            this.Reset();
        }

        public void Reset() 
        {
            if (this.currentState != null) {
                this.currentState.Exit();
                this.currentState.ManualExit();
            }

            this.currentState = this.previousState = this.firstState;
            this.stateDuration = float.MaxValue;

            this.currentState.Enter();
        }

        public void ChangeState(IState toState) 
        {
            if (this.currentState != null) {
                this.currentState.Exit();
                this.currentState.ManualExit();
            }

            this.previousState = this.currentState;
            this.currentState = toState;
            this.stateDuration = 0;

            this.currentState.ManualEnter();
            this.currentState.Enter();
 
            this.OnTransition?.Invoke(this.previousState, this.currentState);
        }

        private void ChangeState(IState toState, Action behavior)
        {
            this.currentState?.Exit();

            behavior?.Invoke();

            this.previousState = this.currentState;
            this.currentState = toState;
            this.stateDuration = 0;

            this.currentState.Enter();

            this.OnTransition?.Invoke(this.previousState, this.currentState);
        }

        public void Update(float dt)
        {
            this.stateDuration += dt;
            this.currentState.Update();

            if (this.transitions.TryGetValue(this.currentState, out var stateTransitions)) {
                var transition = stateTransitions.Find(Transition.CanTransition);
                if (transition) this.ChangeState(transition.toState, transition.behavior);
            }

            #if UNITY_EDITOR
            this.debug = $"Current State : {this.currentState.GetType().Name}\nPrevious State : {this.previousState.GetType().Name}\nDuration : {this.stateDuration}";
            #endif
        }
    }

    public class TransitionDictionary : Dictionary<IState, List<Transition>> { }

    public delegate bool TransitionDelegate();

    public readonly struct Transition 
    {
        public static Predicate<Transition> CanTransition = transition => transition.canTransition.Invoke();

        public static implicit operator bool(Transition t) => t.exists;

        private readonly bool exists;
        private readonly TransitionDelegate canTransition;
        public  readonly IState toState;
        public  readonly Action behavior;

        public Transition(IState toState, TransitionDelegate canTransition)
            => (this.exists, this.toState, this.canTransition, this.behavior)
            =  (true, toState, canTransition, null);

        public Transition(IState toState, TransitionDelegate canTransition, Action behavior)
            => (this.exists, this.toState, this.canTransition, this.behavior)
            =  (true, toState, canTransition, behavior);
    }

    public interface IState
    {
        public void Enter();
        public void Update();
        public void Exit();
        public void ManualEnter();
        public void ManualExit();
    }

    public abstract class State<TContext> : IState
    {
        public State(TContext context) => this.context = context;

        protected readonly TContext context;

        public virtual void Enter () { }
        public virtual void Update() { }
        public virtual void Exit  () { }
        public virtual void ManualEnter() { }
        public virtual void ManualExit() { }
    }

    public abstract class SubState<TContext, TSuperState> : State<TContext> where TSuperState : State<TContext> 
    {
        public SubState(TContext context, TSuperState superState) : base(context) => this.superState = superState;

        protected readonly TSuperState superState;

        public override void Enter()  
        {
            base.Enter();
            this.superState.Enter();
        }
        public override void Update() 
        {
            this.superState.Update();
            base.Update();
        }
        public override void Exit()   
        {
            this.superState.Exit();
            base.Exit();
        }
        public override void ManualEnter() 
        {
            base.ManualEnter();
            this.superState.ManualEnter();
        }
        public override void ManualExit() 
        {
            this.superState.ManualExit();
            base.ManualExit();
        }
    }
}
