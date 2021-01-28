using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace Quests.Shared.Entities.Models
{
    public class Instruction
    {
        [Required]
        public int Id { get; set; }
        [Required (ErrorMessage = "Поле Заголовок обязательное")]
        [DisplayName("Заголовок")]
        public string Title { get; set; }
        [Required (ErrorMessage = "Поле Описание обязательное")]
        [DisplayName("Описание")]
        public string Description { get; set; }
        [DisplayName("Изображение")]
        
        public string Img { get; set; }
    }
}
