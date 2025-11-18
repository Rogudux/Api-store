namespace FrameworkYConexionBD;

public static class Prompts
{
    public static string GenerateOrdersPrompt(string jsonData)
    {
        return $@"
        Eres un analista de datos experto en retail.
        Analiza los sigueinte datos de ordenes, productos y tiendas (en JSON) {jsonData}

        Debes responder exlusivamnte en formato JSON de la sigueinte manera:
        {{
            ""topProdcuts"":{{""name"":string, ""unitsSold"": int, ""totalRevenue"": double}},
            ""topStore"": {{""name"":string,""totalSales"":double,""shareOfTotalSales"":double
            ""avgSpending"": double,
            ""patterns"": [string],
        }}
        En el apartado de patterns agrega apartados como: cual es el producto que mas deja ganancia por orden, cual es la tienda que mas genere.
        Si por alguna razonn no puede generar esta respuesta vlaida, por ejeplo: te hacen falta datos o tienes algun error en el formato responde SOLO con el texto: error.
        No me saludes, no me des explicaciones, no me des comentarios y no incluyas texto adicional.
";
        
        
    }
    
    public static string GenerateInvoicePrompt(string jsonData)
    {
        return $@"
        Eres un analista de datos experto en invoices.
        Analiza los sigueinte datos de invoices (en JSON) {jsonData}

        Debes responder exlusivamnte en formato JSON de la sigueinte manera:
       {{{{
        ""totalInvoices"": int,
        ""paidInvoices"": int,
        ""unpaidInvoices"": int,
        ""totalRevenue"": double,
        ""averageInvoiceAmount"": double,
        ""commonCurrencies"": [string],
        ""patterns"": [string]
      }}}}

        En el apartado de patterns agrega apartados como: 
        Qué porcentaje de facturas están pagadas?
        Qué moneda se usa más?
        Cualquier patrón relevante detectado?
        Si por alguna razonn no puede generar esta respuesta valida, por ejemplo: te hacen falta datos o tienes algun error en el formato responde SOLO con el texto: error.
        No me saludes, no me des explicaciones, no me des comentarios y no incluyas texto adicional.
";
        
        
    }
}