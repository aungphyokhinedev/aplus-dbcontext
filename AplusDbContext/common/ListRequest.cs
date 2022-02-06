namespace AplusDbContext;
public class ListRequest {

   public ListRequest( string tables, int page = 1, int pageSize = 10, string fields = "*" ){
       this.page = page.IsPositiveNumber() ? page : 1;
       this.pageSize = pageSize.IsPositiveNumber() ? pageSize : 10;
       this.fields = fields;
       this.tables = tables;
    
   }    

   public int page {get;}
   public  int pageSize{get;}
   public  string fields{get;set;}
   public WhereClause condition{get;set;}
   public  string? tables {get;}

   public  string? orderBy {get;set;}
   public  string? groupBy {get;set;}

}