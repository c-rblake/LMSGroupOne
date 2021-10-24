using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace LMSGroupOne.Validation
{
    public class AllowedDocumentExtensions : ValidationAttribute
    {
        private readonly string[] _extensions;

        public AllowedDocumentExtensions(string[] extensions)
        {
            _extensions = extensions;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var postedDocuments = value as List<IFormFile>;

            foreach (var postedDocument in postedDocuments)
            {
                var extension = Path.GetExtension(postedDocument.FileName);
                if (postedDocument != null)
                {
                    if (!_extensions.Contains(extension.ToLower()))
                    {
                        return new ValidationResult(GetErrorMessage(postedDocument.FileName, extension));
                    }
                }
            }
            return ValidationResult.Success;
        }

        public string GetErrorMessage(string documentName, string extension)
        {
            return $"Document {documentName}'s file type ({extension}) is not allowed";
        }
    }
}