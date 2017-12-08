using DbManager.Core.DbProvider.Datacontext.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbManager.Models;

namespace DbManager.Core.Services.Serialization
{
    public class SerializationService : ISerializationService
    {
        public Task<List<Facility>> DeserializeJson(string json)
        {
            throw new NotImplementedException();
        }

        public Task<string> SerializationObject(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
