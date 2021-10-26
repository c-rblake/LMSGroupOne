using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LMS.Core.Validation
{
    public class MaxDocumentSize : ValidationAttribute
    {
        private readonly int _maxDocumentSize;
        public MaxDocumentSize(int maxDocumentSize)
        {
            _maxDocumentSize = maxDocumentSize;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var postedDocuments = value as IList<IFormFile>;

            foreach (var postedDocument in postedDocuments)
            {
                if (postedDocument != null)
                {
                    if (postedDocument.Length > _maxDocumentSize)
                    {
                        return new ValidationResult(GetErrorMessage(postedDocument.FileName));
                    }
                }
            }
            return ValidationResult.Success;
        }

        public string GetErrorMessage(string documentName)
        {
            return $"{documentName}'s size is out of range as maximum allowed document size is { _maxDocumentSize} bytes";
        }
    }
}