using System;
using System.Collections.Generic;
using Gremlin.Net.Driver;
using Gremlin.Net.Driver.Remote;
using Gremlin.Net.Process.Traversal;
using GremlinDslImplementationInCSharp.GremlinExtensions;

namespace GremlinDslImplementationInCSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var client = new GremlinClient(new GremlinServer("localhost", 8182)))
            {
                GraphTraversalSource g = AnonymousTraversalSource.Traversal().WithRemote(new DriverRemoteConnection(client));
                
                PopulateGraph(g);
                IDictionary<string, object> projectionResults1 = GetPostUsingStandardProject(g);
                IDictionary<string, object> projectionResults2 = GetPostUsingProjectBy(g);
            }
        }

        static void PopulateGraph(GraphTraversalSource g)
        {
            g.AddV("post")
                .Property("slug", "post-1")
                .Property("author", "Author 1")
                .Property("publishedOn", new DateTime(2021, 3, 19, 17, 7, 52))
                .Property("lastUpdate", new DateTime(2021, 4, 2, 8, 45, 3))
                .Property("title", "Post 1")
                .Property("body", "Body 1")
                .As("p1")
                
            .AddV("comment")
                .Property("userName", "User 1")
                .Property("postedOn", new DateTime(2021, 4, 15, 20, 59, 50))
                .Property("title", "Comment 1 Title")
                .Property("body", "Comment 1 body")
                .Property("wasEdited", false)
                .As("c1")
            .AddE("hasComment").From("p1").To("c1")
                
            .AddV("comment")
                .Property("userName", "User 2")
                .Property("postedOn", new DateTime(2021, 4, 18, 16, 48, 44))
                .Property("title", "Comment 2 Title")
                .Property("body", "Comment 2 body")
                .Property("wasEdited", true)
                .As("c2")
            .AddE("hasComment").From("p1").To("c2")
                
            .AddV("comment")
                .Property("userName", "User 3")
                .Property("postedOn", new DateTime(2021, 4, 22, 4, 12, 21))
                .Property("title", "Comment 3 Title")
                .Property("body", "Comment 3 body")
                .Property("wasEdited", false)
                .As("c3")
            .AddE("hasComment").From("p1").To("c3")
                
            .Iterate();
        }

        static IDictionary<string, object> GetPostUsingStandardProject(GraphTraversalSource g)
        {
            return g.V().Has("post", "slug", "post-1")
                .Project<object>("id", "slug", "author", "publishedOn", "lastUpdate", "title", "body", "comments")
                .By(T.Id)
                .By("slug")
                .By("author")
                .By("publishedOn")
                .By("lastUpdate")
                .By("title")
                .By("body")
                .By(
                    __.Out("hasComment")
                    .Project<object>("id", "userName", "postedOn", "title", "body", "wasEdited")
                    .By(T.Id)
                    .By("userName")
                    .By("postedOn")
                    .By("title")
                    .By("body")
                    .By("wasEdited")
                    .Fold()
                ).Next();
        }

        static IDictionary<string, object> GetPostUsingProjectBy(GraphTraversalSource g)
        {
            return g.V().Has("post", "slug", "post-1")
                .ProjectBy(new Dictionary<string, ITraversal>
                {
                    { "id", __.Id() },
                    { "slug", null },
                    { "writer", __Ext.Values("author") },
                    { "publishedOn", null },
                    { "updateDate", __Ext.Values("lastUpdate") },
                    { "title", null },
                    { "body", null },
                    {
                        "comments",
                        __.Out("hasComment")
                        .ProjectBy(new Dictionary<string, ITraversal>
                        {
                            { "id", __.Id() },
                            { "userName", null },
                            { "postedOn", null },
                            { "subject", __Ext.Values("title") },
                            { "body", null },
                            { "wasEdited", null }
                        }).Fold()
                    }
                }).Next();
        }
    }
}