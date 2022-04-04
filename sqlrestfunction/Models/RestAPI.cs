using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Data;
using SqlKata.Execution;
using System.Linq;
using System.Collections;

namespace SQL.RestFunction
{
public class RestAPI
{

    private SqlConnection _sqlConnection;
   
    public RestAPI(SqlConnection sqlConnection)
    {
        _sqlConnection = sqlConnection;
    }


    public async Task<List<string>> getViews(string StoredProcedureName = "rpt.ListAllViews", string schemaName = "api-test")
    {

        var command = new SqlCommand(StoredProcedureName, _sqlConnection)
        {
            CommandType = CommandType.StoredProcedure
        };

        command.Parameters.Add(new SqlParameter("@SchemaName", schemaName));

        var views = new List<string>();

        using (var reader = await command.ExecuteReaderAsync())
        {
            while (await reader.ReadAsync())
            {
                views.Add((string)reader["name"]);
            }
        }

        return views;
    }

    public async Task<List<object>> getTables() {

        var command = new SqlCommand("select * from sys.tables", _sqlConnection)
        {
            CommandType = CommandType.Text
        };

        var rows = new List<object>();
        
        using (var reader = await command.ExecuteReaderAsync())
        {
            while (await reader.ReadAsync())
            {
                    object[] tempRow = new object[reader.FieldCount];
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        tempRow[i] = reader[i];
                    }
                    rows.Add(tempRow);
                
            }
        }
        
        return rows;

    }
    
    public async Task<List<object>> getView(IDictionary queryDictionary, string view) {

        var result = await SQLQueryBuilder.buildQueryFromRequest(_sqlConnection, queryDictionary, view);
       
        return result;
    }

}

}