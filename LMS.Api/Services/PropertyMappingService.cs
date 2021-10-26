using LMS.Api.Core.Dtos;
using LMS.Api.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Api.Services
{
    /// <summary>
    /// Mapping class Fields to strings. For OrderBy resource parameter in ResourceParameters. For us in mapping to via Linq Dynamic query. See Apply Sort extension.
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
        private Dictionary<string, PropertyMappingValue> _worksPropertyMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                {"Id", new PropertyMappingValue(new List<string>() { "Id"}) },
                {"GenreId", new PropertyMappingValue(new List<string>() { "GenreId"}) },
                {"TypeId", new PropertyMappingValue(new List<string>() { "TypeId"}) },
                {"Title", new PropertyMappingValue(new List<string>() { "Title"}) },
                {"Description", new PropertyMappingValue(new List<string>() { "Description"}) },
                {"Level", new PropertyMappingValue(new List<string>() { "Level"}) },
                {"PublicationDate", new PropertyMappingValue(new List<string>() { "PublicationDate"}) }
                //Todo Authors Genre Type
            };

        //private IList<PropertyMapping<TSource,TDestination> propertyMappings;
        //Circular? Need Marker Interface to Handle TSource, TDestination as they are not types.
        private IList<IPropertyMapping> _propertyMappings = new List<IPropertyMapping>();

        public PropertyMappingService()
        {
            _propertyMappings.Add(new PropertyMapping<AuthorDto, Author>(_authorPropertyMapping));
            _propertyMappings.Add(new PropertyMapping<WorkDto, Work>(_worksPropertyMapping));
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
