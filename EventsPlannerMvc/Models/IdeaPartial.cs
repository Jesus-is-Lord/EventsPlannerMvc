using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventsPlannerMvc.Models
{
    public partial class Idea
    {
        public decimal GetRating()
        {
            var total = 0;
            var result = 0;
            if (this.Votes.Count != 0)
            {
                foreach (var v in this.Votes)
                {
                    if (v.VoteCode.Equals("ONE_STAR"))
                        total = total + 1;
                    if (v.VoteCode.Equals("TWO_STAR"))
                        total = total + 2;
                    if (v.VoteCode.Equals("THREE_STAR"))
                        total = total + 3;
                    if (v.VoteCode.Equals("FOUR_STAR"))
                        total = total + 4;
                    if (v.VoteCode.Equals("FIVE_STAR"))
                        total = total + 5;
                }
                result = total / Votes.Count;
            }
            return result;
        }
    }
}