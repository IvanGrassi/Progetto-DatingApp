namespace DatingApp.API.Data_Transfer_Objects
{
    public class UserForUpdateDto
    {
        // qui ci sono tutte le proprietà modificabili sul propro profilo user

        public string Introduction { get; set; }
        public string LookingFor { get; set; }  
        public string Interests { get; set; }   
        public string City { get; set; }
        public string Country { get; set; }
    }
}