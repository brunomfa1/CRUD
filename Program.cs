using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World3");
app.MapGet("/user",() => "Bruno Marques");

//ADD parametros no Header
app.MapGet("/AddHeader",(HttpResponse response) => {
    response.Headers.Add("Teste","Bruno Marques");
    return new {Name = "Bruno", Age = 35};
});

/*METODO POST PARA INSERÇÃO PRODUTO ATRAVES DO CORPO(BODY)*/
app.MapPost("/saveproduto", (Produto produto)=> {
    ProductRepository.Add(produto);
});

/*METODO COM PARAMETROS PELA URL*/
/*EXEMPLO: api.app.com/product?datastart = {date}&dateend = {date}*/
app.MapGet("/getproduto", ([FromQuery]string dateStart, [FromQuery]string DateEnd)=>{
    return dateStart + " - "  + DateEnd;
});

/*OUTRO EXEMPLO POR URL E ATRAVEZ DE ROTA*/
/*api.app.com/user/{code}*/
app.MapGet("/getproduto/{code}", ([FromRoute]int code)=>{
    var product = ProductRepository.Getby(code);
    return product;
});

/*EXEMPLO ENVIANDO DADOS PELOS HEADER*/
app.MapGet("/getprodutopeloheader",(HttpRequest request)=>{
    return request.Headers["product-code"].ToString();
});

/*ATUALIZAR O DADOS - MÉTODO PUT*/
app.MapPut("/editproduto", (Produto produto) => {
    var x = ProductRepository.Getby(produto.Code);
    x.Nome =  produto.Nome; 
});

app.MapDelete("/deletaproduto/{code}",([FromRoute]int code) => {
    var produtoSaved = ProductRepository.Getby(code);
    ProductRepository.Remove(produtoSaved);
});



app.Run();


public static class ProductRepository
{
    public static List<Produto> products {get;set;}

    public static void Add(Produto product){
        if (products == null)
        {
            products = new List<Produto>();
        }
        products.Add(product);
    }
    public static Produto Getby(int code){
        return products.First(p => p.Code == code);
    }

    public static void Remove(Produto product){
        products.Remove(product);
    }
}

public class Produto{
    public int Code { get; set; }
    public string  Nome { get; set; }
}

