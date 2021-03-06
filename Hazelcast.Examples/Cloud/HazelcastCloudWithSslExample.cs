// Copyright (c) 2008-2019, Hazelcast, Inc. All Rights Reserved.
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using Hazelcast.Client;
using Hazelcast.Config;

namespace Hazelcast.Examples.Cloud
{
    public class HazelcastCloudWithSslExample
    {
        private static void Run(string[] args)
        {
            Environment.SetEnvironmentVariable("hazelcast.logging.level", "info");
            Environment.SetEnvironmentVariable("hazelcast.logging.type", "console");

            var clientConfig = new ClientConfig();
            var clientNetworkConfig = clientConfig.GetNetworkConfig();

            clientNetworkConfig.GetCloudConfig()
                .SetEnabled(true)
                .SetDiscoveryToken("DISCOVERY_TOKEN_HASH");//Discovery token copied from Cloud console

            //Server certificate will be validated by OS,
            //Download and register the server trust store to your OS 
            clientNetworkConfig.GetSSLConfig().SetEnabled(true);
            //in order to disable certificate validating uncomment below line
            //clientNetworkConfig.GetSSLConfig().SetProperty(SSLConfig.ValidateCertificateChain, "false");

            //Client certificate file path downloaded from cloud console
            clientNetworkConfig.GetSSLConfig().SetProperty(SSLConfig.CertificateFilePath, "CLIENT_PFX_CERTIFICATE_PATH");

            var client = HazelcastClient.NewHazelcastClient(clientConfig);

            var map = client.GetMap<string, string>("ssl-example");

            map.Put("key", "value");

            Console.WriteLine("Key: " + map.Get("key"));

            map.Destroy();
            client.Shutdown();
        }
    }
}