using Postgrest.Attributes;
using Postgrest.Models;
using System;
using Unity.Mathematics;

[Table("scores")]
public class ScoreModel : BaseModel
{
    [PrimaryKey("id", false)]
    public int? Id { get; set; }
    
    [Column("player_id")]
    public string PlayerId { get; set; }
    
    [Column("score")]
    public int ScoreValue { get; set; }
    
    [Column("timestamp")]
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    
    [Column("playtime")]
    public float playtime {get; set;}
}