using System;
using System.ComponentModel.DataAnnotations;

namespace Quests.Shared.Entities.Models
{
   public class MyQuest
    {
        [Required]
        public int Id {get;set;}
        [Required]
        public Quest Quest { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public DateTime PurchaseDateTime { get; set; }

        public MyQuestStatus Status { get; set; } = 0;
        public int QuestId { get; set; }

    }

   public enum MyQuestStatus
   {
       NotStarted = 0,
       Started = 1,
       Finished = 2
   }
}
