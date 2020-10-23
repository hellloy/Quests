using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Quests.Shared.Entities.Models
{
    public class NewsRecord
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
        [Required (ErrorMessage = "Поле Изображение обязательное")]
        public string Img { get; set; } = "https://via.placeholder.com/640x480.png?text=%D0%97%D0%B0%D0%B3%D1%80%D1%83%D0%B7%D0%B8%D1%82%D0%B5%20%D0%B8%D0%B7%D0%BE%D0%B1%D1%80%D0%B0%D0%B6%D0%B5%D0%BD%D0%B8%D0%B5";
    }
}
