
using Dapper;
using Npgsql;
using AplusDbContext;

public class PostgresDataContext : IDataContext
{
    IDbConnection _config;
    
    public PostgresDataContext(IDbConnection cofig){
        _config = cofig;
    }

    public async Task<ListResponse> GetListAsync(ListRequest request)
    {
        using (var connection = new NpgsqlConnection(_config.connection))
        {
            try
            {
                connection.Open();

                string query = $"Select {request.fields} from {request.tables}";
                object parameters = new object{};
                //adding where clause 
                //instead of using value directly
                //consider using parameter to prevent SQL injection
                //eg where name = @name and then add @name value in request parameters
               if (request.condition.IsNotNullOrEmpty())
                {
                    query = $"{query} where {request.condition.where}";
                    parameters = (Object)request.condition.parameters;
                }
     
                //adding group by clause
                if (request.groupBy.IsNotNullOrEmpty())
                {
                    query = $"{query} group by {request.groupBy}";
                }

                //adding order by clause
                if (request.orderBy.IsNotNullOrEmpty())
                {
                    query = $"{query}  order by {request.orderBy}";
                }


                query = $"{query} offset {((request.page - 1) * request.pageSize)} limit {request.pageSize};";



                var value = await connection.QueryAsync(query, parameters);

                ///get total for pagination
                long total = 0;
                query = $"select count(*) as total_rows from  {request.tables}";
               if (request.condition.IsNotNullOrEmpty())
                {
                    query = $"{query} where {request.condition.where};";
                }
                
                var countvalue = await connection.QueryAsync(query, parameters);
                total = countvalue.SingleOrDefault().total_rows;


                return new ListResponse
                {
                    code = ResultCode.Success,
                    total = total,
                    page = request.page,
                    pageSize = request.pageSize,
                    rows = value.Select(x => x as IDictionary<string,object>).ToList()
                };
            }
            catch (Exception e)
            {
                return new ListResponse
                {
                    code = ResultCode.Fail,
                    message = e.Message
                };
            }
        }

    }

    public async Task<DataResponse> AddAsync(CreateRequest request)
    {
        using (var connection = new NpgsqlConnection(_config.connection))
        {
            try
            {
                connection.Open();
                
                string columns = String.Join(",", request.data.Select(x=>x.key));
                string values = String.Join(",", request.data.Select(x=>$"@{x.key}"));
                Dictionary<string,object> parameters = request.data.ToDictionary(x=>x.key,x=>{
                    
                    ///this is for message queue serialization on date data problem
                    if(x.type == typeof(DateTimeOffset)){
                        return DateTimeOffset.Parse(x.value.ToString());
                    }
                    return x.value;
                } );

           
                string query = $"INSERT INTO {request.table} ({columns}) VALUES ({values}) RETURNING Id;";

                int Id = await connection.ExecuteScalarAsync<int>(query, parameters);
                var created = await connection.QueryAsync($"SELECT * FROM {request.table} WHERE id = {Id};");
                return new DataResponse
                {
                    code = ResultCode.Success,
                    message = "ok",
                    data = created.FirstOrDefault()
                };
            }
            catch (Exception e)
            {
                return new DataResponse
                {
                    code = ResultCode.Fail,
                    message = e.Message
                };
            }
        }
        
    }


}