﻿// Copyright (c) 2008-2019, Hazelcast, Inc. All Rights Reserved.
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
using Hazelcast.Core;
using Hazelcast.IO.Serialization;
using NUnit.Framework;
using Address = Hazelcast.IO.Address;

namespace Hazelcast.Client.Test
{
    [TestFixture]
    public class EntryEventTest
    {
        private ISerializationService serializationService;
        private DataAwareEntryEvent<string, int?> dataAwareEntryEvent;
        private IData dataString;
        private IData dataInt;
        private string testString;
        private int testInt;

        [SetUp]
        public void Init()
        {
            serializationService = new SerializationServiceBuilder().Build();
            testString = "Test String";
            dataString = serializationService.ToData(testString);
            testInt = 666;
            dataInt = serializationService.ToData(testInt);

            IMember member = new MemberInfo(new Address("localhost", 5701), Guid.Empty,
                null, false,new MemberVersion(4,0,0));

            dataAwareEntryEvent = new DataAwareEntryEvent<string, int?>("source", member, EntryEventType.Added,
                dataString, dataInt, dataInt, null, serializationService);
        }

        [TearDown]
        public void Destroy()
        {
            serializationService.Destroy();
        }


        [Test]
        public virtual void TestGetLazy()
        {
            Assert.AreEqual(dataAwareEntryEvent.GetKey(), testString);
            Assert.AreEqual(dataAwareEntryEvent.GetValue(), testInt);
            Assert.AreEqual(dataAwareEntryEvent.GetOldValue(), testInt);
            Assert.Null(dataAwareEntryEvent.GetMergingValue());
        }
    }
}