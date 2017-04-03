using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EventsPlannerMvc.Models
{
    [MetadataType(typeof(IdeaMetaData))]
    public partial class Idea
    {
        public decimal GetRating
        {
            get
            {
                var total = 0;
                decimal result = 0;
                if (this.Votes.Count != 0)
                {
                    foreach (var v in this.Votes)
                    {
                        total = total + v.VoteCode1.Value;
                    }
                    result = Decimal.Divide(total,Votes.Count);
                }
                return result;
            }
        }
    }

    public class IdeaMetaData
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }
    }
}