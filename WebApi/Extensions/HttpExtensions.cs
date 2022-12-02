using System.Text.Json;
using WebApi.Helpers;

namespace WebApi.Extensions
{
    public static class HttpExtensions
    {
        public static void AddPaginationHeader(this HttpResponse response, PaginationHeader pageHeader){
            
            //Set Naming Policy
            var jsonOptions = new JsonSerializerOptions{PropertyNamingPolicy = JsonNamingPolicy.CamelCase};

            //Serialize object
            response.Headers.Add("Pagination",JsonSerializer.Serialize(pageHeader,jsonOptions));

            response.Headers.Add("Access-control-Expose-Headers","Pagination");
        }
    }
}