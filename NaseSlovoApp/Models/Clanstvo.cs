//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NaseSlovoApp.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Clanstvo
    {
        public int ClanstvoID { get; set; }
        public int KorisnikID { get; set; }
        public System.DateTime DatumPlat { get; set; }
        public System.DateTime DatumIstek { get; set; }
    
        public virtual Korisnik Korisnik { get; set; }
    }
}