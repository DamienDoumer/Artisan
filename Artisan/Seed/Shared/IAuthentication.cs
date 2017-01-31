using System;
using Seed.Delegates;
using System.IO;

namespace Seed.Shared
{
    public interface IAuthentication
    {
        void Authenticate();
        AuthenticationObject ReceiveAuthenticationObject(MemoryStream stream, BinaryReader reader);
    }
}
