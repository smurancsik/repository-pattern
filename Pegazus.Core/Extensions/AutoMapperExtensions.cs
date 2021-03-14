using System.Collections.Generic;
using AutoMapper;

namespace Pegazus.Core.Extensions
{
    public static class AutoMapperExtensions
    {
        /// <summary>
        /// An automapper extension to create shorthand for mapping collections to each other.
        /// </summary>
        /// <typeparam name="TSource">The source collection type.</typeparam>
        /// <typeparam name="TDestination">The destination collection type.</typeparam>
        /// <param name="mapper">The automapper reference.</param>
        /// <param name="source">The source collection to be mapped.</param>
        /// <returns></returns>
        public static IList<TDestination> MapCollection<TSource, TDestination>(this IMapper mapper,
            IList<TSource> source)
        {
            return mapper.Map<IList<TSource>, IList<TDestination>>(source);
        }
    }
}
