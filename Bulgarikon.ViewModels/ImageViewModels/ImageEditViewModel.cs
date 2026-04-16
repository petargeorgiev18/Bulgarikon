using System.ComponentModel.DataAnnotations;

namespace Bulgarikon.ViewModels.ImageViewModels
{
    public class ImageEditViewModel
    {
        public Guid? Id { get; set; }
        [Url(ErrorMessage = "Невалиден URL адрес.")]
        public string? Url { get; set; }
        public string? Caption { get; set; }
        public bool Remove { get; set; }
    }
}
