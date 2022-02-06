using Microsoft.AspNetCore.Mvc;
using AplusDbContext;
using MassTransit;
namespace DataService.Controllers;

[ApiController]
[Route("[controller]")]
public class AddContextController : ControllerBase
{
    IRequestClient<AddData> _client;

    private readonly ILogger<AddContextController> _logger;
    private IDataContext _db;
    public AddContextController(ILogger<AddContextController> logger, IDataContext db,IRequestClient<AddData> client)
    {
        _client = client;
        _logger = logger;
        _db = db;
    }

        //http://localhost:5033/AddContext
        public async Task<DataResponse> Add()
        {
           
            var request2 = new users{
                   UID = 7,
                   NRC = "12/pzt(N)9400449",
                   PIN = "12120",
                   Mobile_no = "9956000",
                   Email= "old@gmail.com",
                  CreatedAt = DateTime.Now,
                   DeleteFlag = false
               };
            var data = new Dictionary<string, object>{
                        {"UID" , 4 },
                        {"NRC" , "12/pzt(N)9400" },
                        {"PIN" , "12120" },
                        {"Mobile_no" , "059505" },
                        {"Email" , "old@gmail.com" },
                        {"CreatedAt" , (DateTimeOffset)DateTime.Now },
                        {"EditedAt" , (DateTimeOffset)DateTime.Now },
                        {"DeleteFlag" , false },
                    };
            var parameters = toParameters(data);

            //direct test 
          /*   var result = await _db.AddAsync(new CreateRequest{
                 table ="users",
                 data = parameters
             });
             */
            var result = await _client.GetResponse<ResultData>(new {request = new CreateRequest {
                table = "users",
                data = parameters
            }});

           
           
            return   result.Message.response;
        
        }

        List<CustomParameter> toParameters(Dictionary<string,object> data){
        return data.Select(x=>new CustomParameter{
                key =  x.Key,
                value = x.Value,
                type = x.Value.GetType()
            }).ToList();
}

}
