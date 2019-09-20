// Author: Ben Huenemann

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpreadsheetUtilities
{

    /// <summary>
    /// (s1,t1) is an ordered pair of strings
    /// t1 depends on s1; s1 must be evaluated before t1
    /// 
    /// A DependencyGraph can be modeled as a set of ordered pairs of strings.  Two ordered pairs
    /// (s1,t1) and (s2,t2) are considered equal if and only if s1 equals s2 and t1 equals t2.
    /// Recall that sets never contain duplicates.  If an attempt is made to add an element to a 
    /// set, and the element is already in the set, the set remains unchanged.
    /// 
    /// Given a DependencyGraph DG:
    /// 
    ///    (1) If s is a string, the set of all strings t such that (s,t) is in DG is called dependents(s).
    ///        (The set of things that depend on s)    
    ///        
    ///    (2) If s is a string, the set of all strings t such that (t,s) is in DG is called dependees(s).
    ///        (The set of things that s depends on) 
    //
    // For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}
    //     dependents("a") = {"b", "c"}
    //     dependents("b") = {"d"}
    //     dependents("c") = {}
    //     dependents("d") = {"d"}
    //     dependees("a") = {}
    //     dependees("b") = {"a"}
    //     dependees("c") = {"a"}
    //     dependees("d") = {"b", "d"}
    /// </summary>
    public class DependencyGraph
    {
        private IDictionary<string, HashSet<string>> dependents;
        private IDictionary<string, HashSet<string>> dependees;

        /// <summary>
        /// Creates an empty DependencyGraph.
        /// </summary>
        public DependencyGraph()
        {
            dependents = new Dictionary<string, HashSet<string>>();
            dependees = new Dictionary<string, HashSet<string>>();
        }


        /// <summary>
        /// The number of ordered pairs in the DependencyGraph.
        /// </summary>
        public int Size
        {
            get;
            private set;
        }


        /// <summary>
        /// The size of dependees(s).
        /// This property is an example of an indexer.  If dg is a DependencyGraph, you would
        /// invoke it like this:
        /// dg["a"]
        /// It should return the size of dependees("a")
        /// </summary>
        public int this[string s]
        {
            get
            {
                //Returns zero if the key doesn't exist since there are no dependees
                if (!dependees.ContainsKey(s))
                    return 0;
                return dependees[s].Count;
            }
        }


        /// <summary>
        /// Reports whether dependents(s) is non-empty.
        /// </summary>
        public bool HasDependents(string s)
        {
            //Returns false if the key doesn't exist since it has no dependents
            if (!dependents.ContainsKey(s))
                return false;
            return dependents[s].Count != 0;
        }


        /// <summary>
        /// Reports whether dependees(s) is non-empty.
        /// </summary>
        public bool HasDependees(string s)
        {
            if (!dependees.ContainsKey(s))
                return false;
            return dependees[s].Count != 0;
        }


        /// <summary>
        /// Enumerates dependents(s).
        /// </summary>
        public IEnumerable<string> GetDependents(string s)
        {
            //Creates a new hash set if the key doesn't exist so it can return that hash set
            if (!dependents.ContainsKey(s))
                return new HashSet<string>();

            return dependents[s].ToList();
        }

        /// <summary>
        /// Enumerates dependees(s).
        /// </summary>
        public IEnumerable<string> GetDependees(string s)
        {
            if (!dependees.ContainsKey(s))
                return new HashSet<string>();

            return dependees[s].ToList();
        }


        /// <summary>
        /// <para>Adds the ordered pair (s,t), if it doesn't exist</para>
        /// 
        /// <para>This should be thought of as:</para>   
        /// 
        ///   t depends on s
        ///
        /// </summary>
        /// <param name="s"> s must be evaluated first. T depends on S</param>
        /// <param name="t"> t cannot be evaluated until s is</param>        /// 
        public void AddDependency(string s, string t)
        {
            //If it doesn't already have the key or doesn't have the connection, it increments the size
            if (!dependents.ContainsKey(s) || !dependents[s].Contains(t))
                Size++;

            //It creates creates a new key with the right hash set if that key doesn't exist. Otherwise it just adds the value to the hashset.
            if (!dependents.ContainsKey(s))
                dependents.Add(s, new HashSet<string> { t });
            else if (!dependents[s].Contains(t))
                dependents[s].Add(t);

            //Same thing for dependees
            if (!dependees.ContainsKey(t))
                dependees.Add(t, new HashSet<string> { s });
            else if (!dependees[t].Contains(s))
                dependees[t].Add(s);
        }


        /// <summary>
        /// Removes the ordered pair (s,t), if it exists
        /// </summary>
        /// <param name="s"></param>
        /// <param name="t"></param>
        public void RemoveDependency(string s, string t)
        {
            //Returns if the pair doesn't exist
            if (!dependents.ContainsKey(s) || !dependents[s].Contains(t))
                return;

            dependents[s].Remove(t);
            dependees[t].Remove(s);

            Size--;

            //Removes the key if it's empty
            if (dependents[s].Count == 0)
                dependents.Remove(s);
            if (dependees[t].Count == 0)
                dependees.Remove(t);
        }


        /// <summary>
        /// Removes all existing ordered pairs of the form (s,r).  Then, for each
        /// t in newDependents, adds the ordered pair (s,t).
        /// </summary>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {
            //Adds a key if there isn't one but otherwise just clears the key
            List<string> DependentsList = new List<string>(GetDependents(s));

            //Uses toList to make a clone so it doesn't edit the iterator
            foreach (string d in DependentsList)
                RemoveDependency(s, d);

            //Adds each element of the collection to the hash set
            foreach (string t in newDependents)
                AddDependency(s, t);
        }


        /// <summary>
        /// Removes all existing ordered pairs of the form (r,s).  Then, for each 
        /// t in newDependees, adds the ordered pair (t,s).
        /// </summary>
        public void ReplaceDependees(string s, IEnumerable<string> newDependees)
        {
            List<string> DependeesList = new List<string>(GetDependees(s));

            foreach (string d in DependeesList)
                    RemoveDependency(d, s);

            foreach (string t in newDependees)
                AddDependency(t, s);
        }

    }

}

