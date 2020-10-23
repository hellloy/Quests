using System;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Quests.Shared.Entities.Models
{
    public class QuestStep
    {
        [Required]
        public int Id { get; set; }
        [Required (ErrorMessage = "Поле Название обязательное")]
        [DisplayName("Название")]
        public string  Name { get; set; }
        [Required (ErrorMessage = "Поле Описание обязательное")]
        [DisplayName("Описание")]
        public string Description { get; set; }
        [Required]
        [DisplayName("Изображение")]
        public string Img { get; set; } = "https://via.placeholder.com/640x480.png?text=%D0%97%D0%B0%D0%B3%D1%80%D1%83%D0%B7%D0%B8%D1%82%D0%B5%20%D0%B8%D0%B7%D0%BE%D0%B1%D1%80%D0%B0%D0%B6%D0%B5%D0%BD%D0%B8%D0%B5";
        [Required (ErrorMessage = "Поле Дистанция маршрута обязательное")]
        [DisplayName("Дистанция маршрута")]
        public int Distance { get; set; }
        [Required (ErrorMessage = "Поле Очки за прохождение обязательное")]
        [DisplayName("Очки за прохождение")]
        public int Points { get; set; }
        [Required (ErrorMessage = "Поле Время прохождения обязательное")]
        [DisplayName("Время прохождения")]
        public DateTime TravelTime { get; set; }
        [Required (ErrorMessage = "Поле Подсказка обязательное")]
        [DisplayName("Подсказка")]
        public string Hint { get; set; }
        [Required (ErrorMessage = "Поле Ответ обязательное")]
        [DisplayName("Ответ")]
        public string Answer { get; set; }
        
        [DisplayName("Код видео (Iframe)")]
        public string VideoCode { get; set; }
        [DisplayName("Код карты (Iframe)")]
        public string MapCode { get; set; }
        [Required (ErrorMessage = "Поле Вопрос обязательное")]
        [DisplayName("Вопрос")]
        public string Question { get; set; }
        [Required (ErrorMessage = "Поле # Шага обязательное")]
        [DisplayName("# Шага")]
        public int StepNumber { get; set; }
        public int QuestId { get; set; }
        public virtual Quest Quest { get; set; }
        
    }
}
