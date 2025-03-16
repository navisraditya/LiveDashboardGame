using Postgrest.Attributes;
using Postgrest.Models;
using System;

[Table("scores")]
public class ScoreModel : BaseModel
{
    [PrimaryKey("id", false)]
    public int? Id { get; set; }
    
    [Column("player_id")]
    public string player_id { get; set; }
    
    [Column("score")]
    public int score { get; set; }
    
    [Column("timestamp")]
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    
    [Column("playtime")]
    public float playtime {get; set;}
}