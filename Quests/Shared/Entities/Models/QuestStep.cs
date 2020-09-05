using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Quests.Shared.Entities.Models
{
    public class QuestStep
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [DisplayName("Название")]
        public string  Name { get; set; }
        [Required]
        [DisplayName("Описание")]
        public string Description { get; set; }
        [Required]
        [DisplayName("Изображение")]
        public string Img { get; set; }
        [Required]
        [DisplayName("Дистанция маршрута")]
        public int Distance { get; set; }
        [Required]
        [DisplayName("Очки за прохождение")]
        public int Points { get; set; }
        [Required]
        [DisplayName("Время прохождения")]
        public TimeSpan TravelTime { get; set; }
        [Required]
        [DisplayName("Подсказка")]
        public string Hint { get; set; }
        [Required]
        [DisplayName("Ответ")]
        public string Answer { get; set; }
        
        [DisplayName("Код видео (Iframe)")]
        public string VideoCode { get; set; }
        [DisplayName("Код карты (Iframe)")]
        public string MapCode { get; set; }
        [Required]
        [DisplayName("Вопрос")]
        public string Question { get; set; }
        [Required]
        [DisplayName("# Шага")]
        public int StepNumber { get; set; }
        public int QuestId { get; set; }
        public virtual Quest Quest { get; set; }
    }
}
