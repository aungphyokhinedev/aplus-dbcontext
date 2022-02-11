using MassTransit;
namespace DataService;

public class RemoveDataConsumer : IConsumer<RemoveData> 
{
    private IDataContext _db;
    public RemoveDataConsumer( IDataContext db)
    {
        _db = db;
    }

    public async Task Consume(ConsumeContext<RemoveData> context)
    {   
        
        var result = await _db.RemoveAsync(context.Message.request);
       
        await context.RespondAsync<ResultData>(new
        {
            response = result
        });
    }
}

