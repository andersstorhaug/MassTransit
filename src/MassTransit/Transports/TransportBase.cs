// Copyright 2007-2008 The Apache Software Foundation.
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use 
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed 
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.
namespace MassTransit.Transports
{
    using System;
    using System.Diagnostics;
    using System.IO;

    [DebuggerDisplay("{Address}")]
    public abstract class TransportBase :
        ITransport
    {
        protected bool _disposed;

        protected TransportBase(IEndpointAddress address)
        {
            Address = address;
        }

        public IEndpointAddress Address { get; private set; }

        public abstract void Send(Action<Stream> sender);

        public void Receive(Func<Stream, Action<Stream>> receiver)
        {
            EnsureNotDisposed();

            Receive(receiver, TimeSpan.Zero);
        }

        public abstract void Receive(Func<Stream, Action<Stream>> receiver, TimeSpan timeout);

        protected void EnsureNotDisposed()
        {
            if(_disposed)
                throw new ObjectDisposedException("The transport has already been disposed: " + Address);
        }

        public abstract void OnDisposing();

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing)
            {
                OnDisposing();
            }

            _disposed = true;
        }

        ~TransportBase()
        {
            Dispose(false);
        }
    }
}