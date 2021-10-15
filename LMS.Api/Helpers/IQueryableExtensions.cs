using LMS.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

using System.Threading.Tasks;

namespace LMS.Api.Helpers
{
    public static class IQueryableExtensions
    {
        /// <summary>
        /// Function that cleans and sorts a IQueryable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">IQueryable</param>
        /// <param name="orderBy">User querystring authors/?orderBy=name desc, age desc => "name desc", "age desc"</param>
        /// <example></example>
        /// <param name="mappingDictionary"></param>
        /// <returns></returns>
        public static IQueryable<T> ApplySort<T>(this IQueryable<T> source,
            string orderBy, Dictionary<string, PropertyMappingValue> mappingDictionary)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (mappingDictionary is null)
            {
                throw new ArgumentNullException(nameof(mappingDictionary));
            }

            if (orderBy is null)
            {
                return source;
            }
            var orderByString = string.Empty;

            var orderByAfterSplit = orderBy.Split(','); 

            foreach (var orderByClause in orderByAfterSplit)
            {
                var trimmedOrderByClause = orderByClause.Trim();

                var orderDescending = trimmedOrderByClause.EndsWith(" desc"); //Bool

                var indexOfFirstSpace = trimmedOrderByClause.IndexOf(" "); // ex "DateOfBirth Desc"
                var propertyName = indexOfFirstSpace == -1 ?
                    trimmedOrderByClause : trimmedOrderByClause.Remove(indexOfFirstSpace); //If 1 params, returns first part.

                if (!mappingDictionary.ContainsKey(propertyName))
                {
                    throw new ArgumentException($"Mapping for {propertyName} is missing");
                }
                var propertyMappingValue = mappingDictionary[propertyName];

                if(propertyMappingValue == null)
                {
                    throw new ArgumentNullException("propertyMappingValue");
                }

                // Actual sorting using Dynamic Linq
                foreach (var destinationProperty in propertyMappingValue.DestinationProperties)
                {
                    if(propertyMappingValue.Revert)
                    {
                        orderDescending = !orderDescending;
                    }

                    orderByString = orderByString +
                        (string.IsNullOrWhiteSpace(orderByString) ? string.Empty : ", ")
                        + destinationProperty
                        + (orderDescending ? " descending" : " ascending");
                }
            }

            return source.OrderBy(orderByString);


        }
    }
}
