using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using SQL.RestFunction;
using Microsoft.Data.SqlClient;
using System.Linq;
using Microsoft.AspNetCore.WebUtilities;
namespace Company.Function
{
    public static class HttpTrigger
    {

        
        [Function("GetViews")]
        public static async Task<HttpResponseData> GetViews(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "schema/{schema}/views")] HttpRequestData req, FunctionContext executionContext, string schemaName)
        {
            
            SqlConnection sqlConnection = AuthMiddleware.SetupDBConnectionFromUserToken(req, schemaName, false);
            RestAPI restApi = new RestAPI(sqlConnection);
            var list = await restApi.getViews();
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(list);
            return response;
            
        }

        [Function("GetView")]
        public static async Task<HttpResponseData> GetView(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "schema/{schemaName}/view/{view}")] HttpRequestData req, FunctionContext executionContext, string view, string schemaName)
        {

            bool isServicePrincipalUser = false;
            
            try {
            var isSpUser = req.Headers.First(header => header.Key == "iuser");
            var val = isSpUser.Value;
            isServicePrincipalUser = bool.Parse(val.First());
            }

            catch {
                var res = req.CreateResponse(HttpStatusCode.BadRequest);
                await res.WriteStringAsync("iuser request header is missing, or is not a boolean value");
                return res;
            }

            SqlConnection sqlConnection = AuthMiddleware.SetupDBConnectionFromUserToken(req, schemaName, isServicePrincipalUser);
            RestAPI restApi = new RestAPI(sqlConnection);
  
            var queryDictionary = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(req.Url.Query);
            var list = await restApi.getView(queryDictionary, view);
            
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(list);
            return response;
        }

       
    }
}
