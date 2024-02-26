using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels; 

public class CategoryViewModel {
    [Required(ErrorMessage = "Nome obrigatório")]
    [MinLength(5, ErrorMessage = "Nome deve ter no mínimo 5 caracteres")]
    public string Name { get; set; } = string.Empty;
    [Required(ErrorMessage = "Slug obrigatório")]
    [MinLength(5, ErrorMessage = "Slug deve ter no mínimo 5 caracteres")]
    public string Slug { get; set; } = string.Empty;
}
