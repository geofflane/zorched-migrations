using System;
using Zorched.Migrations.Framework.Data;

namespace Zorched.Migrations.Framework
{
    public interface IOperationRepository
    {
        void Register<T>(Type impl) where T : IOperation;
        void RegisterReader<T>(Type impl) where T : IReaderOperation;
    }
}