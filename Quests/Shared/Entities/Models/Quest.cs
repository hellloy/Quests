using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Quests.Shared.Entities.Models
{
   public class Quest
    {
        [Required]
        public int Id {get;set;}
        [Required]
        [DisplayName("Название")]
        public string Name {get;set;}
        [Required]
        [DisplayName("Описание")]
        public string Description {get;set;}
        [Required]
        [DisplayName("Город")]
        public string City { get; set; }
        [Required]
        [DisplayName("Цена")]
        public int Price {get;set;} = 0;
        [Required]
        [DisplayName("Бесплатный")]
        public bool Free {get;set;} = false;

        [Required]
        [DisplayName("Изображение")]
        public string Img { get; set; } =
            "https://via.placeholder.com/640x480.png?text=%D0%97%D0%B0%D0%B3%D1%80%D1%83%D0%B7%D0%B8%D1%82%D0%B5%20%D0%B8%D0%B7%D0%BE%D0%B1%D1%80%D0%B0%D0%B6%D0%B5%D0%BD%D0%B8%D0%B5";
        [Required]
        [DisplayName("Дистанция")]
        public int Distance {get;set;}
        [Required]
        [DisplayName("Время прохождения")]
        public TimeSpan TravelTime {get;set;} = TimeSpan.FromHours(0);
        [DisplayName("Код карты (Iframe)")]
        public string MapCode {get;set;}
        [DisplayName("Код видео (Iframe)")]
        public string VideoCode {get;set;}
        public string QuestCategoryId {get;set;}
        public virtual QuestCategory QuestCategory { get; set; }
    }
}
