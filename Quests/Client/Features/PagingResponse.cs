using Quests.Shared.Entities.RequestFeatures;
using System.Collections.Generic;

namespace Quests.Client.Features
{
    public class PagingResponse<T> where T : class
    {
        public List<T> Items { get; set; }
        public MetaData MetaData { get; set; }
    }
}
