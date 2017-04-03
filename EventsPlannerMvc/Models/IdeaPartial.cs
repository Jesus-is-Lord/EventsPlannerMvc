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
                var result = 0;
                if (this.Votes.Count != 0)
                {
                    foreach (var v in this.Votes)
                    {
                        if (v.VoteCode1.Code.Equals("ONE_STAR"))
                            total = total + 1;
                        if (v.VoteCode1.Code.Equals("TWO_STAR"))
                            total = total + 2;
                        if (v.VoteCode1.Code.Equals("THREE_STAR"))
                            total = total + 3;
                        if (v.VoteCode1.Code.Equals("FOUR_STAR"))
                            total = total + 4;
                        if (v.VoteCode1.Code.Equals("FIVE_STAR"))
                            total = total + 5;
                    }
                    result = total / Votes.Count;
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