using System.Collections.Generic;
using Gremlin.Net.Process.Traversal;

namespace GremlinDslImplementationInCSharp.GremlinExtensions
{
    public static class __Ext
    {
        public static GraphTraversal<object, IDictionary<string, E2>> ProjectBy<E2>(
            IDictionary<string, ITraversal> projectionMap)
        {
            return new GraphTraversal<object, object>().ProjectBy<object, object, E2>(projectionMap);
        }

        public static GraphTraversal<object, IDictionary<string, object>> ProjectBy(
            IDictionary<string, ITraversal> projectionMap)
        {
            return ProjectBy<object>(projectionMap);
        }

        public static GraphTraversal<object, object> Values(params string[] propertyKeys)
        {
            return __.Values<object>(propertyKeys);
        }
    }
}