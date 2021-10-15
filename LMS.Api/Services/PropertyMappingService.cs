using LMS.Api.Core.Dtos;
using LMS.Api.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Api.Services
{
    /// <summary>
    /// Objects to map Fields to String for OrderBy methods via Linq Dynamic
    /// </summary>
    public class PropertyMappingService : IPropertyMappingService

    {
        private Dictionary<string, PropertyMappingValue> _authorPropertyMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                //Using EF Core Linq Dynamic for string sorting. Careful that Field name is Actually spell correct as a string!
                {"Id",  new PropertyMappingValue(new List<string>() {"Id"}, false)},
                {"Name", new PropertyMappingValue(new List<string>() {"FirstName", "LastName"})},//bool is default false
                {"Age", new PropertyMappingValue(new List<string>() {"DateOfBirth"}, true)}
            };
        //Todo Works Dicitonary

        //private IList<PropertyMapping<TSource,TDestination> propertyMappings;
        //Circular? Need Marker Interface to Handle TSource, TDestination as they are not types.
        private IList<IPropertyMapping> _propertyMappings = new List<IPropertyMapping>();

        public PropertyMappingService()
        {
            _propertyMappings.Add(new PropertyMapping<AuthorDto, Author>(_authorPropertyMapping));
            //Todo Add the Works to the list of Possible prepertyMappings.
        }

        public Dictionary<string, PropertyMappingValue> GetPropertyMapping
            <TSource, TDestination>()
        {
            var matchingMapping = _propertyMappings
                .OfType<PropertyMapping<TSource, TDestination>>();

            if (matchingMapping.Count() == 1)
            {
                return matchingMapping.First()._mappingDictionary;
            }
            throw new Exception($"Could not find exact property mapping instance" +
                $"for <{typeof(TSource)}, {typeof(TDestination)}");

        }
    }
}
