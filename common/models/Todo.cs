using Newtonsoft.Json;
using System;

namespace common.models
{
    public class Todo
    {
        [JsonProperty]
        public Guid Id { get; set; }
        [JsonProperty]
        public int? Order { get; set; }
        [JsonProperty]
        public string Title { get; set; }
        [JsonProperty]
        public string Description { get; set; }
        [JsonProperty]
        public DateTime? DueDate { get; set; }
        [JsonProperty]
        public DateTime Created { get; private set; } = DateTime.Now;
        [JsonProperty]
        public bool? Completed { get; set; }
    }
}
