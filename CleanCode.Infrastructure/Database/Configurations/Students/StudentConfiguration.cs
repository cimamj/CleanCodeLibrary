using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CleanCodeLibrary.Domain.Entities.Students;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanCode.Infrastructure.Database.Configurations.Users
{//ode mapiramo podatke iz baze u nas entitet , u bazi moze bit drukcija naming konvencija, sealed niko ne nasljeduje od nje
    internal sealed class StudentConfiguration : IEntityTypeConfiguration<Student> //genericki tip, unutar EF , Student iz domain sloja, s builderom manipuliramo propertijima od studenta
    {
        public void Configure(EntityTypeBuilder<Student> builder)  //ugradena metoda
            //ovo je konfiguracija, ne mapiranje, kako mapirati domain klasu Student na tablicu u bazi
            //ovo je POVEZIVANJE propertija iz klase sa stupcima u bazi 
            //nije klasicno mapiranje
            //npr mos zanemariti ovime neki stupac iz baze, ef ce ga ignorirati kod inserta,updatea
            //ja odabirem stupce koje zelim koristiti u svom domain modelu !!!!!
            //
        {
            builder.ToTable("Students"); //kako se zove u bazi ""? Students

            builder.HasKey(t => t.Id); //properti naseg entiteta
            builder.Property(t => t.Id)
                .HasColumnName("Id") //properti nase tablice
                .ValueGeneratedOnAdd();  // ← važno za SERIAL

            builder.Property(t => t.FirstName)
                .HasColumnName("FirstName")
                .HasMaxLength(100) //
                .IsRequired(); //

            builder.Property(t => t.LastName)
                .HasColumnName("LastName")
                .HasMaxLength(100) //
                .IsRequired(); //

            builder.Property(t => t.DateOfBirth)
                .HasColumnName("DateOfBirth");
            
        }
    }
}
