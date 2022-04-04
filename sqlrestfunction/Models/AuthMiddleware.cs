using Microsoft.Azure.Functions.Worker.Http;
using System.Linq;
using System.IO;
using Microsoft.Data.SqlClient;

namespace SQL.RestFunction
{
public static class AuthMiddleware
{
    
    public static SqlConnection SetupDBConnectionFromUserToken(HttpRequestData req, string schemaName, bool isServicePrincipal) {

         //strip BEARER from Authorization header
        var authZhdr = req.Headers.FirstOrDefault(h => h.Key.Equals("Authorization"));
        var token = authZhdr.Value.FirstOrDefault().Substring(7);

        ADDatabaseConnector dc = new ADDatabaseConnector(token, isServicePrincipal);    
        
        string requestBody = "";
        using (StreamReader streamReader =  new  StreamReader(req.Body))
        {
            requestBody = streamReader.ReadToEnd();
        }

        var databaseConnection = dc.getOpenDatabaseConnectionUsingAccessToken(schemaName);
    
        return databaseConnection;
        
    }

    public static SqlConnection SetupDBConnectionFromConfig() {

            return DatabaseConnector.getOpenDatabaseConnectionFromConfig();
            
        }
    }

}