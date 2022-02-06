using AplusDbContext;


public interface IDataContext  {

    Task<ListResponse>  GetListAsync(ListRequest request);

    Task<DataResponse> AddAsync(CreateRequest request);
}