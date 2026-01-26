using CleanCodeLibrary.Domain.Common.Model;
using CleanCodeLibrary.Domain.Persistance.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanCode.Infrastructure
{
    public class Repository<TEntity, TId> : IRepository<TEntity, TId> where TEntity : class //sve sto bi repozitorij trebao imati, mora imat implementirano
    {
        //moramo se nekako spojit na bazu jer sve ove metode rade s bazom...KONTEKST
        private readonly DbContext _context; 
        private readonly DbSet<TEntity> _dbSet; //zasto  private? mozemo svakako minjat iz child preko appl dbcon studeents umisto ovog polja
        //ako je readonly kako ga korisitm
        public Repository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>(); // tablica, ovo dodaje, 
        } 
        public async Task<GetAllResponse<TEntity>> Get() //dohvati sve iz tablice
            //task async, await se koristi, bez toga bi se threadovi potrosili nebi mogli posluzivati druge zahtjeve
        {
            var entites = await _dbSet.ToListAsync(); //u pozadini generira sql query, salje na bazu, baza vraca podatke, ako imamo whereisprid?? pamti to 
            return new GetAllResponse<TEntity> { Values = entites };
        }
        public async Task InsertAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);

            var trackedEntities = _context.ChangeTracker.Entries();

            //await _context.SaveChangesAsync();
        }
        //svaki rad iteracija s bazom je asinkron, kako ovo doli nije?
        public void Update(TEntity entity)
        {
             _dbSet.Update(entity);
        }

        //ili ovako
        public async Task UpdateAsync(TEntity entity)  
        {
            _dbSet.Update(entity);  // u memoriji samo oznaci kao "modified"
       /*     await _context.SaveChangesAsync(); */ // I/O
        }

        public async Task<bool> DeleteAsync(TId id) 
        {
            var entity = await _dbSet.FindAsync(id); //onda je i ovo moglo biti sinkrono, oznaci entitet ako ga nade, dolje istog oznaci obrisanog i tek na save changes se sve odradi
            if (entity != null)
            {
                _dbSet.Remove(entity); //samo oznaci entitet kao obrisan u memoriji aplikacije
                return true;
            }
            else return false;
            

        }

        public void Delete(TEntity? entity) //iako ne treba pretrazivat u bazi, opet triba projverit je li null ?????  upitnici su bitni kod argumenata
        {
            if(entity != null)
            {
                _dbSet.Remove(entity);
            }
        }
       
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
