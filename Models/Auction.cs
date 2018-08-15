using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CsBeltExam.Models
{
    public class Auction
    {
        public int AuctionId {get;set;}

        public string ProductName {get;set;}

        public string Description {get;set;}

        public DateTime EndDate {get;set;}

        public Double TopBid {get;set;}

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedAt {get;set;}

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt {get;set;}

        public int CreatorId {get;set;}
        
        public User Creator {get;set;}

        public int TopBidderId {get;set;}

        public User TopBidder {get;set;}

    }
}