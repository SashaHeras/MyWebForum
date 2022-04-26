using System.Reflection;
using Autofac.Extensions.DependencyInjection;

namespace MyWebForum.Helpers
{
    public static class AssemblyHelper
    {
        public static Assembly[] GetSolutionAssemblies()
        {
            return new[]
            {
                typeof(ServiceCollectionExtensions).Assembly,
                typeof(Data.Infrastructure.IRegisterDependency).Assembly,
                typeof(Data.Infrastructure.IRegisterDependency).Assembly
            };
        }
    }
}