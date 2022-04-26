using MyWebForum.Data.Infrastructure;
using System;

namespace MyWebForum.Data.Infrastructure
{
    public interface IExecutionContext : ISingletonService
    {
        DateTime GetDate();

        string GetLoginName();
    }
}