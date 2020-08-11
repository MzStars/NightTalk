using System;
using System.Collections.Generic;
using System.IO;

namespace Infrastructure.Common
{
     public interface IStorage
    {
        Uri Save(string container, string fileName, Stream stream, bool lowercaseUri = true);

        void Remove(string container, string fileName, bool lowercaseUri = true);
    }
}
