using System.Collections.Generic;

namespace ProjectFileAnalyzer
{
    /// <summary>
    /// 
    /// 
    /// BFS:
    /// 
    /// var startVertex = ...;
    /// startVertex.PathLength = 0;
    /// Queue q = ...;
    /// q.Enqueue(startVertex);
    /// 
    /// while (q.Any()) {
    ///     var current = q.Dequeue();
    ///     foreach (var vertex in current.AdjacentVertices) {
    ///         if (vertext.PathLength == -1) {
    ///             vertex.PathLength = current.PathLength + 1;
    ///             v.BackVertex = current;
    ///             q.Enqueue(vertex);
    ///         }
    ///     }
    /// }
    /// 
    /// 
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Vertex<T>
    {
        public T Value { get; set; }
        public UniqueVertices AdjacentVertices { get; set; } = new UniqueVertices();

        /// <summary>
        /// -1 will also serve as WasExplored information
        /// </summary>
        public int PathLength { get; set; } = -1;

        public Vertex<T> BackVertex { get; set; } = null;

        public Vertex(T value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        #region UniqueVertices

        public class UniqueVertices : List<Vertex<T>>
        {
            new public void Add(Vertex<T> v)
            {
                if (!Contains(v))
                {
                    base.Add(v);
                }
            }
        }

        #endregion
    }
}
