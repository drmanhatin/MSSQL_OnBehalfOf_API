

using Microsoft.Identity.Client;
using Microsoft.Data.SqlClient;
using System;

namespace SQL.RestFunction {
public static class DatabaseConnector {


public static SqlConnection getOpenDatabaseConnectionFromConfig() {   
    var serverName = Environment.GetEnvironmentVariable("SQL_DATABASE_SERVER");
    var databaseName = Environment.GetEnvironmentVariable("SQL_DATABASE_NAME");
    var userName = Environment.GetEnvironmentVariable("SQL_USERNAME"); 
    var userPassword = Environment.GetEnvironmentVariable("SQL_PASSWORD"); 
    var sqlConnectionString = $"Server={serverName}; Database={databaseName}; User ID={userName}; Password={userPassword}";
    var connection = new SqlConnection(sqlConnectionString);
    connection.Open();
    return connection;
}


}

}