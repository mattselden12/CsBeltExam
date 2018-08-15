using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CsBeltExam.Models
{
    public class Auction
    {
        public int AuctionId {get;set;}

        [Required]
        [MinLength(4)]
        public string ProductName {get;set;}

        [Required]
        [MinLength(11)]
        public string Description {get;set;}

        [DataType(DataType.DateTime, ErrorMessage = "Invalid Datetime")]
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