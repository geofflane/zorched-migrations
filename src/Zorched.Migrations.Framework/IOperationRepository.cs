using System;
using Zorched.Migrations.Framework.Data;
using Zorched.Migrations.Framework.Inspection;

namespace Zorched.Migrations.Framework
{
    public interface IOperationRepository
    {
        void Register<T>(Type impl) where T : IOperation;
        void RegisterReader<T>(Type impl) where T : IReaderOperation;
        void RegisterInspecor<T>(Type impl) where T : IInspectionOperation;

        T InstanceForInteface<T>();
    }
}