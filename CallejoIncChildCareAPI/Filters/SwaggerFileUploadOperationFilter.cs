using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace CallejoIncChildcareAPI.Filters
{
    public class SwaggerFileUploadOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Check if the action has an IFormFile parameter
            var hasFormFileParameter = context.ApiDescription.ParameterDescriptions
                .Any(p => p.Type == typeof(IFormFile) || p.Type == typeof(IEnumerable<IFormFile>));

            if (!hasFormFileParameter)
            {
                return;
            }

            // Define the RequestBody with file and additional parameters
            operation.RequestBody = new OpenApiRequestBody
            {
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    ["multipart/form-data"] = new OpenApiMediaType
                    {
                        Schema = new OpenApiSchema
                        {
                            Type = "object",
                            Properties = new Dictionary<string, OpenApiSchema>
                            {
                                ["file"] = new OpenApiSchema
                                {
                                    Type = "string",
                                    Format = "binary",
                                    Description = "The file to upload (PDF, JPG, or DOC)."
                                },
                                ["documentType"] = new OpenApiSchema
                                {
                                    Type = "string",
                                    Description = "The type of the document (e.g., IdentificationInfo)."
                                }
                            },
                            Required = new HashSet<string> { "file", "documentType" } // Make fields required
                        }
                    }
                }
            };
        }
    }
}
