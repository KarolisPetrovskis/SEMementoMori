namespace MementoMori.Server.DTOS.DeckEditorDTOS
{
    public class DeckUpdateCards
    {
        public required string Id { get; set; }
        public Guid realId { get; set; }
        private Guid _realId;

        public void ConvertRealId(string id)
        {
           if (Guid.TryParse(id, out Guid result))
            {
                realId = result;
            }
            else
            {
                realId = Guid.NewGuid();
            }
        }

        public required string Question { get; set; }

        public string? Description { get; set; }

        public required string Answer { get; set; }

    }
}
