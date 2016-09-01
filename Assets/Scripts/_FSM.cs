namespace _FSM
{
    /// <summary>
    /// Generic template for finite state machines.
    /// Will store possible states and transitions,
    /// as well as check for valid transitions
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class _FSM<T>
    {
        /// <summary>
        /// Adds a staate of generic type to the list of possible states.
        /// </summary>
        /// <param name="a_state"> State to add </param>
        public int AddState(T a_state)
        {
            if (!m_states.Contains(a_state))
            {
                m_states.Add(a_state);
                return 1;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Adds a valid transition from state A to state B to the list of valid transitions.
        /// </summary>
        /// <param name="a_state"> State the transistion is from</param>
        /// <param name="b_state"> State the transistion is to</param>
        /// <param name="a_handler"> Delegate to excuite upon success</param>
        public int AddTransition(T a_state, T b_state, System.Delegate a_handler)
        {
            string trans = a_state.ToString() + "->" + b_state.ToString();
            trans = trans.ToLower();

            if (!m_transitions.ContainsKey(trans))
            {
                m_transitions.Add(trans, a_handler);
                return 1;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Removes a state from the list of states as well as all valid transitions associated with it.
        /// </summary>
        /// <param name="a_state"> The state to be removed</param>
        public int RemoveState(T a_state)
        {
            System.Collections.Generic.List<string> keys = new System.Collections.Generic.List<string>();                         // new, empty list to store keys

            if (m_states.Contains(a_state))                                 // checks to make sure the state exist
            {                                                               // if so...
                m_states.Remove(a_state);                                   // remove it from the list of possible states

                foreach (System.Collections.Generic.KeyValuePair<string, System.Delegate> s in m_transitions) // and for every possible transition
                {                                                           //  
                    keys.Add(s.Key);                                        // add the key to the new list
                }

                foreach (string s in keys)                                  // and for every key in the list
                {                                                           //
                    if (s.Contains(a_state.ToString().ToLower()))           // check if the state is part of it
                    {                                                       // and if it is...
                        m_transitions.Remove(s);                            // remove it from the possible transitions
                    }
                }
                return 1;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Returns TRUE or FALSE if the transition from state A to state B is valid or not.
        /// </summary>
        /// <param name="a_state"> State the transistion is from</param>
        /// <param name="b_state"> State the transistion is to</param>
        /// <returns></returns>
        public bool CheckTransition(T a_state, T b_state)
        {
            string trans = a_state.ToString() + "->" + b_state.ToString();
            trans = trans.ToLower();
            return m_transitions.ContainsKey(trans);
        }

        /// <summary>
        /// Chacks if transition form current state to state b is valid.
        /// If so, excuite delegate associated with thats transition.
        /// </summary>
        /// <param name="b_state"> State the transistion is to</param>
        public bool MakeTransitionTo(T b_state)
        {
            string trans = m_currentState.ToString() + "->" + b_state.ToString();
            trans = trans.ToLower();

            if (m_transitions.ContainsKey(trans))
            {
                m_currentState = b_state;
                m_transitions[trans].DynamicInvoke();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// The current state of the object.
        /// </summary>
        public T m_currentState;
        /// <summary>
        /// The list of possible states for the object.
        /// </summary>
        private System.Collections.Generic.List<T> m_states = new System.Collections.Generic.List<T>();
        /// <summary>
        /// The list of possible transitions for the object and function to excuite upon success.
        /// </summary>
        private System.Collections.Generic.Dictionary<string, System.Delegate> m_transitions = new System.Collections.Generic.Dictionary<string, System.Delegate>();
    }

    public delegate void Handeler();
    public delegate void Handeler<t>();
    public delegate void Handeler<t, u>();
    public delegate void Handeler<t, u, v>();
}
//Eric Mouledoux
