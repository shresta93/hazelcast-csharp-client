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
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Hazelcast.IO.Serialization;

namespace Hazelcast.Util
{
    internal class ReadOnlyLazyDictionary<TKey, TValue, D> : AbstractLazyDictionary<TKey, TValue, D>, IDictionary<TKey, TValue>
        where D : class
    {
        public ReadOnlyLazyDictionary(IList<KeyValuePair<IData, D>> content, ISerializationService serializationService) :
            base(content, serializationService)
        {
        }

        public ICollection<TKey> Keys
        {
            get
            {
                var keyDatas = _content.Select(pair => pair.Key).ToList();
                return new ReadOnlyLazyList<TKey, IData>(keyDatas, _serializationService);
            }
            private set => throw new NotSupportedException("Readonly dictionary");
        }

        public ICollection<TValue> Values
        {
            get
            {
                var valueDatas = _content.Select(pair => pair.Value).ToList();
                return new ReadOnlyLazyList<TValue, D>(valueDatas, _serializationService);
            }
            private set => throw new NotSupportedException("Readonly dictionary");
        }
    }
}