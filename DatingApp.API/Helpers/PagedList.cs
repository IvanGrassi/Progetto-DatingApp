using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Helpers
{
    public class PagedList<T> : List<T>
    // generica con T, può essere sostituita con User
    {
        // eredita dalla List<User>

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }     // totale delle pagine su cui verranno mostrati gli user
        public int PageSize { get; set; }       // dimensione pagina (quanti users ci vedo)
        public int TotalCount { get; set; }     // totale di tutti gli users

        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);  // ritorna l'int più piccolo possibile rispetto ad un numero dato
            // Esempio: 13 utenti e 5 per pagina: 13/ 5 mi ritornerà 3 (e non 2.6)
            this.AddRange(items);
        }

        // ritorna una nuova istanza di PagedList
        // source = conta di tutti gli users
        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, 
        int pageNumber, int pageSize)
        {
            var count = await source.CountAsync();  // conta di TUTTI gli users
            var items = await source.Skip((pageNumber -1) * pageSize)
            .Take(pageSize).ToListAsync();          
            // skippo (evito) tutti gli elementi della pagina precedente, prendo (take), i 5 elementi successivi
            // se ne ho 13: 5 prima pagina, 5 seconda pagina e 3 terza pagina
            return new PagedList<T>(items, count, pageNumber, pageSize);        
        }

    }
}