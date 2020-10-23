using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Quests.Shared.Entities.Models
{
   public class Quest
    {
        [Required]
        public int Id {get;set;}
        [Required (ErrorMessage = "Поле Название обязательное")]
        [DisplayName("Название")]
        public string Name {get;set;}
        [Required (ErrorMessage = "Поле Описание обязательное")]
        [DisplayName("Описание")]
        public string Description {get;set;}
        [Required (ErrorMessage = "Поле Город обязательное")]
        [DisplayName("Город")]
        public string City { get; set; }
        [Required (ErrorMessage = "Поле Цена обязательное")]
        [DisplayName("Цена")]
        public int Price {get;set;} = 0;
        [Required]
        [DisplayName("Бесплатный")]
        public bool Free {get;set;} = false;

        
        [DisplayName("Изображение")]
        public string Img { get; set; } =
            "https://via.placeholder.com/640x480.png?text=%D0%97%D0%B0%D0%B3%D1%80%D1%83%D0%B7%D0%B8%D1%82%D0%B5%20%D0%B8%D0%B7%D0%BE%D0%B1%D1%80%D0%B0%D0%B6%D0%B5%D0%BD%D0%B8%D0%B5";
        [Required (ErrorMessage = "Поле Дистанция обязательное")]
        [DisplayName("Дистанция")]
        public int Distance {get;set;}
        [Required (ErrorMessage = "Поле Время прохождения обязательное")]
        [DisplayName("Время прохождения")]
        public DateTime TravelTime {get;set;} =new DateTime();
        [DisplayName("Код карты (Iframe)")]
        public string MapCode {get;set;}
        [DisplayName("Код видео (Iframe)")]
        public string VideoCode {get;set;}

        public int QuestCategoryId { get; set; } 
        public virtual QuestCategory QuestCategory { get; set; }
        
    }
}
