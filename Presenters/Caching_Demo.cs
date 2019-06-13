using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using StackExchange.Redis;
using GraphQL.Client;
using GraphQL.Common.Request;
using Newtonsoft.Json;

namespace Redis_Starter
{
    class Caching_Demo
    {
        static void Main(string[] args)
        {
            // Redis Connection
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("K1D-REDIS-CLST.ksg.int,password=ZECjTH9cx24ukQA");
            IDatabase redisDB = redis.GetDatabase();

            // GraphQL Connection
            GraphQLClient graphQL = new GraphQLClient("https://globaldeviceservice.dev.koreone/api/v1");

            // Stopwatch
            MicroLibrary.MicroStopwatch stopwatch = new MicroLibrary.MicroStopwatch();

            // Time the query 10 times
            string query = "query {" +
                "   deviceById(deviceId: 1) {" +
                "       description" +
                "   }" +
                "}" +
                "# Your Name Here";

            redisDB.KeyDelete(query);
            for (int i = 0; i < 20; i++)
            {
                stopwatch.Restart();
                do_query_with_caching(query, redisDB, graphQL);
                stopwatch.Stop();
                Console.WriteLine("Execution #" + i.ToString() + ": " + stopwatch.ElapsedMicroseconds + "µs");
            }
        }

        static dynamic do_query_with_caching(string query, IDatabase redisDB, GraphQLClient graphQL)
        {

            // Try to get the query result from redis
            string jsonString = redisDB.StringGet(query);

            // Check if a result was returned (the query IS in the cache)
            if (String.IsNullOrEmpty(jsonString))  // If the query IS NOT in the cache
            {

                // Issue graphQL query
                var graphQLResponse = graphQL.PostQueryAsync(query).GetAwaiter().GetResult();

                // Convert graphQL response to JSON string
                jsonString = graphQLResponse.Data.ToString();

                // Conditionally store result in Redis cache
                if (jsonString.Length * sizeof(char) < 1024 * 1024 * 10)
                {

                    // Add to cache
                    redisDB.StringSetAsync(query, jsonString);
                }

                // Return JObject
                return graphQLResponse.Data;

            } else      // The query IS in the cache
            {

                // Parse JSON string into JObject and return
                return JsonConvert.DeserializeObject<dynamic>(jsonString);
            }
        }
    }
}
