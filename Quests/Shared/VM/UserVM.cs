using System;
using System.Collections.Generic;
using System.Text;
using Quests.Shared.Entities.Models;

namespace Quests.Shared.VM
{
    public class UserVm
    {
        public string Id { get; set; }
        public string Img { get; set; }
        public string Name { get; set; }
        public string RoleName { get; set; }
        public int Points { get; set; } = 0;
        public string Phone { get; set; }
        public string UserName { get; set; }
        public DateTime? LatestSign { get; set; }
        public int ActiveQuestId { get; set; }
        public int ActiveStateStatus { get; set; }

    }
}
