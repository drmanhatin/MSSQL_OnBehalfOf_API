
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.Azure.Functions.Worker.Http;
using System.Threading.Tasks;
using SqlKata;
using SqlKata.Compilers;
using SqlKata.Execution;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using Newtonsoft.Json.Linq;

namespace SQL.RestFunction {
    public static class SQLQueryBuilder {

        public static async Task<List<object>> buildQueryFromRequest(SqlConnection sqlConnection, IDictionary queryDictionary, string databaseView) 
        {   
            var compiler = new SqlServerCompiler();

            var qf = new QueryFactory(sqlConnection, compiler);

            var queryBase = qf.Query(databaseView);

            foreach(string key in queryDictionary.Keys)
            {
                switch(key)
                {
                    case "limit":
                        queryBase.Limit(int.Parse(queryDictionary[key].ToString()));
                        break;
                    case "offset":
                        queryBase.Offset(int.Parse(queryDictionary[key].ToString()));
                        break;
                    case "where":
                        JObject json = JObject.Parse(queryDictionary[key].ToString());
                        queryBase.Where(json["column"].ToString(), json["operator"].ToString(), json["value"].ToString());
                        break;
                    case "orderBy":
                        JObject jsonOrderBy = JObject.Parse(queryDictionary[key].ToString());
                        queryBase.OrderBy(jsonOrderBy["column"].ToString(), jsonOrderBy["order"].ToString());
                        break;
                }
            }

            var q1 = await queryBase.GetAsync();

            var q2 = q1.ToList<object>();  

            
            return q2;
        }
    }
} 