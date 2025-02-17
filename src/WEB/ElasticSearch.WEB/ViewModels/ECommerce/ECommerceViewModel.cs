namespace ElasticSearch.WEB.ViewModels.ECommerce;

public class ECommerceViewModel
{
    public string Id { get; set; } = null!;
    public string CustomerFirstName { get; set; } = null!;
    public string CustomerLastName { get; set; } = null!;
    public string CustomerFullName { get; set; } = null!;
    public string CustomerGender { get; set; } = null!;
    public double TaxFulTotalPrice { get; set; }
    public string Category { get; set; } = null!;
    public int OrderId { get; set; }
    public string OrderDate { get; set; } = null!;
}

public class EProductViewModel
{
    public long ProductId { get; set; }
    public string ProductName { get; set; } = null!;
}