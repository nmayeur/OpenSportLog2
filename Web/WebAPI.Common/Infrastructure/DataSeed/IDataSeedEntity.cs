using System;
using System.Collections.Generic;
using System.Text;
using WebAPI.Common.Model;

namespace WebAPI.Common.Infrastructure.DataSeed
{
    public interface IDataSeedEntity<T>
    {
        void CreateTable();
        Task<bool> CheckTableAsync();
        IEnumerable<T> GetFromFile();
        void InsertData(T Data);

    }
}
