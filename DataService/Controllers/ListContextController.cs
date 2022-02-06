using Microsoft.AspNetCore.Mvc;
using AplusDbContext;
using MassTransit;
namespace DataService.Controllers;

[ApiController]
[Route("[controller]")]
public class ListContextController : ControllerBase
{
    IRequestClient<GetList> _client;

    private readonly ILogger<ListContextController> _logger;
    private IDataContext _db;
    public ListContextController(ILogger<ListContextController> logger, IDataContext db,IRequestClient<GetList> client)
    {
        _client = client;
        _logger = logger;
        _db = db;
    }

    //http://localhost:5033/ListContext?PageSize=15&page=1
        public async Task<object> GetList(int page, int pageSize)
        {
            var request = new ListRequest(tables:"users", pageSize: pageSize, page: page){
                fields = "id,nrc,mobile_no,createdat",
                orderBy = "id desc",
               /* condition = new WhereClause{
                    where = "id = 4",
                    parameters = new Dictionary<string, object>{
                        {"id" , 4 }
                    }
                }*/
            };
           
           var result = await _client.GetResponse<ListData>(new {request=request});

           // var result = await _db.GetListAsync(request);
           
            return  result.Message.response;
        }


}
