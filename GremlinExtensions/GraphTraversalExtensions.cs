using System;
using System.Collections.Generic;
using System.Linq;
using Gremlin.Net.Process.Traversal;

namespace GremlinDslImplementationInCSharp.GremlinExtensions
{
    public static class GraphTraversalExtensions
    {
        public static GraphTraversal<S, IDictionary<string, E2>> ProjectBy<S, E, E2>(
            this GraphTraversal<S, E> tIn, IDictionary<string, ITraversal> projectionMap)
        {
            if (tIn is null)
                throw new ArgumentNullException(nameof(tIn));
            if (projectionMap is null)
                throw new ArgumentNullException(nameof(projectionMap));
            if (!projectionMap.Any())
                throw new ArgumentException("The projection map should contain at least one item.", nameof(projectionMap));

            GraphTraversal<S, IDictionary<string, E2>> tOut = tIn.Project<E2>(
                projectionMap.First().Key, projectionMap.Skip(1).Select(kv => kv.Key).ToArray());

            foreach (KeyValuePair<string, ITraversal> keyValue in projectionMap)
			{
				if (keyValue.Value is null)
					tOut = tOut.By(keyValue.Key);
				else
					tOut = tOut.By(keyValue.Value);
			}

            return tOut;
        }

        public static GraphTraversal<S, IDictionary<string, object>> ProjectBy<S, E>(
            this GraphTraversal<S, E> tIn, IDictionary<string, ITraversal> projectionMap)
        {
            return ProjectBy<S, E, object>(tIn, projectionMap);
        }
    }
}