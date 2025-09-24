namespace Bulgarikon.Data.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public DateTime Date { get; set; }
        public string Description { get; set; } = null!;
        public int EraId { get; set; }
        public Era Era { get; set; } = null!;
        public int? CivilizationId { get; set; }
        public Civilization Civilization { get; set; } = null!;
        //public ICollection<Event_Figure> EventFigures { get; set; }
    }
}