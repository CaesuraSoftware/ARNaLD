
using System;

namespace Caesura.Arnald.Core.Agents
{
    using System.Collections.Generic;
    using Caesura.Standard;
    
    public class State : IState
    {
        private readonly Object _stateLock = new Object();
        public IAgent HostAgent { get; set; }
        public IStateAtom InitialState { get; set; }
        private List<IStateAtom> Atoms { get; set; }
        private IStateAtom Current { get; set; }
        
        public State()
        {
            this.Atoms = new List<IStateAtom>();
        }
        
        public State(IAgent owner) : this()
        {
            this.HostAgent = owner;
        }
        
        public State(IAgent owner, IStateAtom initial) : this(owner)
        {
            this.InitialState = initial;
            this.TryAdd(initial); // add the initial state to the Atoms if it's not already there.
        }
        
        public static State LoadDefaults(IAgent owner)
        {
            var defaultstate = new State();
            var initstate = new StateAtom(defaultstate, StateAtomState.Begin, (state, message) => StateAtomState.End  );
            var endstate  = new StateAtom(defaultstate, StateAtomState.End  , (state, message) => StateAtomState.Begin);
            defaultstate.HostAgent = owner;
            defaultstate.Add(initstate);
            defaultstate.Add(endstate );
            defaultstate.InitialState = initstate;
            return defaultstate;
        }
        
        public Boolean TryAdd(IStateAtom atom)
        {
            lock (this._stateLock)
            {
                var item = this.Find(x => x.Name == atom.Name);
                if (item is null)
                {
                    
                    this.Atoms.Add(atom);
                    return true;
                }
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
            lock (this._stateLock)
            {
                return this.Atoms.Remove(atom);
            }
        }
        
        public IStateAtom Find(Predicate<IStateAtom> predicate)
        {
            lock (this._stateLock)
            {
                return this.Atoms.Find(predicate);
            }
        }
        
        public Boolean TrySetState(String name)
        {
            var atom = this.Find(x => x.Name == name);
            if (atom is null)
            {
                return false;
            }
            lock (this._stateLock)
            {
                this.Current = atom;
            }
            return true;
        }
        
        public void SetState(String name)
        {
            var success = this.TrySetState(name);
            if (!success)
            {
                throw new ArgumentException($"{nameof(IStateAtom)} \"{name}\" is not present in this {nameof(State)} instance.");
            }
        }
        
        public void Next(IMessage message)
        {
            lock (this._stateLock)
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
