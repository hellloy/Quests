using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Quests.Server.Models
{
    public partial class ApplicationUser
    {
        [Required (ErrorMessage = "Поле Имя обязательное")]
        [DisplayName("Имя")]
        public string FirstName { get; set; }

        [DisplayName("Очки")] 
        public int Points { get; set; } = 0;

        [DisplayName("Аватар")] 
        public string Img { get; set; } = "assets/media/users/blank.png";
        [Required] 
        public bool Confidentiality { get; set; } = false;
        public DateTime? LastLogin { get; set; }
        public int ActiveQuest { get; set; }
        public int ActiveQuestStatus { get; set; }
        public string RoleName { get; set; }
    }
}
