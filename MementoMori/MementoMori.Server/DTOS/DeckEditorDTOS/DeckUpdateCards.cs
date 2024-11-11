namespace MementoMori.Server.DTOS.DeckEditorDTOS
{
    public class DeckUpdateCards
    {
        public string Id { get; set; }
        public Guid realId { get; set; }
        private Guid _realId;

        public void convertRealId(string id)
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

       // public Guid realId 
       // {
       //     get => _realId;
       //     set => _realId = convertStringToGuid(Id);        
       // }

        private Guid convertStringToGuid(string id)
        {
            if (Guid.TryParse(id, out Guid result))
            {
                return result;
            }
            else
            {
                return Guid.Empty;
            }
        }

        public required string Question { get; set; }

        public string? Description { get; set; }

        public required string Answer { get; set; }

    }
}
