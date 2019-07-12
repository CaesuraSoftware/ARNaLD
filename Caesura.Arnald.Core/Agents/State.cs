
using System;

namespace Caesura.Arnald.Core.Agents
{
    using System.Collections.Generic;
    using System.Linq;
    using Caesura.Standard;
    
    public class State : IState
    {
        public IAgent Owner { get; set; }
        public IStateAtom InitialState { get; set; }
        private List<IStateAtom> Atoms { get; set; }
        private IStateAtom Current { get; set; }
        
        public State()
        {
            this.Atoms = new List<IStateAtom>();
        }
        
        public State(IAgent owner, IStateAtom initial) : this()
        {
            this.Owner = owner;
            this.InitialState = initial;
            this.TryAdd(initial); // add the initial state to the Atoms if it's not already there.
        }
        
        public static State LoadDefaults(IAgent owner)
        {
            var state = new State();
            var initstate = new StateAtom(state, StateAtomState.Begin, (self, message) => StateAtomState.End  );
            var endstate  = new StateAtom(state, StateAtomState.End  , (self, message) => StateAtomState.Begin);
            state.Owner = owner;
            state.Add(initstate);
            state.Add(endstate );
            state.InitialState = initstate;
            return state;
        }
        
        public Boolean TryAdd(IStateAtom atom)
        {
            var item = this.Find(x => x.Name == atom.Name);
            if (item is null)
            {
                this.Atoms.Add(atom);
                return true;
            }
            return false;
        }
        
        public void Add(IStateAtom atom)
        {
            var success = this.TryAdd(atom);
            if (!success)
            {
                throw new ElementExistsException();
            }
        }
        
        public void Add(String name, IStateAtomCallback callback)
        {
            var state = new StateAtom(this, name, callback);
            this.Add(state);
        }
        
        public Boolean Remove(IStateAtom atom)
        {
            return this.Atoms.Remove(atom);
        }
        
        public IStateAtom Find(Predicate<IStateAtom> predicate)
        {
            return this.Atoms.Find(predicate);
        }
        
        public Boolean TrySetState(String name)
        {
            var atom = this.Atoms.Find(x => x.Name == name);
            if (atom is null)
            {
                return false;
            }
            this.Current = atom;
            return true;
        }
        
        public void SetState(String name)
        {
            var success = this.TrySetState(name);
            if (!success)
            {
                throw new ArgumentException($"{nameof(IStateAtom)} is not present in this {nameof(State)} instance.");
            }
        }
        
        public void Next(IMessage message)
        {
            if (this.InitialState is null)
            {
                throw new InvalidOperationException($"{nameof(this.InitialState)} is null.");
            }
            if (this.Current is null)
            {
                this.Current = this.InitialState;
            }
            
            var result = this.Current.Call(message);
            if (result)
            {
                this.SetState(result.Value);
            }
            else
            {
                this.Current = this.InitialState;
            }
        }
        
        public void Dispose()
        {
            this.Dispose(null);
        }
        
        public void Dispose(IMessage message)
        {
            this.TrySetState(StateAtomState.Disposing);
            this.Next(message);
        }
    }
}
