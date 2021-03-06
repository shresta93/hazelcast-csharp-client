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
using System.Collections.Generic;
using Hazelcast.Client.Protocol.Codec;
using Hazelcast.Client.Spi;
using Hazelcast.Core;
using Hazelcast.Util;

namespace Hazelcast.Client.Proxy
{
    internal class ClientSetProxy<T> : AbstractClientCollectionProxy<T>, IHSet<T>
    {
        public ClientSetProxy(string serviceName, string name, HazelcastClient client) : base(serviceName, name, client)
        {
        }

        public override Guid AddItemListener(IItemListener<T> listener, bool includeValue)
        {
            var request = SetAddListenerCodec.EncodeRequest(Name, includeValue, IsSmart());

            DistributedEventHandler handler = message => SetAddListenerCodec.EventHandler.HandleEvent(message,
                (item, uuid, type) =>
                {
                    HandleItemListener(item, uuid, (ItemEventType) type, listener, includeValue);
                });

            return RegisterListener(request, m => SetAddListenerCodec.DecodeResponse(m).Response,
                id => SetRemoveListenerCodec.EncodeRequest(Name, id), handler);
        }

        public override bool RemoveItemListener(Guid registrationId)
        {
            return DeregisterListener(registrationId);
        }

        public override bool Add(T item)
        {
            ValidationUtil.ThrowExceptionIfNull(item);
            var value = ToData(item);
            var request = SetAddCodec.EncodeRequest(Name, value);
            return Invoke(request, m => SetAddCodec.DecodeResponse(m).Response);
        }

        public override void Clear()
        {
            var request = SetClearCodec.EncodeRequest(Name);
            Invoke(request);
        }

        public override bool Contains(T item)
        {
            var request = SetContainsCodec.EncodeRequest(Name, ToData(item));
            return Invoke(request, m => SetContainsCodec.DecodeResponse(m).Response);
        }

        public override bool Remove(T item)
        {
            var request = SetRemoveCodec.EncodeRequest(Name, ToData(item));
            return Invoke(request, m => SetRemoveCodec.DecodeResponse(m).Response);
        }

        public override int Size()
        {
            var request = SetSizeCodec.EncodeRequest(Name);
            return Invoke(request, m => SetSizeCodec.DecodeResponse(m).Response);
        }

        public override bool IsEmpty()
        {
            var request = SetIsEmptyCodec.EncodeRequest(Name);
            return Invoke(request, m => SetIsEmptyCodec.DecodeResponse(m).Response);
        }

        public override bool ContainsAll<TE>(ICollection<TE> c)
        {
            var values = ToDataList(c);
            var request = SetContainsAllCodec.EncodeRequest(Name, values);
            return Invoke(request, m => SetContainsAllCodec.DecodeResponse(m).Response);
        }

        public override bool RemoveAll<TE>(ICollection<TE> c)
        {
            var values = ToDataList(c);
            var request = SetCompareAndRemoveAllCodec.EncodeRequest(Name, values);
            return Invoke(request, m => SetCompareAndRemoveAllCodec.DecodeResponse(m).Response);
        }

        public override bool RetainAll<TE>(ICollection<TE> c)
        {
            var values = ToDataList(c);
            var request = SetCompareAndRetainAllCodec.EncodeRequest(Name, values);
            return Invoke(request, m => SetCompareAndRetainAllCodec.DecodeResponse(m).Response);
        }

        public override bool AddAll<TE>(ICollection<TE> c)
        {
            var values = ToDataList(c);
            var request = SetAddAllCodec.EncodeRequest(Name, values);
            return Invoke(request, m => SetAddAllCodec.DecodeResponse(m).Response);
        }

        protected override ICollection<T> GetAll()
        {
            var request = SetGetAllCodec.EncodeRequest(Name);
            var result = Invoke(request, m => SetGetAllCodec.DecodeResponse(m).Response);
            return ToList<T>(result);
        }
    }
}