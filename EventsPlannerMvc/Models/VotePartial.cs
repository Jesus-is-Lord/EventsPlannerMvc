using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventsPlannerMvc.Models
{
    public partial class Vote
    {
        public int GetVote
        {
            get {
                var result = 0;


                if (this.VoteCode1.Code.Equals("ONE_STAR"))
                    return 1;
                if (this.VoteCode1.Code.Equals("TWO_STAR"))
                    return 2;
                if (this.VoteCode1.Code.Equals("THREE_STAR"))
                    return 3;
                if (this.VoteCode1.Code.Equals("FOUR_STAR"))
                    return 4;
                if (this.VoteCode1.Code.Equals("FIVE_STAR"))
                    return 5;

                return result;
            }
        }
    }
}