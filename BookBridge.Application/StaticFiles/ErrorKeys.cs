namespace BookBridge.Application.StaticFiles
{
    public static class ErrorKeys
    {
        public const string BadRequest = " Bad request, Server do not give good response";
        public const string UnSuccessFullInsert = "Insert was not succesfull";
        public const string UnSuccessFullUpdate = "Update was not succesfully";
        public const string NotFound = "Not Found any relate Entities";
        public const string InternalServerError = " there  was internall error";
        public const string Mapped = "Mapped  not was succesfully";
        public const string General = " General Exception while send request";
        public const string ArgumentNull = " Argument is null , please check";
        public const string NoCategory = " Book Category is null";
        public const string NoBook = "No Such Book Exist in DB!";
        public const string Unauthorized = "User not authenticated";
    }
}
