
using System.IO;
using Microsoft.Identity.Client;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json.Linq;
using System;
namespace SQL.RestFunction {
public class ADDatabaseConnector {

private IConfidentialClientApplication authClient;
private string _databaseAccessToken;

public ADDatabaseConnector(string userAccessTokenWithoutBearer, bool isServicePrincipalUser) {
    this.authClient = getAuthClient();
    
    //two auth scenarios: 1 for service principal caller, 1 for user caller
    //if the user is a normal AD user, we need to get a database access token on his behalf
    if(!isServicePrincipalUser) {
        this._databaseAccessToken = getAccessTokenForDatabase(userAccessTokenWithoutBearer);
    }


    //if the caller of the db is a service principal, we do not need to get a db token on his behalf 
    //because a service principal does not support the on behalf of authentication flow
    //and we can use the service principal's access token directly
    if(isServicePrincipalUser){
        this._databaseAccessToken = userAccessTokenWithoutBearer;
    }
}

private IConfidentialClientApplication getAuthClient() {
    authClient = ConfidentialClientApplicationBuilder
                .Create(System.Environment.GetEnvironmentVariable("ClientId"))
                .WithClientSecret(System.Environment.GetEnvironmentVariable("ClientSecret"))
                .WithAuthority(System.Environment.GetEnvironmentVariable("Authority"))
                .WithTenantId(System.Environment.GetEnvironmentVariable("TenantId"))
                .Build();

    return authClient;
}

public SqlConnection getOpenDatabaseConnectionUsingAccessToken(string schemaName) {
     
        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

        builder.DataSource = System.Environment.GetEnvironmentVariable("DatabaseUrl");

        builder.InitialCatalog = schemaName;
        
        SqlConnection connection = new SqlConnection(builder.ConnectionString);
    
        connection.AccessToken = _databaseAccessToken;
        connection.Open();
        return connection;

}

private string getAccessTokenForDatabase(string userAccessTokenWithoutBearer) {
        UserAssertion ua = new UserAssertion(userAccessTokenWithoutBearer);
        var res = authClient.AcquireTokenOnBehalfOf(new string[] { "https://sql.azuresynapse-dogfood.net/user_impersonation" }, ua)
                .ExecuteAsync().Result;
        return res.AccessToken;
}

}

}