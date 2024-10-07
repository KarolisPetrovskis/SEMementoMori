namespace MementoMori.Server{
    public class ErrorHandling
    {
        public string GetErrorMessage(ErrorCode code)
        {
            switch (code)
            {
                case ErrorCode.NotFound:
                    return "The requested resource was not found.";
                case ErrorCode.InvalidInput:
                    return "Invalid input provided.";
                case ErrorCode.DatabaseError:
                    return "An error occurred with the database.";
                case ErrorCode.ServerError:
                    return "A server error occurred.";
                default:
                    return "An unknown error occurred.";
            }
        }
    }
}