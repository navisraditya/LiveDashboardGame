using Newtonsoft.Json;
using Postgrest.Attributes;
using Postgrest.Models;
using System;

[Table("scores")]
public class ScoreModel : BaseModel
{
    [PrimaryKey("id", false)]
    [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)] 
    public int? Id { get; set; }
    
    [Column("player_id")]
    [JsonProperty("player_id")]
    public string Player_id { get; set; }
    
    [Column("score")]
    [JsonProperty("score")]
    public int Score { get; set; }
    
    [Column("timestamp")]
    [JsonProperty("timestamp")]
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    
    [Column("playtime")]
    [JsonProperty("playtime")]
    public float Playtime {get; set;}
}