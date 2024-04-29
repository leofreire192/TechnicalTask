using System;

namespace CustomersAPI.Services
{
    public interface IMapper<TSource, TDestination>
        where TSource : class
        where TDestination : class
    {
        TDestination Map(TSource source);
    }
}