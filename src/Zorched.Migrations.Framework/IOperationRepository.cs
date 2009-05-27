using System;
using Zorched.Migrations.Framework.Data;
using Zorched.Migrations.Framework.Inspection;

namespace Zorched.Migrations.Framework
{
    /// <summary>
    /// Maintains a mapping of Interfaces to their implementation classes.
    /// </summary>
    /// <remarks>
    /// This will allow users to override the default implementations as well
    /// as create their own interfaces and implementations.
    /// </remarks>
    public interface IOperationRepository
    {
        void Register<T>(Type impl) where T : IOperation;
        void RegisterReader<T>(Type impl) where T : IReaderOperation;
        void RegisterInspecor<T>(Type impl) where T : IInspectionOperation;

        T InstanceForInteface<T>();
    }
}