using Neo4j.Driver.V1;
using Neo4jClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Transactions;

namespace SandboxCreator
{
    class Neo4JClient
    {
        private IDriver _driver;
        private GraphClient client;

        public Neo4JClient(string driverUri, string connectionString, string userId, string password)
        {
            _driver = GraphDatabase.Driver(driverUri,
                AuthTokens.Basic(userId, password));
            client = new GraphClient(new Uri(connectionString), userId, password);
            client.Connect();
        }
        
        public async void CreateSandBox()
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var query = client.Cypher.Create("(user:User {guid: randomUUID()})")
                                    .With("user, range(30, 52) as sizeRange")
                                    .With("user, sizeRange, sizeRange[toInteger(floor(rand()*size(sizeRange)))] as mainSize")
                                    .With("user, range(mainSize,mainSize+2) as sizes")
                                    .Match("(shoe:Shoe)")
                                    .With("shoe, sizes, user, rand() as r ORDER BY r LIMIT 3")
                                    .Merge("(shoe)<-[l:LIKES]-(user)")
                                    .Set("l.size = sizes[toInteger(floor(rand()*size(sizes)))]");

                for (var i = 0; i < 300; i++)
                    query.ExecuteWithoutResults();
            }
        }
    }
}
