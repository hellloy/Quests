using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace Quests.Shared
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
        [DisplayName("Цена")]
        public int Price {get;set;} = 0;
        [Required]
        [DisplayName("Бесплатный")]
        public bool Free {get;set;} = false;
        [Required]
        [DisplayName("Изображение")]
        public string Img {get;set;}
        [Required]
        [DisplayName("Дистанция")]
        public int Distance {get;set;}
        [Required]
        [DisplayName("Время прохождения")]
        public TimeSpan TravelTime {get;set;}
        [DisplayName("Код карты (Iframe)")]
        public string MapCode {get;set;}
        [DisplayName("Код видео (Iframe)")]
        public string VideoCode {get;set;}
        public string QuestCategoryId {get;set;}
        public virtual QuestCategory QuestCategory { get; set; }
    }
}
