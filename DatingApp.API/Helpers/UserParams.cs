namespace DatingApp.API.Helpers
{
    public class UserParams
    {
        // contiene i parametri dello user passati tramite metodo GetUser

        private const int MaxPageSize = 50;     // numero massimo di elementi ritornabili per pagina
        public int PageNumber { get; set; } = 1;    // la pagina ovviamente parte da 1
        private int pageSize = 10;      // Totale dei risultati settati per pagina (grandezza default)
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value > MaxPageSize) ? MaxPageSize : value; }
            // se il value à > 50, value viene settato a 50
        }

        // CRITERI DI FILTRAGGIO - STEP 1
        
        public int UserId { get; set; }
        public string Gender { get; set; }
        public int MinAge { get; set; } = 18;   // età minima di 18 anni
        public int MaxAge { get; set; } = 99;   // età massima
        public string OrderBy { get; set; }
    }
}