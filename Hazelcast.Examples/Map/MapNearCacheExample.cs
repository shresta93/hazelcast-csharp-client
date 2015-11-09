﻿using System;
using System.Diagnostics;
using Hazelcast.Client;
using Hazelcast.Config;

namespace Hazelcast.Examples.Map
{
    class MapNearCacheExample
    {
        public static void Run(string[] args)
        {
            Environment.SetEnvironmentVariable("hazelcast.logging.level", "info");
            Environment.SetEnvironmentVariable("hazelcast.logging.type", "console");

            var config = new ClientConfig();

            var nearCacheConfig = new NearCacheConfig();
            nearCacheConfig.SetMaxSize(1000)
                .SetInvalidateOnChange(true)
                .SetEvictionPolicy("Lru")
                .SetInMemoryFormat(InMemoryFormat.Binary);

            config.AddNearCacheConfig("nearcache-map-*", nearCacheConfig);
            config.GetNetworkConfig().AddAddress("127.0.0.1");

            var client = HazelcastClient.NewHazelcastClient(config);

            var map = client.GetMap<string, string>("nearcache-map-1");

            for (int i = 0; i < 1000; i++)
            {
                map.Put("key" + i, "value" + i);
            }

            var sw = new Stopwatch();

            sw.Start();
            for (int i = 0; i < 1000; i++)
            {
                map.Get("key" + i);
            }
            Console.WriteLine("Got values in " + sw.ElapsedMilliseconds + " millis");

            sw.Restart();
            for (int i = 0; i < 1000; i++)
            {
                map.Get("key" + i);
            }
            Console.WriteLine("Got cached values in " + sw.ElapsedMilliseconds + " millis");

            map.Destroy();
            client.Shutdown();
        }
    }
}