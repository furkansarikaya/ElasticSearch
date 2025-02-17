using System.ComponentModel.DataAnnotations;

namespace ElasticSearch.WEB.ViewModels.ECommerce;

public class ECommerceSearchViewModel
{
    [Display(Name = "Kategori")]
    public string? Category { get; set; }
    
    [Display(Name = "Cinsiyet")]
    public string? Gender { get; set; }
    
    [Display(Name = "Siparis tarihi(baslangic)")]
    [DataType(DataType.Date)]
    public DateTime? OrderDateStart { get; set; }
    
    [Display(Name = "Siparis tarihi(bitis)")]
    [DataType(DataType.Date)]
    public DateTime? OrderDateEnd { get; set; }
    
    [Display(Name = "Ad soyad")]
    public string? CustomerFullName { get; set; }
}