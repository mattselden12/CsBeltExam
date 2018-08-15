using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CsBeltExam.Models
{
    public class User
    {
        public int UserId {get;set;}

        public string FirstName {get;set;}

        public string LastName {get;set;}

        public string Username {get;set;}

        public string Password {get;set;}

        public Double Balance {get;set;}

        public Double Available {get;set;}

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedAt {get;set;}
        
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt {get;set;}

        [InverseProperty("Creator")]
        public List<Auction> CreatedAuctions {get;set;}

        [InverseProperty("TopBidder")]
        public List<Auction> TopBidderOn {get;set;}

        public User()
        {
            CreatedAuctions = new List<Auction>();
            TopBidderOn = new List<Auction>();
        }

    }
}